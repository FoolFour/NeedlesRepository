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

            var stage_name = stage_info.stageName;
            Debug.Log("ステージ名:"     + stage_name);
            PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);

            var scene_name = stage_info.sceneName;
            Debug.Log("シーン名:"       + scene_name);
            PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);

            var border1   = stage_info.border1;
            Debug.Log("ボーダータイム1:" + border1);
            PlayerPrefs.SetFloat(PrefsDataName.Border1,    border1);

            var border2   = stage_info.border2;
            Debug.Log("ボーダータイム2:" + border2);
            PlayerPrefs.SetFloat(PrefsDataName.Border2,    border2);

            sceneChange.SceneChange(scene_name);
        }
    }
}
