using System.Collections.Generic;
using UnityEngine;
using TimeSpan = System.TimeSpan;

public class StageSceneInfo : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public string worldName;
        public List<StageInfo> stageList;

        public Stage()
        {
            stageList = new List<StageInfo>();
        }

        public int Count
        {
            get { return stageList.Count; }
        }

        public void Add(StageInfo stageInfo)
        {
            stageList.Add(stageInfo);
        }
        public StageInfo this [int i]
        {
            get { return stageList[i]; }
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
    public List<Stage> worldList;

    public int selectWorld;
    public int selectStage;

    public StageInfo GetSelectStageInfo()
    {
        return worldList[selectWorld][selectStage];
    }

    public int GetSelectWorldStageNum()
    {
        return worldList[selectWorld].Count;
    }

    public string GetSelectWorldName()
    {
        return worldList[selectWorld].worldName;
    }

    /// <summary>次のステージを選択状態にする</summary>
    public void SelectStageNext()
    {
        int tmp =  worldList[selectWorld].Count;
        selectStage = (int)Mathf.Repeat(selectStage+1, tmp);
    }

    /// <summary>前のステージを選択状態にする</summary>
    public void SelectStagePrev()
    {
        int tmp =  worldList[selectWorld].Count;
        selectStage = (int)Mathf.Repeat(selectStage-1, tmp);
    }

    private void Reset()
    {
        worldList.Clear();

        for(int i_w = 0; i_w < 3; i_w++)
        {
            worldList.Add(new Stage());
                
            for(int j_s = 0; j_s < 3; j_s++)
            {
                string stageName = "World " + j_s;

                StageInfo stageInfo = new StageInfo();
                stageInfo.stageName = stageName;

                worldList[i_w].Add(stageInfo);
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
        for(int i_w = 0; i_w < worldList.Count; i_w++)
        {
            for(int j_s = 0; j_s < worldList[i_w].Count; j_s++)
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

        for(int i_w = 0; i_w < worldList.Count; i_w++)
        {
            for(int j_s = 0; j_s < worldList[i_w].Count; j_s++)
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
        Stage newStage = new Stage();
        for(int i = 0; i < 3; i++)
        {
            StageInfo info = new StageInfo();
            info.stageName = "Stage" + (i+1);
            newStage.Add(info);
        }

        var tempList = new List<Stage>();
        tempList.Add(newStage);
        tempList.AddRange(worldList);

        worldList = new List<Stage>(tempList);
    }
}
