using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    //正解テキストを参照する変数：ansText
    public GameObject ansText;

    //開始時に呼び出される関数
    void Start()
    {
        //正解テキストを表示するかどうか
        if (GameManager.getDisplayAns())
        {
            //正解テキストをアクティブ化
            ansText.gameObject.SetActive(true);
        }
        else
        {
            //正解テキストを非アクティブ化
            ansText.gameObject.SetActive(false);
        }
    }
}
