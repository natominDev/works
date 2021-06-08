using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video : MonoBehaviour
{
    //ワープ中の動画を参照する変数：warpVideo
    public static GameObject warpVideo;

    //飛行機の動画を参照する変数：planeVideo
    public static GameObject planeVideo;

    //船の動画を参照する変数：shipVideo
    public static GameObject shipVideo;

    //列車の動画を参照する変数：trainVideo
    public static GameObject trainVideo;

    //トラベル中の画像を参照する変数：travelingImage
    public static GameObject travelingImage;

    //トラベル中の画像を参照する変数：travelingImage2
    public GameObject travelingImage2;

    

    //最初に呼び出される関数
    void Start()
    {
        //初期化
        warpVideo = GameObject.Find("WarpingVideo");
        planeVideo = GameObject.Find("PlaneVideo");
        shipVideo = GameObject.Find("ShipVideo");
        trainVideo = GameObject.Find("TrainVideo");
        travelingImage = travelingImage2;

        //すべて非アクティブ化
        warpVideo.gameObject.SetActive(false);
        planeVideo.gameObject.SetActive(false);
        trainVideo.gameObject.SetActive(false);
        shipVideo.gameObject.SetActive(false);
        travelingImage.gameObject.SetActive(false);
    }



    //指定した動画を再生する関数
    public static void StartVideo(string sceneName)
    {
        if (!WarpVsTravelButton.GetTravelOn())
        {
            warpVideo.gameObject.SetActive(true);
        }
        //トラベルモード（砂漠）
        else if (sceneName == "DesertScene")
        {
            planeVideo.gameObject.SetActive(true);
        }
        //トラベルモード（南極）
        else if (sceneName == "ArcticScene")
        {
            shipVideo.gameObject.SetActive(true);
        }
        //トラベルモード（熱帯雨林）
        else if (sceneName == "RainforestScene")
        {
            trainVideo.gameObject.SetActive(true);
        }
        //その他の行き先
        else
        {
            planeVideo.gameObject.SetActive(true);
            //travelingImage.gameObject.SetActive(true);
        }
    }



    //すべての動画を止める関数
    public static void StopVideo()
    {
        //ワープ中の画像を非表示に
        warpVideo.gameObject.SetActive(false);
        planeVideo.gameObject.SetActive(false);
        shipVideo.gameObject.SetActive(false);
        trainVideo.gameObject.SetActive(false);
        travelingImage.gameObject.SetActive(false);
    }
}
