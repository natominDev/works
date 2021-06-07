from __future__ import print_function
import pickle
import os.path
from googleapiclient.discovery import build
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request
import subprocess
import datetime,os

#メンバーの名前
Members = ['member1', 'member2', 'member3', 'member4', 'member5', 'member6', 'member7', 'member8', 'member9', 'member10', 'member11', 'member12']
#シートに書き込む際の列指定
Ndic = [chr(66+i) for i in range(len(Members))]
#何回連続で不在だったか
Cnum = 12

SCOPES = ['https://www.googleapis.com/auth/spreadsheets']

#https://docs.google.com/spreadsheets/d/[spreadsheet_id]/edit#gid=0
spreadsheet_id = 'spreadsheet_id'

dt_now = datetime.datetime.now()

#何分前に帰ったかを計算
#minutes=(何回連続で不在か) x (何分ごとに確認するか)
td = datetime.timedelta(minutes = Cnum * 5)

dt_before = dt_now - td
d = '{0}/{1}'.format(dt_now.month, dt_now.day)
t = '{0}:{1:02}'.format(dt_now.hour, dt_now.minute)
_t = '{0}:{1:02}'.format(dt_before.hour, dt_before.minute)


def make_body(mae, now, day):
    global t, _t, Ndic, I, count, Cnum
    i = 0
    data = []
    print(mae, now, count)
    for b, a in zip(mae, now):
        if a != b:
            if a == 1:
                if count[i] > Cnum:
                    v = {}
                    v['range'] = day + '!{0}{1}'.format(Ndic[i], I[i])
                    v['values'] = [[t]]
                    I[i] += 1
                    data.append(v)
                    count[i] = 0
                else:
                    count[i] = 0
            else:
                count[i] = 1
        elif a == 0:
            count[i] += 1
            if count[i] == Cnum:
                v = {}
                v['range'] = day + '!{0}{1}'.format(Ndic[i],I[i])
                v['values'] = [[_t]]
                I[i] += 1
                data.append(v)
        i += 1
    return data


def tomorrow(_d, d, spreadsheet_id):
    global count, I, Ndic, x, Members
    #日付跨いだときにいた人たちの帰宅処理
    i = 0
    data = []
    for b in x:
        if b == 1:
            v = {}
            v['range'] = _d + '!{0}{1}'.format(Ndic[i],I[i])
            #v['majorDimension']="ROWS"
            v['values'] = [[t]]
            data.append(v)
        i += 1
    V = {}
    value_input_option = 'USER_ENTERED'
    V['valueInputOption'] = value_input_option
    V['data'] = data
    print(V['data'])
    if V['data'] != []:
        result = service.spreadsheets().values().batchUpdate(
            spreadsheetId = spreadsheet_id, body = V).execute()
        print('{0} cells updated.'.format(result.get('updatedCells')))

    #次の日のシート作成
    requests = []
    requests.append({
        'addSheet':{
            "properties":{
                "title": d
            }
        }
    })
    body = {'requests': requests}
    spreadsheet = service.spreadsheets().batchUpdate(body = body, spreadsheetId = spreadsheet_id).execute()

    value_input_option = 'USER_ENTERED'
    V = {}
    V['valueInputOption'] = value_input_option
    _ = ['入室','退室']
    style = [{'range' : d + '!A2:A51', 'values' : [[_[i%2]] for i in range((51-2+1)//2)]},
            {'range' : d + '!B1:M1', 'values' : [Members]}]
    V['data']=style
    result = service.spreadsheets().values().batchUpdate(spreadsheetId = spreadsheet_id, body = V).execute()

    #直近の在室状況を初期化
    I = [2] * len(Ndic)
    x = [0] * len(Ndic)
    count = [Cnum + 1] * len(Ndic)


'''
spreadsheetにアクセス
googleのquickstartそのまま
参考：https://qiita.com/connvoi_tyou/items/7cd7ffd5a98f61855f5c
'''
creds = None
if os.path.exists('./in_out_chk/token.pickle'):
    with open('./in_out_chk/token.pickle', 'rb') as token:
        creds = pickle.load(token)
# If there are no (valid) credentials available, let the user log in.
if not creds or not creds.valid:
    if creds and creds.expired and creds.refresh_token:
        creds.refresh(Request())
    else:
        flow = InstalledAppFlow.from_client_secrets_file('./in_out_chk/credentials.json', SCOPES)
        creds = flow.run_local_server()
    # Save the credentials for the next run
    with open('./in_out_chk/token.pickle', 'wb') as token:
        pickle.dump(creds, token)
service = build('sheets', 'v4', credentials = creds)


'''
全員の端末のmac address取得
'''
sheetname = 'sheetname'
range_ = sheetname + "!A1:B{0}".format(len(Members))
result = service.spreadsheets().values().get(spreadsheetId = spreadsheet_id, range = range_).execute()
rows = result.get('values', [])


'''
ネットワークをスキャン
unix os(MacOS)では
sudo arp-scan -I en0 -l
linux(Raspberry pi)では
sudo arp-scan -I wlan0 -l
'''
passwd = ('password\n').encode()
proc = subprocess.run(["sudo", "-S","arp-scan", "-I", "wlan0", "-l"], input = passwd, stdout = subprocess.PIPE, stderr = subprocess.PIPE)
proc = proc.stdout.decode("utf8")
proc = ''.join(proc)


'''
端末が 存在する→1 存在しない→０で格納
'''
y = []
for i, r in enumerate(rows):
    if len(r) == 1:
        y.append(0)
        continue
    if r[1] in proc:
        print(r[0])
        y.append(1)
    else:
        y.append(0)


'''
直近の在室記録（data.pickle）をロード
I 次にシートに書き込む位置
x 前回のチェック時にいた or いなかった
count いないと認識された回数（帰宅したか一時的に端末が認識されなかっただけかを判定）
'''
if os.path.exists('./in_out_chk/data.pickle'):
    with open('./in_out_chk/data.pickle', 'rb') as f:
        I, x, count, _d = pickle.load(f)
else:
    x = [0] * len(Ndic)
    count = [Cnum + 1] * len(Ndic)
    I = [2] * len(Ndic)
    _d = d


'''
シートに帰宅・登校時刻を書き込み
'''
sheetname = d
value_input_option = 'USER_ENTERED'
if _d != d:
    tomorrow(_d, d, spreadsheet_id)
data = make_body(x, y, d)
v = {}
v['range'] = d + '!A1'
v['values'] = [[t]]
data.append(v)
V = {}
V['valueInputOption'] = value_input_option
V['data'] = data
print(V['data'])
if V['data'] != []:
    result = service.spreadsheets().values().batchUpdate(spreadsheetId = spreadsheet_id, body = V).execute()
    print('{0} cells updated.'.format(result.get('updatedCells')))


'''
終了処理として今現在のデータを記録
'''
with open('./in_out_chk/data.pickle', 'wb') as f:
    pickle.dump([I, y, count, d], f)
