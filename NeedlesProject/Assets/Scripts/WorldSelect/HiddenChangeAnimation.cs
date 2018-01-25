using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HiddenChangeAnimation : MonoBehaviour
{
    [SerializeField]
    WorldSelect    worldSelect;

    [SerializeField]
    StageBasicInfoManager stageBasicInfo;

    Graphic graphic;

    private void Reset()
    {
        worldSelect    = FindObjectOfType<WorldSelect>();
        stageBasicInfo = FindObjectOfType<StageBasicInfoManager>();
    }

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void Update()
    {
        float alpha = graphic.color.a;

        if(worldSelect.IsChangeAnimation)
        {
            alpha = 0.0f;
        }
        else
        {
            alpha += Time.deltaTime;
        }

        Color col = graphic.color;
        col.a = alpha;
        graphic.color = col;
    }
}
