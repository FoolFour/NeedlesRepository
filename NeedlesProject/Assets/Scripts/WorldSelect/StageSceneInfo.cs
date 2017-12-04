using System.Collections.Generic;
using UnityEngine;

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
        public string sceneName;
        public string mission1;
        public string mission2;
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

    [ContextMenu("ShowSceneName")]
    private void ShowSceneName()
    {
        Debug.Log(worldList[0][0].sceneName);
    }
}
