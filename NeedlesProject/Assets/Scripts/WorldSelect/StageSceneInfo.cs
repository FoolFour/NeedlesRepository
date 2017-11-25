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

    private void Reset()
    {
        worldList.Clear();

        var tmp_obj = GameObject.Find("WorldObject");
        if(tmp_obj == null) { return; }

        int childNum = tmp_obj.transform.childCount;

        for(int i_world = 0; i_world < childNum; i_world++)
        {
            Transform world = tmp_obj.transform.GetChild(i_world);

            worldList.Add(new Stage());
                
            for(int j_stage = 0; j_stage < world.childCount; j_stage++)
            {
                Transform stage = world.GetChild(j_stage);
                string stageName = stage.name;

                StageInfo stageInfo = new StageInfo();
                stageInfo.stageName = stageName;

                worldList[i_world].Add(stageInfo);
            }
        }
    }
}
