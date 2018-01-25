using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WorldUI : MonoBehaviour
{
    [SerializeField]
    WorldSelect    worldSelect;

    [SerializeField]
    StageBasicInfoManager stageBasicInfo;

    [SerializeField]
    int spaceSize = 100;

    string space = "";
    Text           text;

    float          alpha;

    private void Reset()
    {
        worldSelect    = FindObjectOfType<WorldSelect>();
        stageBasicInfo = FindObjectOfType<StageBasicInfoManager>();
    }

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        space = "<size=" + spaceSize + "> </size>";
    }

    private void Update()
    {
        if (worldSelect.IsChangeAnimation)
        {
            alpha = 0.0f;
        }
        else
        {
            if(alpha == 0.0f)
            {
                string worldName = stageBasicInfo.NowSelectedWorldName;
                worldName = worldName.Replace(" ", space);
                text.text = worldName;
            }
            alpha += Time.deltaTime;
        }

        Color col = text.color;
        col.a = alpha;
        text.color = col;
    }
}
