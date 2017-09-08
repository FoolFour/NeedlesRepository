using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ジョイスティックの設定
public static class GamePad
{
    // 入力関係
    private static string vertical    = "Vertical";
    public  static string Vertical    { get { return vertical;    } }

    private static string horizontal  = "Horizontal";
    public  static string Horizontal  { get { return horizontal;  } }

    private static string vertical2   = "Vertical2";
    public  static string Vertical2   { get { return vertical2;   } }

    private static string horizontal2 = "Horizontal2";
    public  static string Horizontal2 { get { return horizontal2; } }

    private static bool isDirectMode;

    // ダイレクトモードで設定
    public static void SetDirectMode()
    {
        vertical    = "Vertical";
        horizontal  = "Horizontal";
        vertical2   = "Vertical2";
        horizontal2 = "Horizontal2";

        isDirectMode = false;
    }

    // Xinputモードで設定
    public static void SetXinputMode()
    {
        vertical    = "xVertical";
        horizontal  = "xHorizontal";
        vertical2   = "xVertical2";
        horizontal2 = "xHorizontal2";

        isDirectMode = true;
    }
}
