using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSubmitToSceneChange : MonoBehaviour
{
    [SerializeField]
    private SceneChangeFade sceneChange;

    [SerializeField]
    private StageBasicInfoManager info;

    StageSelect select;

    private void Reset()
    {
        sceneChange = FindObjectOfType<SceneChangeFade>();
        info        = FindObjectOfType<StageBasicInfoManager>();
    }

    private void Awake()
    {
        select = GetComponent<StageSelect>();
    }

    private void Update()
    {
        if(Input.GetButtonUp(GamePad.Submit))
        {
            select.isSelect = false;

            //情報を送る
            var stage_info = info.NowStageSelectInfo;

            var scene_name = stage_info.sceneName;
            sceneChange.SceneChange(scene_name);
        }
    }
}
