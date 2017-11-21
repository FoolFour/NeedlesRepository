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

    [Space()]
    [Header("下のチェックボックスで強制的にXinputに")]
    [SerializeField]
    private bool debugXInputMode;
#endif

    //様々な可能性を考慮

    readonly string[] detectList = new []
    {
        "Xinput",  "X input",  "X-input",  "X_input",
        "Xbox360", "Xbox 360", "Xbox-360", "Xbox_360",
    };

    private void Awake()
    {
#if UNITY_EDITOR
        if(debugXInputMode)
        {
            Log("強制的にX Inputの設定にします");
            GamePad.SetXinputMode();
            Destroy(this);
            return;
        }
#endif

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
        //ゲームパッドによってリストの文字が全て小文字の場合や全て大文字の可能性を考慮し
        //大文字と小文字を無視して判定するようにする
        var option = System.StringComparison.OrdinalIgnoreCase;

        foreach(string controllerName in detectList)
        {
            int result = joystickName.IndexOf(controllerName, option);
            if (result != -1) { return true; }
        }

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
