using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    StageSceneInfo info;

    [SerializeField]
    CameraControl  control;

    public  int    selectStage;

    private bool   selectFlag;

    private void Reset()
    {
        info    = FindObjectOfType<StageSceneInfo>();
        control = FindObjectOfType<CameraControl>();
    }

    private void Awake()
    {
        selectFlag = false;
    }

    private void OnEnable()
    {
        selectStage      = 0;
        info.selectWorld = control.Current-1;
        info.selectStage = selectStage;
    }

    private void Start()
    {

    }

    private void Update()
    {
        const float noticeStickValue = 0.6f;

        //一気にスクロールするのでselectFlagで制限をかける
        if(Input.GetAxis(GamePad.Horizontal) > noticeStickValue)
        {
            //→に入力
            if(selectFlag) { return; }
            info.SelectStageNext();
            selectFlag = true;
            return;
        }

        if(Input.GetAxis(GamePad.Horizontal) < -noticeStickValue)
        {
            //←に入力
            if(selectFlag) { return; }
            info.SelectStagePrev();
            selectFlag = true;
            return;
        }
        
        selectFlag = false;
    }
}
