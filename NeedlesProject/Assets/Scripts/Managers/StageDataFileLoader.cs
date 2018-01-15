using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using IO = System.IO;

public class StageDataFileLoader : MonoBehaviour
{
    public string nowStageData;
    public string nextStageScene;

    private void Awake()
    {
        if(nowStageData == "")
        {
            //ファイル指定無し(デフォルト値)
            Debug.LogError("ファイルが指定されていません デフォルトの設定を適用します");
            LoadDefault();
        }
        else
        {
            //ファイル指定有り
            LoadFile();
        }
        
        PlayerPrefs.SetString(PrefsDataName.NextSene, nextStageScene);
    }

    private void LoadDefault()
    {
        const string stage_name = "無名ステージ";
        const float  border1    = 100;
        const float  border2    =  50;

        string scene_name = SceneManager.GetActiveScene().name;
        Debug.Log(scene_name);

        PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);
        PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);
        PlayerPrefs.SetFloat (PrefsDataName.Border1,   border1);
        PlayerPrefs.SetFloat (PrefsDataName.Border2,   border2);
    }

    private void LoadFile()
    {
        string path = Application.streamingAssetsPath + "/" + nowStageData;
        using (var fs = new IO.FileStream(path, IO.FileMode.Open))
        {
            using (var br = new IO.BinaryReader(fs))
            {
                string stage_name = br.ReadString();
                float  border1    = br.ReadSingle();
                float  border2    = br.ReadSingle();

                string scene_name = SceneManager.GetActiveScene().name;
                Debug.Log(scene_name);

                PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);
                PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);
                PlayerPrefs.SetFloat (PrefsDataName.Border1,   border1);
                PlayerPrefs.SetFloat (PrefsDataName.Border2,   border2);

                br.Close();
                fs.Close();
            }
        }
    }
}
