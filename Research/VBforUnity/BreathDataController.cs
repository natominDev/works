using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//csvファイル読み込み用
using System.IO;

public class BreathDataController : MonoBehaviour
{
    //スクリプトで制御する部位
    //body: 腹部、shoulderL: 左肩、shoulderR: 右肩
    public GameObject body, shoulderL, shoulderR;

    //振幅
    private float value, bodyScaleY, shoulderScaleZ;

    //csvファイル
    TextAsset csvFile;

    //ファイル内のデータを格納するリスト
    List<string[]> csvDatas = new List<string[]>();

    //ファイル名
    public string fileName;

    //カウンタ
    int itr;

    //各ボーンのtransformを格納する変数
    float bodyX, bodyY, bodyZ;
    float shoulderLX, shoulderLY, shoulderLZ;
    float shoulderRX, shoulderRY, shoulderRZ;

    //開始時に呼び出される関数
    void Start()
    {
        //csvファイルの読み込み
        csvFile = Resources.Load(fileName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        //1行ずつ読み込み、','で区切る
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }

        //カウンタの初期化
        itr = 0;

        //腹部のボーンのスケールを取得
        bodyX = body.transform.localScale.x;
        bodyY = body.transform.localScale.y;
        bodyZ = body.transform.localScale.z;

        //左肩のボーンの傾きを取得
        shoulderLX = shoulderL.transform.localEulerAngles.x;
        shoulderLY = shoulderL.transform.localEulerAngles.y;
        shoulderLZ = shoulderL.transform.localEulerAngles.z;

        //右肩のボーンの傾きを取得
        shoulderRX = shoulderR.transform.localEulerAngles.x;
        shoulderRY = shoulderR.transform.localEulerAngles.y;
        shoulderRZ = shoulderR.transform.localEulerAngles.z;
    }

    //フレームごとに呼び出される関数
    void Update()
    {
        //リストから振幅を読み出す
        value = float.Parse(csvDatas[itr][1]);

        //カウントアップ
        itr = (itr + 1) % csvDatas.Count;

        //腹部の振幅を計算
        bodyScaleY = 0.3f * bodyY * value + bodyY;

        //腹部のボーンサイズを変更
        body.transform.localScale = new Vector3(bodyX, bodyScaleY , bodyZ);

        //両肩の傾きを計算
        shoulderScaleZ = 2.5f * value;

        //両肩のボーンの傾きを変更
        shoulderL.transform.rotation = Quaternion.Euler(shoulderLX, shoulderLY, shoulderLZ + shoulderScaleZ);
        shoulderR.transform.rotation = Quaternion.Euler(shoulderRX, shoulderRY, - shoulderLZ - shoulderScaleZ );
    }
}
