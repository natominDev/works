using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TimeNowScript : MonoBehaviour
{
    //時間を表示するテキストを参照する変数：clockText
    private Text clockText;

    //開始時に呼び出される関数
    void Start()
    {
        //コンポーネントを取得
        clockText = GetComponentInChildren<Text>();
    }

    //フレームごとに呼び出される関数
    void Update()
    {
        //現在の日時を取得し、文字列型へ変換したものにテキストを書き換える
        clockText.text = DateTime.Now.ToString();
    }
}
