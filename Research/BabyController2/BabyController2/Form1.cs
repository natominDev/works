using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//csvファイル読み込み用
using System.IO;
//シリアルポート通信用
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BabyController2
{
    public partial class Form1 : Form
    {
        //シリアルポート用の変数
        public SerialPort myport;

        //ファイル内のデータを格納するリスト
        List<string[]> csvDatas = new List<string[]>();

        //送信データを格納するリスト
        List<int> sendDatas = new List<int>();

        //COMポート名：初代はCOM3、2代目はCOM4にする
        private string comName = "COM4";

        public Form1()
        {
            InitializeComponent();

            ReadCsVFile();

            //ViewCsvDatas();

            ProcessingDatas();

            ViewSendDatas();

            SendDatas();
        }

        //csvファイルを読み込んでcsvDatas[]に格納する関数
        private void ReadCsVFile()
        {
            //csvファイルの読み込み
            string filePath = @"C:\Users\itohg\Desktop\natomin\BabyController2\BabyController2\Resources\cosCSV.csv";
            StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));

            //1行ずつ読み込み、','で区切る
            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }

            //csvファイルを閉じる
            reader.Close();
        }

        //csvファイルの内容を確認する関数
        void ViewCsvDatas()
        {
            //csvファイルの内容を出力
            for (int i = 0; i < csvDatas.Count; ++i)
            {
                for (int j = 0; j < csvDatas[i].Length; ++j)
                {
                    System.Diagnostics.Debug.Write(csvDatas[i][j] + ",");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        //csvDatasの傾きの正負で区間分割を行う関数
        void ProcessingDatas()
        {
            //カウント用変数
            int cnt = 0;

            //今までの傾きを表す変数
            bool up = true;

            for (int i = 0; i < csvDatas.Count - 1; ++i)
            {
                if (csvDatas[i].Length < 1) continue;
                if (csvDatas[i + 1].Length < 1) continue;

                //string型のデータをfloatへ変換
                float val1 = float.Parse(csvDatas[i][1]);
                float val2 = float.Parse(csvDatas[i + 1][1]);

                //傾きがひとつ前と同じならカウントアップ
                if ((val1 > val2) ^ (up))
                {
                    ++cnt;
                }
                //傾きが変化していればリストに追加し、カウントをリセット
                else
                {
                    sendDatas.Add(cnt);
                    cnt = 1;
                    up = !up;
                }
            }

            //最後のカウントをリストに追加
            sendDatas.Add(cnt);
        }

        void ViewSendDatas()
        {
            for (int i = 0; i < sendDatas.Count; ++i)
            {
                if (sendDatas[i] == 0) continue;
                System.Diagnostics.Debug.WriteLine(sendDatas[i]);
            }
        }

        void SendDatas()
        {
            try
            {
                myport = new SerialPort();
                myport.BaudRate = 9600;
                myport.PortName = comName;
                myport.Open();

                //'r'を送信（リセット）
                myport.WriteLine("r");

                //'d'を送信（データ送信開始の合図）
                myport.WriteLine("d");

                //処理したデータを順に送信
                for (int i = 0; i < sendDatas.Count; ++i)
                {
                    if (sendDatas[i] == 0) continue;
                    myport.WriteLine(sendDatas[i].ToString() + "c");
                }

                //'e'を送信（データ送信終了の合図）
                myport.WriteLine("e");

                myport.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error!");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //TEMP_ONボタン
        private void button1_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = comName;
            myport.Open();
            //'o'を送信
            myport.WriteLine("o");
            myport.Close();
        }

        //TEMP_OFFボタン
        private void button2_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = comName;
            myport.Open();
            //'p'を送信
            myport.WriteLine("p");
            myport.Close();
        }

        //AIR_ONボタン
        private void button3_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = comName;
            myport.Open();
            //'a'を送信
            myport.WriteLine("a");
            myport.Close();
        }

        //AIR_OFFボタン
        private void button4_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = comName;
            myport.Open();
            //'b'を送信
            myport.WriteLine("b");
            myport.Close();
        }
    }
}
