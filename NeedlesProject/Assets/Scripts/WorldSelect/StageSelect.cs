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
            StartCoroutine(AutoRightInput());
            return;
        }

        if(Input.GetAxis(GamePad.Horizontal) < -noticeStickValue)
        {
            //←に入力
            if(selectFlag) { return; }
            Sound.PlaySe("CursorMove");
            info.StageSelectPrev();
            selectFlag = true;
            StartCoroutine(AutoLeftInput());
            return;
        }
        
        selectFlag = false;
        StopAllCoroutines();
    }

    private IEnumerator AutoLeftInput()
    {
        yield return new WaitForSeconds(0.6f);

        var wait = new WaitForSeconds(0.1f);
        while(true)
        {
            Sound.PlaySe("CursorMove");
            info.StageSelectPrev();
            yield return wait;
        }
    }

    private IEnumerator AutoRightInput()
    {
        yield return new WaitForSeconds(0.6f);

        var wait = new WaitForSeconds(0.1f);
        while(true)
        {
            Sound.PlaySe("CursorMove");
            info.StageSelectNext();
            yield return wait;
        }
    }
}
