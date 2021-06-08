using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessModeImage : MonoBehaviour
{
    //砂漠シナリオの枠組みを表示する画像を参照する変数：desertImage
    public static GameObject desertImage;

    //南極シナリオの枠組みを表示する画像を参照する変数：arcticImage
    public static GameObject arcticImage;

    //熱帯雨林シナリオの枠組みを表示する画像を参照する変数：rainforestImage
    public static GameObject rainforestImage;

    //ファイナルアンサーの画像を参照する変数：finalAnswearImage
    public static GameObject finalAnswearImage;

    //砂漠シナリオの枠組みを表示する画像を参照する変数：desertImage2
    public GameObject desertImage2;

    //南極シナリオの枠組みを表示する画像を参照する変数：arcticImage2
    public GameObject arcticImage2;

    //熱帯雨林シナリオの枠組みを表示する画像を参照する変数：rainforestImage2
    public GameObject rainforestImage2;

    //ファイナルアンサーの画像を参照する変数：finalAnswearImage2
    public GameObject finalAnswearImage2;



    //最初に呼び出される関数
    void Start()
    {
        desertImage = desertImage2;
        arcticImage = arcticImage2;
        rainforestImage = rainforestImage2;
        finalAnswearImage = finalAnswearImage2;
    }



    //推測モードにおける画像のアクティブ化を司る関数
    public static void ActiveImage2(GameObject activeImage, bool flag)
    {
        //砂漠の画像を非アクティブ化
        desertImage.gameObject.SetActive(false);

        //南極の画像を非アクティブ化
        arcticImage.gameObject.SetActive(false);

        //熱帯雨林の画像を非アクティブ化
        rainforestImage.gameObject.SetActive(false);

        //引数の画像のみアクティブ化
        //フラグが立っていないときはすべてを非アクティブ化
        if (flag) activeImage.gameObject.SetActive(true);

        //ファイナルアンサーテキストを表示
        finalAnswearImage.gameObject.SetActive(true);
    }



    //砂漠シナリオを選択する関数
    public void AnsDesert()
    {
        // 砂漠シナリオの画像をアクティブ化
        ActiveImage2(desertImage, true);
    }



    //南極シナリオを選択する関数
    public void AnsArctic()
    {
        //南極シナリオの画像をアクティブ化
        ActiveImage2(arcticImage, true);
    }



    //熱帯雨林シナリオを選択する関数
    public void AnsRainforest()
    {
        // 熱帯雨林シナリオの画像をアクティブ化
        ActiveImage2(rainforestImage, true);
    }



    //すべてを非アクティブ化
    public static void NonActive()
    {
        //すべての画像を非アクティブ化
        ActiveImage2(desertImage, false);

        //ファイナルアンサーテキストを非アクティブ化
        finalAnswearImage.gameObject.SetActive(false);
    }
}
