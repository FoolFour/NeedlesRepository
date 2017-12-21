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
            Debug.Log("ステージに渡す情報");

            //情報を送る
            var stage_info = info.GetSelectStageInfo();

            var scene_name = stage_info.sceneName;
            sceneChange.SceneChange(scene_name);
        }
    }
}
