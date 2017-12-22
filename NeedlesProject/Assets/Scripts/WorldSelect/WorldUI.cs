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
    StageSceneInfo stageSceneInfo;

    Text           text;

    float          alpha;

    private void Reset()
    {
        worldSelect    = FindObjectOfType<WorldSelect>();
        stageSceneInfo = FindObjectOfType<StageSceneInfo>();
    }

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (worldSelect.IsChangeAnimation)
        {
            alpha = 0.0f;
        }
        else
        {
            alpha += Time.deltaTime;
            text.text = stageSceneInfo.worldList[worldSelect.SelectWorld-1].worldName;
        }
        Color col = text.color;
        col.a = alpha;
        text.color = col;
    }
}
