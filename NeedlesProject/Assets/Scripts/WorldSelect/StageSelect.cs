﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    StageBasicInfoManager info;

    [SerializeField]
    CameraControl  control;

    [SerializeField]
    public Sprite[] sprite;

    [SerializeField]
    Image backgroundImage;

    public  int    selectStage;
    public  bool   isSelect;
    
    private bool   selectFlag;
    
    private void Reset()
    {
        info    = FindObjectOfType<StageBasicInfoManager>();
        control = FindObjectOfType<CameraControl>();
    }

    private void Awake()
    {
        selectFlag = false;
        isSelect   = true;
    }

    private void OnEnable()
    {
        selectStage      = 0;

        backgroundImage.sprite = sprite[info.selectWorld];
    }

    private void Start()
    {

    }

    private void Update()
    {
        const float noticeStickValue = 0.6f;

        if(!isSelect) { return; }

        //一気にスクロールするのでselectFlagで制限をかける
        if(Input.GetAxis(GamePad.Horizontal) > noticeStickValue)
        {
            //→に入力
            if(selectFlag) { return; }
            Sound.PlaySe("CursorMove");
            info.StageSelectNext();
            selectFlag = true;
            return;
        }

        if(Input.GetAxis(GamePad.Horizontal) < -noticeStickValue)
        {
            //←に入力
            if(selectFlag) { return; }
            Sound.PlaySe("CursorMove");
            info.StageSelectPrev();
            selectFlag = true;
            return;
        }
        
        selectFlag = false;
    }
}
