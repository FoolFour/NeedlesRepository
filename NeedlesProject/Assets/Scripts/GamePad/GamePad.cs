using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ジョイスティックの設定
public static class GamePad
{
    // 入力関係
    private static string vertical    = VERTICAL;
    public  static string Vertical     { get { return vertical;     } }

    private static string horizontal  = HORIZONTAL;
    public  static string Horizontal   { get { return horizontal;   } }

    private static string vertical2   = VERTICAL   + "2";
    public  static string Vertical2    { get { return vertical2;    } }

    private static string horizontal2 = HORIZONTAL + "2";
    public  static string Horizontal2  { get { return horizontal2;  } }

    private static string submit      = "Submit";
    public  static string Submit       { get { return submit;       } }

    private static string cancel      = "Cancel";
    public  static string Cancel       { get { return cancel;       } }

    private static string pause       = "Pause";
    public  static string Pause        { get { return pause;        } }

    private static bool   isDirectMode;
    public  static bool   IsDirectMode { get { return isDirectMode; } }

    private static bool   isFlipAnalog;
    public  static bool   IsFlipAnalog { get { return isFlipAnalog; } }

    const string VERTICAL     = "Vertical";
    const string HORIZONTAL   = "Horizontal";
    const string X_VERTICAL   = "x" + VERTICAL;
    const string X_HORIZONTAL = "x" + HORIZONTAL; 

    // ダイレクトモードで設定
    public static void SetDirectMode()
    {
        if(isFlipAnalog)
        {
            vertical    = VERTICAL   + "2";
            horizontal  = HORIZONTAL + "2";
            vertical2   = VERTICAL;
            horizontal2 = HORIZONTAL;
        }
        else
        {
            vertical    = VERTICAL;
            horizontal  = HORIZONTAL;
            vertical2   = VERTICAL   + "2";
            horizontal2 = HORIZONTAL + "2";
        }
        isDirectMode = false;
    }

    // Xinputモードで設定
    public static void SetXinputMode()
    {
        

        if (isFlipAnalog)
        {
            vertical    = X_VERTICAL   + "2";
            horizontal  = X_HORIZONTAL + "2";
            vertical2   = X_VERTICAL;
            horizontal2 = X_HORIZONTAL;
        }
        else
        {
            vertical    = X_VERTICAL;
            horizontal  = X_HORIZONTAL;
            vertical2   = X_VERTICAL   + "2";
            horizontal2 = X_HORIZONTAL + "2";
        }
        
        isDirectMode = true;
    }

    public static void SetFlipAnalog(bool isFlip)
    {
        isFlipAnalog = isFlip;
        if (isDirectMode)
        {
            SetDirectMode();
        }
        else
        {
            SetXinputMode();
        }
    }

    /// <summary>スティックが右方向に入力されているか</summary>
    public static bool IsStickRightInclined(float limit)
    {
        limit = Mathf.Abs(limit);
        return Input.GetAxis(horizontal) >=  limit;
    }

    /// <summary>スティックが左方向に入力されているか</summary>
    public static bool IsStickLeftInclined (float limit)
    {
        limit = Mathf.Abs(limit);
        return Input.GetAxis(horizontal) <= -limit;
    }

    /// <summary>スティックが上方向に入力されているか</summary>
    public static bool IsStickUpInclined   (float limit)
    {
        limit = Mathf.Abs(limit);
        return Input.GetAxis(vertical)   <= -limit;
    }

    /// <summary>スティックが下方向に入力されているか</summary>
    public static bool IsStickDownInclined (float limit)
    {
        limit = Mathf.Abs(limit);
        return Input.GetAxis(vertical)   >=  limit;
    }
}
