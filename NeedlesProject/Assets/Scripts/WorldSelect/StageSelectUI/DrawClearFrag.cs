using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DrawClearFrag : MonoBehaviour
{
    enum ClearFlag
    {
        StageClear,
        Border1Clear,
        Border2Clear,
    }

    
    [SerializeField]
    ClearFlag getClearFlag;
    [SerializeField]
    StageBasicInfoManager info;
    
    Image image;

    private void Reset()
    {
        info = FindObjectOfType<StageBasicInfoManager>();
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image.enabled = false;
    }

    private void Update()
    {
        bool flag = GetClearFlag();
        image.enabled = flag;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(flag);
        }
    }

    private bool GetClearFlag()
    {
        var tmp = info.NowStageSelectInfo;
        if(getClearFlag == ClearFlag.StageClear  ) { return tmp.stageClearFlag;   }
        if(getClearFlag == ClearFlag.Border1Clear) { return tmp.border1ClearFlag; }
        if(getClearFlag == ClearFlag.Border2Clear) { return tmp.border2ClearFlag; }

        throw null;
    }
}
