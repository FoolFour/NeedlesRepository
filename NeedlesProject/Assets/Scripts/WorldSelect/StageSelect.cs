using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    StageSceneInfo info;

    public int selectStage;

    private bool selectFlag;

    private void Reset()
    {
        info = FindObjectOfType<StageSceneInfo>();
    }

    private void Awake()
    {
        selectFlag = false;
    }

    private void OnEnable()
    {
        selectStage      = 0;
        info.selectStage = selectStage;
    }

    private void Start()
    {

    }

    private void Update()
    {
        const float noticeStickValue = 0.6f;

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

        //それ以外の場合
        selectFlag = false;
    }
}
