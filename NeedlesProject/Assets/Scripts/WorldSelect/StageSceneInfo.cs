using System.Collections.Generic;
using UnityEngine;
using TimeSpan = System.TimeSpan;

public class StageSceneInfo : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
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

        var tmp_obj = GameObject.Find("WorldObject");
        if(tmp_obj == null) { return; }

        int childNum = tmp_obj.transform.childCount;

        for(int i_w = 0; i_w < childNum; i_w++)
        {
            Transform world = tmp_obj.transform.GetChild(i_w);

            worldList.Add(new Stage());
                
            for(int j_s = 0; j_s < world.childCount; j_s++)
            {
                Transform stage = world.GetChild(j_s);
                string stageName = stage.name;

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
                string tmp;
                tmp = PlayerPrefs.GetString(PrefsDataName.StageClearFrag(stageName));
                info.stageClearFlag    = bool.Parse(tmp);

                tmp = PlayerPrefs.GetString(PrefsDataName.Border1ClearFrag(stageName));
                info.border1ClearFlag = bool.Parse(tmp);

                tmp = PlayerPrefs.GetString(PrefsDataName.Border2ClearFrag(stageName));
                info.border2ClearFlag = bool.Parse(tmp);
            }
        }
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
                PlayerPrefs.SetString(PrefsDataName.StageClearFrag(stageName),    bool.FalseString);

                info.border1ClearFlag = false;
                PlayerPrefs.SetString(PrefsDataName.Border1ClearFrag(stageName), bool.FalseString);

                info.border2ClearFlag = false;
                PlayerPrefs.SetString(PrefsDataName.Border2ClearFrag(stageName), bool.FalseString);
            }
        }
        PlayerPrefs.SetString(PrefsDataName.isInit, "true");
    }

    [ContextMenu("Initialize data")]
    private void InitializeData()
    {
        PlayerPrefs.DeleteAll();
    }
}
