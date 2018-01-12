using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ResultDataCollector : MonoBehaviour
{
    ///////////////////
    // 変数(private)  /
    /////////////////
    StageData stageData;

    ///////////////////
    // 関数(private)　/
    /////////////////

    private void Awake()
    {
        stageData = GetComponent<StageData>();

        stageData.ApplySceneName(PlayerPrefs.GetString(PrefsDataName.Scene));
        
        stageData.ApplyTime     (PlayerPrefs.GetFloat(PrefsDataName.Time));
        stageData.ApplyStageName(PlayerPrefs.GetString(PrefsDataName.StageName));
        stageData.ApplyBorder1  (PlayerPrefs.GetFloat(PrefsDataName.Border1));
        stageData.ApplyBorder2  (PlayerPrefs.GetFloat(PrefsDataName.Border2));

        SubmitBestTIme(stageData.sceneName, stageData.time);
        SubmitStageClear(stageData.sceneName);

        PlayerPrefs.DeleteKey(PrefsDataName.Scene);
        PlayerPrefs.DeleteKey(PrefsDataName.Time);
        PlayerPrefs.DeleteKey(PrefsDataName.StageName);
        PlayerPrefs.DeleteKey(PrefsDataName.Border1);
        PlayerPrefs.DeleteKey(PrefsDataName.Border2);

        PlayerPrefs.Save();
    }

    private void SubmitBestTIme(string stageName, float new_time)
    {
        var tmp_prefsName = PrefsDataName.StageTime(stageName);
        float old_time = PlayerPrefs.GetFloat(tmp_prefsName);

        if (new_time < old_time)
        {
            //最速クリア更新!
            PlayerPrefs.SetFloat(tmp_prefsName, new_time);
            stageData.ApplyIsNewRecord(true);
        }
        else
        {
            stageData.ApplyIsNewRecord(false);
        }

        //提示された時間内にクリアできているか
        if (new_time < PlayerPrefs.GetFloat(PrefsDataName.Border1))
        {
            string clear_frag = PrefsDataName.Border1ClearFrag(stageData.stageName);
            PlayerPrefs.SetString(clear_frag, bool.TrueString);
            stageData.ApplyIsBorder1Clear(true);
        }
        else
        {
            stageData.ApplyIsBorder1Clear(false);
        }

        if (new_time < PlayerPrefs.GetFloat(PrefsDataName.Border2))
        {
            string clear_frag = PrefsDataName.Border2ClearFrag(stageData.stageName);
            PlayerPrefs.SetString(clear_frag, bool.TrueString);
            stageData.ApplyIsBorder2Clear(true);
        }
        else
        {
            stageData.ApplyIsBorder2Clear(false);
        }
    }

    private void SubmitStageClear(string stageName)
    {
        //ステージクリアにする
        PlayerPrefs.SetString(PrefsDataName.StageClearFrag(stageName), bool.TrueString);
    }
}