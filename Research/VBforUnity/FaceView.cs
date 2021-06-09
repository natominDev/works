using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceView : MonoBehaviour
{
    public GameObject head;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private int mouthOpenIndex;
    private int mouthSizeIndex;
    private int eyeLIndex;
    private int eyeRIndex;
    private int browLIndex;

    private int mouthSecCnt;
    private int mouthOpenCnt;

    int cnt;

    void Start()
    {
        //head = GameObject.Find("natomi/ItSeez3D Head");
        //skinnedMeshRenderer = head.GetComponent<SkinnedMeshRenderer>();
        mouthOpenIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("AA");
        mouthSizeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("smile");
        eyeLIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("EyeBlink_L");
        eyeRIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("EyeBlink_R");
        browLIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("BrowsD_L");

        mouthOpenCnt = 10;
        mouthSecCnt = 0;
        cnt = 50;
    }

    

    public void SetMouthVal(float open, float size)
    {
        cnt = 30;

        if (open > 0)
        {
            if(--mouthOpenCnt < 0) mouthOpenCnt = 0;
            
            if(mouthOpenCnt == 0) skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenIndex, open * 50);
        }
        else
        {
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenIndex, 0);
            mouthOpenCnt = 10;
        }

        if (size == -1)
        {
            if (--mouthSecCnt < 0) mouthSecCnt = 0;
            if(mouthSecCnt == 0) skinnedMeshRenderer.SetBlendShapeWeight(mouthSizeIndex, 0);
        }
        else
        {
            size -= 0.5f;
            if (size < 0) size = 0;
            else size = size * 8.0f;
            
            if (size * 50 > 0.9f) mouthSecCnt = 50;
            else
            {
                if (--mouthSecCnt <= 0)
                {
                    mouthSecCnt = 0;

                }
            }
            if (mouthSecCnt > 0) skinnedMeshRenderer.SetBlendShapeWeight(mouthSizeIndex, size * 50);
        }
        /*
        if(open > 0.3f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(mouthSizeIndex, size * 50);
        }
        else
        {
            skinnedMeshRenderer.SetBlendShapeWeight(mouthSizeIndex, 0);
        }
        */
    }
    /*
    [SerializeField] SkinnedMeshRenderer MTH_DEF;
    [SerializeField] SkinnedMeshRenderer BLW_DEF;
    [SerializeField] SkinnedMeshRenderer EYE_DEF;
    [SerializeField] SkinnedMeshRenderer EYE_DEF2;
    */
    
    public enum EyeState { 
        Open,
        Half,
        Close
    }
    
    [Range(0, 100)]
    public float EyeParam = 0;
    float leftEyeParam = 0;
    float rightEyeParam = 0;

    
    public enum BrowState
    {
        High,
        Middle,
        Low
    }

    [Range(0, 100)]
    public float BrowParam = 0;
    float leftBrowParam = 0;
    float rightBrowParam = 0;

    [Range(0, 100)]
    public float MouthParam = 0;

    public bool IsDebug = true;

    public float speed = 2;



    void Update()
    {
        if (--cnt < 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenIndex, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(mouthSizeIndex, 0);
        }
        if (cnt < 0) cnt = 0;
        
        
        if (IsDebug)
        {
            
            skinnedMeshRenderer.SetBlendShapeWeight(eyeLIndex, leftEyeParam);
            skinnedMeshRenderer.SetBlendShapeWeight(eyeRIndex, rightEyeParam);
            skinnedMeshRenderer.SetBlendShapeWeight(browLIndex, leftBrowParam);
            //EYE_DEF.SetBlendShapeWeight(eyeIndex, EyeParam);
            //EYE_DEF2.SetBlendShapeWeight(eyeIndex, EyeParam);
            //BLW_DEF.SetBlendShapeWeight(browIndex, BrowParam);
        }
        else
        {
            //skinnedMeshRenderer.SetBlendShapeWeight(eyeLIndex, EyeParam);
            //EYE_DEF.SetBlendShapeWeight(eyeIndex, Mathf.Lerp(EYE_DEF.GetBlendShapeWeight(eyeIndex), eyeParam, Time.deltaTime * speed * 5));
            //EYE_DEF2.SetBlendShapeWeight(eyeIndex, Mathf.Lerp(EYE_DEF2.GetBlendShapeWeight(eyeIndex), eyeParam, Time.deltaTime * speed * 5));
            //BLW_DEF.SetBlendShapeWeight(eyeIndex, Mathf.Lerp(BLW_DEF.GetBlendShapeWeight(browIndex), browParam, Time.deltaTime * speed * 5));
        }
        
    }
    
    public void SetLeftEyeState(EyeState state)
    {
        if (state == EyeState.Open)
        {
            leftEyeParam = 0;
        }
        else if (state == EyeState.Half)
        {
            leftEyeParam = 50;
        }
        else
        {
            leftEyeParam = 100;
        }
    }

    public void SetRightEyeState(EyeState state)
    {
        if (state == EyeState.Open)
        {
            rightEyeParam = 0;
        }
        else if (state == EyeState.Half)
        {
            rightEyeParam = 50;
        }
        else
        {
            rightEyeParam = 100;
        }
    }

    public void SetLeftBrowState(BrowState state)
    {
        if(state == BrowState.High)
        {
            leftBrowParam = 100;
        }
        else if(state == BrowState.Middle)
        {
            leftBrowParam = 50;
        }
        else
        {
            leftBrowParam = 0;
        }
    }
    
}
