using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    //タイトル画面を参照する変数：titleScreen
    public GameObject titleScreen;

    //ワールドツアーモードの画面を参照する変数：worldTourScreen
    public GameObject worldTourScreen;

    //身体探しモードの画面を参照する変数：questionModeScreen
    public GameObject questionModeScreen;

    //推測画面を参照する変数：guessScreen
    public GameObject guessScreen;

    //タイトル画面か否か参照する変数：isTitle
    private bool isTitle;

    //ワールドツアー画面か否か参照する変数：isWorldTour
    private bool isWorldTour;

    //身体探しモード画面か否かを参照する変数：isQuestion
    private bool isQuestion;

    //推測画面か否かを参照する変数：isGuess
    private bool isGuess;

    //タイトル画面へ戻るボタンを参照する変数：returnTitleButton
    public GameObject returnTitleButton;
        
    //背景画像を参照する変数：backGroundImage
    public GameObject backGroundImage;

    //正解のシーン名を参照する変数：ansScene
    private string ansScene;

    //説明テキストを参照する変数：expText
    public GameObject expText;

    //各シーンに対し、正解テキストを表示するかを渡すための変数：displayAns
    public static bool displayAns;



    //最初に呼び出される関数
    void Start()
    {
        //タイトル画面を表示
        isTitle = true;
        isWorldTour = false;
        isQuestion = false;
        isGuess = false;

        //タイトル画面へ戻るボタンを非アクティブ化
        returnTitleButton.gameObject.SetActive(false);

        //正解テキストは表示しないように
        displayAns = false;
    }



    //フレームごとに呼び出される関数
    void Update()
    {
        //すべてのスクリーンをオフに
        SetScreenOff(true);

        //タイトル画面
        if (isTitle)
        {
            titleScreen.gameObject.SetActive(true);
        }
        //ワールドツアーモード
        else if(isWorldTour)
        {
            worldTourScreen.gameObject.SetActive(true);
        }
        //身体探しモード
        else if (isQuestion)
        {
            questionModeScreen.gameObject.SetActive(true);
        }
        //推測モード
        else if (isGuess)
        {
            guessScreen.gameObject.SetActive(true);
        }
        //モード選択をしていないとき
        else
        {
            SetScreenOff(false);
        }
    }



    //すべてのスクリーンを非アクティブ化する関数
    //引数は背景をアクティブ化するかどうか
    public void SetScreenOff(bool flag = true)
    {
        //タイトル画面を非アクティブ化
        titleScreen.gameObject.SetActive(false);

        //ワールドツアーモード画面を非アクティブ化
        worldTourScreen.gameObject.SetActive(false);

        //身体探しモード画面を非アクティブ化
        questionModeScreen.gameObject.SetActive(false);

        //推測モード画面を非アクティブ化
        guessScreen.gameObject.SetActive(false);

        //背景を非アクティブ化
        backGroundImage.gameObject.SetActive(flag);
    }



    //ワールドツアーモードへ遷移する関数
    public void WorldTourStart()
    {
        //画面設定
        isTitle = false;
        isWorldTour = true;

        //タイトル画面へ戻るボタンをアクティブ化
        returnTitleButton.gameObject.SetActive(true);
    }



    //ワープを実行する関数
    //引数は行き先シーン、ワールドツアーモードかどうかを示す
    public void Warping(string sceneName, bool flag)
    {
        //ワールドツアーモードを終了
        isWorldTour = false;

        returnTitleButton.gameObject.SetActive(false);

        //動画を再生
        Video.StartVideo(sceneName);

        //コルーチン開始
        StartCoroutine(TimeCoroutine(sceneName, 5.0f, flag));
    }



    //時間停止のコルーチン
    private IEnumerator TimeCoroutine(string sceneName, float waitTime, bool flag)
    {
        //waitTime秒間待機
        yield return new WaitForSeconds(waitTime);

        //動画を停止
        Video.StopVideo();

        //ワールドツアーモード
        if (flag)
        {
            //引数で指定したシナリオへ遷移
            SceneManager.LoadScene(sceneName);
        }
        //身体探しモード
        else
        {
            //推測モードへ
            isGuess = true;
            returnTitleButton.gameObject.SetActive(true);
        }
    }



    //砂漠シナリオを選択する関数
    public void SelectDesertImage()
    {
        //ワープを実行
        Warping("DesertScene", true);
    }



    //南極シナリオを選択する関数
    public void SelectArcticImage()
    {
        //ワープを実行
        Warping("ArcticScene", true);
    }



    //熱帯雨林シナリオを選択する関数
    public void SelectRainforestImage()
    {
        //ワープを実行
        Warping("RainforestScene", true);
    }



    //タイトル画面へ選択する関数
    public void TitleStart()
    {
        //リターンボタン等を非アクティブ化
        GuessModeImage.NonActive();

        //タイトル画面を表示
        isTitle = true;
        isWorldTour = false;
        isQuestion = false;
        isGuess = false;

        //タイトル画面へ戻るボタンを非アクティブ化
        returnTitleButton.gameObject.SetActive(false);
    }



    //身体探しモードへ遷移する関数
    public void QuestionModeStart()
    {
        //タイトル画面へ戻るボタンをアクティブ化
        returnTitleButton.gameObject.SetActive(true);

        //画面設定
        isTitle = false;
        isQuestion = true;

        //正解のシーンをルーレットで決定
        ansScene = Roulette.StartRoulette();

        //コルーチン開始
        StartCoroutine("TimeCoroutine2");
    }



    //時間停止のコルーチンその２
    private IEnumerator TimeCoroutine2()
    {
        //3秒間待機
        yield return new WaitForSeconds(3.0f);

        //身体探しモード終了
        isQuestion = false;

        //推測モードへ
        Warping("", false);
    }

    

    //正解シーンへ遷移する関数
    public void ToAnsScene()
    {
        //すべての画像を非アクティブ化
        GuessModeImage.NonActive();

        //説明テキストを表示
        expText.gameObject.SetActive(true);

        //コルーチン開始
        StartCoroutine("TimeCoroutine3");
    }



    //時間停止のコルーチンその3
    private IEnumerator TimeCoroutine3()
    {
        //3秒間待機
        yield return new WaitForSeconds(3.0f);

        //説明テキストを非表示に
        expText.gameObject.SetActive(false);

        //シーンへ渡すフラグを立てる
        displayAns = true;

        //引数で指定したシナリオへ遷移
        SceneManager.LoadScene(ansScene);
    }



    //各シーンが正解テキストを表示するか聞いてきたときに呼び出される関数
    public static bool getDisplayAns() { return displayAns; }
}

