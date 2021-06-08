using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    //ルーレットを参照する変数：roulette
    public GameObject roulette;

    //ルーレットの回転速度
    public static float rouletteSpeed;

    //正解のシーンを参照する変数：ansScene
    public static string ansScene;

    //最初に呼び出される関数
    void Start()
    {
        //速度を初期化
        rouletteSpeed = 0;
    }

    //フレームごとに呼び出される関数
    void Update()
    {
        //回転
        roulette.gameObject.transform.Rotate(0, 0, rouletteSpeed);
        rouletteSpeed *= 0.97f;
    }

    //ルーレットを回転させる関数
    public static string StartRoulette()
    {
        //乱数生成
        float rn = UnityEngine.Random.value;

        //ルーレットスタート
        rouletteSpeed = -80 * (rn + 1.0f);

        //乱数を表示
        Debug.Log(rn);

        //乱数によって遷移先シーンを決定
        if (rn < 0.33) ansScene = "DesertScene";
        else if (rn < 0.66) ansScene = "ArcticScene";
        else ansScene = "RainforestScene";

        //正解のシーン名を返す
        return ansScene;
    }
}
