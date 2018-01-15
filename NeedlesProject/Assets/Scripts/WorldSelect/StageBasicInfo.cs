using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeSpan = System.TimeSpan;
using IO = System.IO;

public class StageBasicInfo : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public string worldName;
        public StageInfo[] stageList;

        public Stage()      { stageList = new StageInfo[ ] { }; }
        public Stage(int i) { stageList = new StageInfo[i];     }

        public int Length
        {
            get { return stageList.Length; }
        }

        public StageInfo this [int i]
        {
            get { return stageList[i]; }
            set { stageList[i] = value; }
        }
    }

    [System.Serializable]
    public class StageInfo
    {
        public string stageName;
        public bool   stageClearFlag;
        public string sceneName;
        public float  border1;
        public bool   border1ClearFlag;
        public float  border2;
        public bool   border2ClearFlag;
        public float  time;
    }
    
    [SerializeField]
    public Stage[] worldList;

    public int WorldCount
    {
        get { return worldList.Length; }
    }

    public int StageCount(int world)
    {
        return worldList[world].Length;
    }

    private void Reset()
    {
        worldList = new Stage[3];

        for(int i_w = 0; i_w < 3; i_w++)
        {
            worldList[i_w] = new Stage(3);
                
            for(int j_s = 0; j_s < 3; j_s++)
            {
                string stageName = "World " + j_s;

                StageInfo stageInfo = new StageInfo();
                stageInfo.stageName = stageName;

                worldList[i_w][j_s] = stageInfo;
            }
        }
    }

    private void Awake()
    {
        //初期化
        bool isInit = PlayerPrefs.HasKey(PrefsDataName.isInit);
        
        if(isInit)
        { 
            LoadTime();
        }
        else
        {
            InitTime();
        }
    }

    private void LoadTime()
    {
        for(int i_w = 0; i_w < worldList.Length; i_w++)
        {
            for(int j_s = 0; j_s < worldList[i_w].Length; j_s++)
            {
                var info = worldList[i_w][j_s];
                string stageName = info.stageName;
                
                float  time      = PlayerPrefs.GetFloat(PrefsDataName.StageTime(stageName));
                info.time = time;

                //クリアしているかどうかの情報
                info.  stageClearFlag = GetClearFlag(PrefsDataName.  StageClearFrag(stageName));
                info.border1ClearFlag = GetClearFlag(PrefsDataName.Border1ClearFrag(stageName));
                info.border2ClearFlag = GetClearFlag(PrefsDataName.Border2ClearFrag(stageName));
            }
        }
    }

    private bool GetClearFlag(string prefsData)
    {
        string tmp = PlayerPrefs.GetString(prefsData);

        bool result;
        if(bool.TryParse(tmp, out result))
        {
            return result;
        }

        //初期値を入れる
        PlayerPrefs.SetString(prefsData, bool.FalseString);
        return false;
    }

    private void InitTime()
    {
        TimeSpan timeSpan = new TimeSpan(
            days:           0, 
            hours:          0, 
            minutes:       59, 
            seconds:       59, 
            milliseconds: 999
        );

        for(int i_w = 0; i_w < worldList.Length; i_w++)
        {
            for(int j_s = 0; j_s < worldList[i_w].Length; j_s++)
            {
                string stageName = worldList[i_w][j_s].stageName;

                float time = (float)timeSpan.TotalSeconds;
                var info = worldList[i_w][j_s];

                info.time = time;
                PlayerPrefs.SetFloat(PrefsDataName.StageTime(stageName), time);

                info.stageClearFlag = false;
                PlayerPrefs.SetString(PrefsDataName.StageClearFrag(stageName),   bool.FalseString);

                info.border1ClearFlag = false;
                PlayerPrefs.SetString(PrefsDataName.Border1ClearFrag(stageName), bool.FalseString);

                info.border2ClearFlag = false;
                PlayerPrefs.SetString(PrefsDataName.Border2ClearFrag(stageName), bool.FalseString);
            }
        }
        PlayerPrefs.SetString(PrefsDataName.isInit, "true");

        PlayerPrefs.Save();
    }

    [ContextMenu("Initialize data")]
    private void InitializeData()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("AddFrontList")]
    private void AddFront()
    {
        Stage newStage = new Stage(3);
        for(int i = 0; i < 3; i++)
        {
            StageInfo info = new StageInfo();
            info.stageName = "Stage" + (i+1);
            newStage[i] = info;
        }

        var tempList = new List<Stage>();
        tempList.Add(newStage);
        tempList.AddRange(worldList);

        worldList = tempList.ToArray();
    }

    [ContextMenu("Generate File")]
    public void GenerateFile()
    {
        var directory = Application.streamingAssetsPath + "/Stages";

        //初期化
        if(IO.Directory.Exists(directory))
        {
            IO.Directory.Delete(directory, true);
        }

        IO.Directory.CreateDirectory(directory);

        for(int i_w = 0; i_w < worldList.Length; i_w++)
        {
            for(int j_s = 0; j_s < worldList[i_w].Length; j_s++)
            {
                var s = new System.Text.StringBuilder();

                s.Append(directory);
                s.Append("/");
                s.Append(worldList[i_w].worldName);

                if (!IO.Directory.Exists(s.ToString()))
                {
                    IO.Directory.CreateDirectory(s.ToString());
                }
                
                s.Append("/");
                s.Append(worldList[i_w][j_s].stageName);
                s.Append(".sdf");
                using (var fs = new IO.FileStream(s.ToString(), IO.FileMode.Create))
                {
                    using (var bw = new IO.BinaryWriter(fs))
                    {
                        var info = worldList[i_w][j_s];
                        bw.Write(info.stageName);
                        bw.Write(info.border1);
                        bw.Write(info.border2);

                        bw.Close();
                        fs.Close();
                    }
                }
            }
        }
    }
}
