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
        using (var fs = new IO.FileStream(nowStageData, IO.FileMode.Open))
        {
            using (var br = new IO.BinaryReader(fs))
            {
                string stage_name = br.ReadString();
                string scene_name = br.ReadString();
                float  border1    = br.ReadSingle();
                float  border2    = br.ReadSingle();

                PlayerPrefs.SetString(PrefsDataName.StageName, stage_name);
                PlayerPrefs.SetString(PrefsDataName.Scene,     scene_name);
                PlayerPrefs.SetFloat (PrefsDataName.Border1,   border1);
                PlayerPrefs.SetFloat (PrefsDataName.Border2,   border2);

                br.Close();
                fs.Close();
            }
        }
        
        PlayerPrefs.SetString(PrefsDataName.NextSene, nextStageScene);
    }
}
