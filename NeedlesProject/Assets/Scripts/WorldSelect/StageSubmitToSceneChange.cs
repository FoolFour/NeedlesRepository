﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSubmitToSceneChange : MonoBehaviour
{
    [SerializeField]
    private SceneChangeFade sceneChange;

    [SerializeField]
    private StageBasicInfoManager info;

    private void Reset()
    {
        sceneChange = FindObjectOfType<SceneChangeFade>();
        info        = FindObjectOfType<StageBasicInfoManager>();
    }

    private void Update()
    {
        if(Input.GetButton(GamePad.Submit))
        {
            //情報を送る
            var stage_info = info.NowStageSelectInfo;

            var scene_name = stage_info.sceneName;
            sceneChange.SceneChange(scene_name);
        }
    }
}
