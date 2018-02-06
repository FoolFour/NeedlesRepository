using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using IO = System.IO;

public class StageDataFileLoader : MonoBehaviour
{
    [SerializeField]
    [Header("何も設定しなければ シーン名でロードする")]
    string LoadFile;

    private void Awake()
    {
        string path = Application.streamingAssetsPath + "/Stages/";
        if(LoadFile == "")
        {
            string nowStageData = SceneManager.GetActiveScene().name;
            path += nowStageData + ".sdf";
        }
        else
        {
            path += LoadFile + ".sdf";
        }
        Debug.Log(path);

        using (var fs = new IO.FileStream(path, IO.FileMode.Open))
        {
            using (var br = new IO.BinaryReader(fs))
            {
                string stage_name = br.ReadString();
                float  border1    = br.ReadSingle();
                float  border2    = br.ReadSingle();
                string next_stage = br.ReadString();

                string scene_name = SceneManager.GetActiveScene().name;
                Debug.Log(scene_name);

                PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);
                PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);
                PlayerPrefs.SetFloat (PrefsDataName.Border1,   border1);
                PlayerPrefs.SetFloat (PrefsDataName.Border2,   border2);
                PlayerPrefs.SetString(PrefsDataName.NextSene,  next_stage);

                br.Close();
                fs.Close();
            }
        }
    }
}
