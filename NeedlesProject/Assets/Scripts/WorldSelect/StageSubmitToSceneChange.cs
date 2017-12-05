using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSubmitToSceneChange : MonoBehaviour
{
    [SerializeField]
    private SceneChangeFade sceneChange;

    [SerializeField]
    private StageSceneInfo info;

    private void Reset()
    {
        sceneChange = FindObjectOfType<SceneChangeFade>();
        info        = FindObjectOfType<StageSceneInfo >();
    }

    private void Update()
    {
        if(Input.GetButton(GamePad.Submit))
        {
            //情報を送る
            var stage_info = info.GetSelectStageInfo();

            var stage_name = stage_info.stageName;
            PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);

            var scene_name = stage_info.sceneName;
            PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);

            var mission1   = stage_info.mission1;
            PlayerPrefs.SetString(PrefsDataName.Mission1,  mission1);

            var mission2   = stage_info.mission2;
            PlayerPrefs.SetString(PrefsDataName.Mission2,  mission2);

            sceneChange.SceneChange(scene_name);
        }
    }
}
