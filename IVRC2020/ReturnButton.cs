using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    //ボタンをクリックするとスタート画面へ戻る関数
    public void OnClickStartButton()
    {
        //MainSceneをロード
        SceneManager.LoadScene("MainScene");
    }
}
