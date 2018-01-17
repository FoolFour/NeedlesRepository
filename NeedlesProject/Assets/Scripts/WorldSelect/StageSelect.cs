using UnityEngine;
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
    Sprite[] worldSprites;

    [SerializeField]
    Image backgroundImage;

    public  int    selectStage;

    private bool   selectFlag;

    private void Reset()
    {
        info    = FindObjectOfType<StageBasicInfoManager>();
        control = FindObjectOfType<CameraControl>();
    }

    private void Awake()
    {
        selectFlag = false;
    }

    private void OnEnable()
    {
        selectStage      = 0;

        backgroundImage.sprite = worldSprites[info.selectWorld];
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
