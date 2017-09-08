using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class DetectXinput : MonoBehaviour
{

    [SerializeField]
    private bool   detectOnlyOnce;

    private string useControllerName = "";
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
        Log("コントローラーのタイプの判別開始");
        var joysticks = Input.GetJoystickNames();

        bool isDetected = false;

        //1Pのコントローラーが常に配列の0番目にあるとは限らない為
        foreach (string name in joysticks)
        {
            if (name != "")
            {
                SetControllerMode(name);
                useControllerName = name;
                isDetected = true;
            }
        }

        if (!isDetected)
        {
            Log("<color=orange>コントローラーの判別失敗 :</color> コントローラーが接続されていません");
        }
        Log("コントローラの判別処理をを終了します");
    }

    private void Start()
    {
        if (detectOnlyOnce)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        var  joysticks  = Input.GetJoystickNames();
        bool isDetected = false;

        foreach (string name in joysticks)
        {
            if (name == "") { continue; }

            isDetected = true;
            if (name == useControllerName) { break; }

            Log("コントローラーのタイプの判別開始");
            SetControllerMode(name);
            useControllerName = name;
            Log("コントローラの判別処理をを終了します");
        }

        if (!isDetected)
        {
            useControllerName = "";
        }
    }

    private void SetControllerMode(string joystickName)
    {
        Log("使うコントローラーの名前 : " + joystickName);
        if (IsXinput(joystickName))
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

    private bool IsXinput(string joystickName)
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
