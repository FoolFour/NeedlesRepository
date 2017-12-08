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
    StageSceneInfo info;


    Image image;

    private void Reset()
    {
        info = FindObjectOfType<StageSceneInfo>();
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        image.enabled = GetClearFrag();
    }

    private bool GetClearFrag()
    {
        var tmp = info.GetSelectStageInfo();
        if(getClearFlag == ClearFlag.StageClear   ) { return tmp.stageClearFlag;    }
        if(getClearFlag == ClearFlag.Border1Clear) { return tmp.border1ClearFlag; }
        if(getClearFlag == ClearFlag.Border2Clear) { return tmp.border2ClearFlag; }

        throw null;
    }
}
