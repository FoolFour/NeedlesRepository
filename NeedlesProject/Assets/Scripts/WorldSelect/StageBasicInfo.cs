using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeSpan = System.TimeSpan;
using IO = System.IO;

public class StageBasicInfo : MonoBehaviour
{
    [System.Serializable]
    public class World
    {
        public string worldName;
        public StageInfo[] stageList;

        public World()      { stageList = new StageInfo[ ] { }; }
        public World(int i) { stageList = new StageInfo[i];     }

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
        public bool   isTutorial;
    }
    
    [SerializeField]
    public World[] worldList;

    /// <summary>ワールド数</summary>
    public int WorldCount
    {
        get { return worldList.Length; }
    }

    /// <summary>ステージ数</summary>
    public int StageCount(int world)
    {
        return worldList[world].Length;
    }

    private void Reset()
    {
        worldList = new World[3];

        for(int i_w = 0; i_w < 3; i_w++)
        {
            worldList[i_w] = new World(3);
                
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
        //初期化したか
        bool isInit = PlayerPrefs.HasKey(PrefsDataName.isInit);
        
        if(isInit)
        { 
            LoadData();
        }
        else
        {
            InitTime();
        }
    }

    private void LoadData()
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
        //59:59.99を初期値とする
        TimeSpan timeSpan = new TimeSpan(
            days:           0, 
            hours:          0, 
            minutes:       59, 
            seconds:       59, 
            milliseconds: 999
        );

        for(int i_w = 0; i_w < WorldCount; i_w++) {
        for(int j_s = 0; j_s < StageCount(i_w); j_s++)
        {
            string stageName = worldList[i_w][j_s].stageName;

            float time = (float)timeSpan.TotalSeconds;
            StageInfo info = worldList[i_w][j_s];

            info.time = time;
            PlayerPrefs.SetFloat (PrefsDataName.StageTime(stageName), time);

            info.stageClearFlag   = false;
            PlayerPrefs.SetString(PrefsDataName.StageClearFrag(stageName),   bool.FalseString);

            info.border1ClearFlag = false;
            PlayerPrefs.SetString(PrefsDataName.Border1ClearFrag(stageName), bool.FalseString);
            
            info.border2ClearFlag = false;
            PlayerPrefs.SetString(PrefsDataName.Border2ClearFrag(stageName), bool.FalseString);
        } }
        PlayerPrefs.SetString(PrefsDataName.isInit, bool.TrueString);

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
        World newStage = new World(3);
        for(int i = 0; i < 3; i++)
        {
            StageInfo info = new StageInfo();
            info.stageName = "Stage " + (i+1); 
            newStage[i] = info;
        }

        var tempList = new List<World>();
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

        for(int i_w = 0; i_w < WorldCount;      i_w++) {
        for(int j_s = 0; j_s < StageCount(i_w); j_s++) {
            var s = new System.Text.StringBuilder();
            string sceneName = IO.Path.GetFileNameWithoutExtension(worldList[i_w][j_s].sceneName);

            s.Append(directory);
            s.Append("/");
            s.Append(sceneName);
            s.Append(".sdf");
            using (var fs = new IO.FileStream(s.ToString(), IO.FileMode.Create)) {
            using (var bw = new IO.BinaryWriter(fs)) {

                StageInfo info = worldList[i_w][j_s];
                bw.Write(info.stageName);
                bw.Write(info.border1);
                bw.Write(info.border2);
                bw.Write(info.isTutorial);
                    
                if(j_s < StageCount(i_w)-1)
                {
                    bw.Write(worldList[i_w][j_s+1].sceneName);
                }
                else
                {
                    bw.Write("");
                }

                bw.Close();
                fs.Close();
            } }
        } }
    }
}
