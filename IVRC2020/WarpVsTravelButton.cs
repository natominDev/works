using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpVsTravelButton : MonoBehaviour
{
    //ワープの設定
    public static bool travelOn;

    //ワープボタンを参照する変数：warpButton
    public GameObject warpButton;

    //トラベルボタンを参照する変数：travelButton
    public GameObject travelButton;

    //開始時に呼び出される関数
    void Start()
    {
        //ワープモードをデフォルトに
        travelOn = false;
    }

    //フレームごとに呼び出される関数
    void Update()
    {
        //ワープ/トラベルボタンの切り替え
        travelButton.gameObject.SetActive(travelOn);
        warpButton.gameObject.SetActive(!travelOn);
    }

    //ワープ/トラベルモードを切り替える関数
    public void WarpSetting()
    {
        travelOn = !travelOn;
    }

    //ワープ設定を返す関数
    public static bool GetTravelOn()
    {
        return travelOn;
    }
}
