using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class DetectXinput : MonoBehaviour
{
    [SerializeField]
    private int useGamePadNumber;

#if UNITY_EDITOR
    [SerializeField]
    private bool showLog;
#endif

    //様々な可能性を考慮
    const string xinput1 = " Xinput";
    const string xinput2 = " X input";
    const string xinput3 = " X-input";
    const string xinput4 = " X_input";

    private void Awake()
    {
        Log("コントローラーがXinputかどうか開始");
        var joysticks = Input.GetJoystickNames();
        if (joysticks.Length >= 1)
        {
            Log("使うコントローラーの名前 : " + joysticks[useGamePadNumber]);

            if (IsXinput(joysticks[useGamePadNumber]))
            {
                Log("Xinput検知！ Xinput用の設定を適用します");
                GamePad.SetXinputMode();
            }
            else
            {
                Log("Xinput以外を検知！ Direct用の設定を適用します");
                GamePad.SetDirectMode();
            }
        }
        else
        {
            Log("<color=0x888800>コントローラーの判別失敗 :</color> コントローラーが接続されていません");
        }
        Log("コントローラの判別処理をを終了します");
    }

    bool IsXinput(string joystickName)
    {
        //ゲームパッドによって[Xinput]の文字が全て小文字の場合や全て大文字の可能性を考慮し
        //大文字と小文字を無視して判定するようにする
        var option = System.StringComparison.OrdinalIgnoreCase;
        int result;

        result = joystickName.IndexOf(xinput1, option);
        if (result != -1) { return true; }

        result = joystickName.IndexOf(xinput2, option);
        if (result != -1) { return true; }

        result = joystickName.IndexOf(xinput3, option);
        if (result != -1) { return true; }

        result = joystickName.IndexOf(xinput4, option);
        if (result != -1) { return true; }

        return false;
    }

    private void Log(object message)
    {
#if UNITY_EDITOR
        if (showLog)
        {
            Debug.Log(message);
        }
#endif
    }
}
