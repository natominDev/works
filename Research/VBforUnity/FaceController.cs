using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class FaceController : MonoBehaviour
{
    // FaceView component
    FaceView faceView;

    // Bandsize
    const int MAX_SIZE = 16;

    // Left Eye Parameters
    private int[] leftEyeLog;
    int leftEyeSum;
    int leftEyeItr;

    // Right Eye Parameters
    private int[] rightEyeLog;
    int rightEyeSum;
    int rightEyeItr;

    // Left Brow Parameters
    private int[] leftBrowLog;
    int leftBrowSum;
    int leftBrowItr;

    private void Awake()
    {
        faceView = GetComponent<FaceView>();

        // Initialize to left eye parameters
        leftEyeLog = new int[MAX_SIZE];
        for (int i = 0; i < MAX_SIZE; ++i) leftEyeLog[i] = 0;
        leftEyeSum = 0;
        leftEyeItr = 0;

        // Initialize to right eye parameters
        rightEyeLog = new int[MAX_SIZE];
        for (int i = 0; i < MAX_SIZE; ++i) rightEyeLog[i] = 0;
        rightEyeSum = 0;
        rightEyeItr = 0;

        // Initialize to left brow parameters
        leftBrowLog = new int[MAX_SIZE];
        for (int i = 0; i < MAX_SIZE; ++i) leftBrowLog[i] = 0;
        leftBrowSum = 0;
        leftBrowItr = 0;
    }

    public void FaceModelUpdate(Point[] points)
    {
        calcStateLeftEye(points);

        calcStateRightEye(points);

        calcStateLeftBrow(points);
        /*
        //mouth open
        float mouthOpen = getRatioOfMouthOpen_Y(points);
        mouthOpen *= 0.4f;
        if (mouthOpen < 0.4f) mouthOpen = 0;

        //mouth size
        float mouthSize = getRatioOfMouthSize(points);
        if (mouthSize < 0f) mouthSize = -1;

        faceView.SetMouthVal(mouthOpen, mouthSize);
        */
    }

    

    private void calcStateLeftEye(Point[] points)
    {
        float ratio = getRatioOfEyeOpen_L(points);

        if(ratio >= 0.85f)
        {
            leftEyeSum -= leftEyeLog[leftEyeItr];
            leftEyeLog[leftEyeItr] = 1;
            leftEyeSum += 1;
        }
        else
        {
            leftEyeSum -= leftEyeLog[leftEyeItr];
            leftEyeLog[leftEyeItr] = 0;
            leftEyeSum += 0;
        }

        if (leftEyeSum == MAX_SIZE)
        {
            faceView.SetLeftEyeState(FaceView.EyeState.Open);
            //Debug.Log("L Open");
        }
        else if (leftEyeSum >= MAX_SIZE*4/5)
        {
            faceView.SetLeftEyeState(FaceView.EyeState.Half);
            //Debug.Log("L Half");
        }
        else
        {
            faceView.SetLeftEyeState(FaceView.EyeState.Close);
            //Debug.Log("L Close");
        }

        leftEyeItr = (leftEyeItr + 1) % MAX_SIZE;
    }

    private void calcStateRightEye(Point[] points)
    {
        float ratio = getRatioOfEyeOpen_R(points);

        if (ratio >= 0.85f)
        {
            rightEyeSum -= rightEyeLog[rightEyeItr];
            rightEyeLog[rightEyeItr] = 1;
            rightEyeSum += 1;
        }
        else
        {
            rightEyeSum -= rightEyeLog[rightEyeItr];
            rightEyeLog[rightEyeItr] = 0;
            rightEyeSum += 0;
        }

        if (rightEyeSum == MAX_SIZE)
        {
            faceView.SetRightEyeState(FaceView.EyeState.Open);
            //Debug.Log("R Open");
        }
        else if (rightEyeSum >= MAX_SIZE*4/5)
        {
            faceView.SetRightEyeState(FaceView.EyeState.Half);
            //Debug.Log("R Half");
        }
        else
        {
            faceView.SetRightEyeState(FaceView.EyeState.Close);
            //Debug.Log("R Close");
        }

        rightEyeItr = (rightEyeItr + 1) % MAX_SIZE;
    }

    private void calcStateLeftBrow(Point[] points)
    {
        float ratio = getRatioOfBrow_L_Y(points);
        /*
        ratio *= 10f;
        int tmp = (int)ratio;
        Debug.Log("R: " + tmp);
        ratio /= 10f;
        */
        if (ratio <= 0.3f)
        {
            leftBrowSum -= leftBrowLog[leftBrowItr];
            leftBrowLog[leftBrowItr] = 1;
            leftBrowSum += 1;
            //Debug.Log("H");
        }
        else
        {
            leftBrowSum -= leftBrowLog[leftBrowItr];
            leftBrowLog[leftBrowItr] = 0;
            leftBrowSum += 0;
            //Debug.Log("M");
        }

        if (leftBrowSum >= MAX_SIZE*4/5)
        {
            faceView.SetLeftBrowState(FaceView.BrowState.High);
        }
        else if (leftBrowSum <= -MAX_SIZE*4/5)
        {
            faceView.SetLeftBrowState(FaceView.BrowState.Low);
        }
        else
        {
            faceView.SetLeftBrowState(FaceView.BrowState.Middle);
        }
    }

    public float getRatioOfEyeOpen_L(Point[] points) {
        if (points.Length != 69)
            return 0f;
        return Mathf.Clamp(Mathf.Abs(points[44].Y - points[48].Y) / (Mathf.Abs(points[44].X - points[45].X) * 0.8f), -0.1f, 2.0f);
    }

    public float getRatioOfEyeOpen_R(Point[] points)
    {
        if (points.Length != 69)
            return 0f;
        return Mathf.Clamp(Mathf.Abs(points[39].Y - points[41].Y) / (Mathf.Abs(points[39].X - points[38].X) * 0.8f), -0.1f, 2.0f);
    }

    public float getRatioOfBrow_L_Y(Point[] points) { 
        if(points.Length != 69)
            return 0f;
        float y = Mathf.Abs(points[22].Y - points[42].Y) / Mathf.Abs(points[25].Y - points[45].Y);
        //y -= 1;
        //y *= 4f;
        return y;
        //return Mathf.Clamp(y, -1.0f, 1.0f);
    }
    
    public float getRatioOfMouthOpen_Y(Point[] points) {
        if (points.Length != 69)
            return 0f;
        return Mathf.Clamp01(Mathf.Abs(points[63].Y - points[67].Y) * 1.5f / (Mathf.Abs(points[52].Y - points[63].Y) + Mathf.Abs(points[67].Y - points[58].Y)));
    }

    public float getRatioOfMouthSize(Point[] points) {
        if (points.Length != 69)
            return 0f;
        float size = Mathf.Abs(points[49].X - points[55].X) / (Mathf.Abs(points[32].X - points[36].X) * 1.8f);
        size -= 1;
        size *= 4f;
        return Mathf.Clamp(size, -1.0f, 1.0f);
    }

}
