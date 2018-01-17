using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(StageBasicInfo))]
public class StageBasicInfoManager : MonoBehaviour
{
    StageBasicInfo info;

    public int selectWorld{ get; private set; }
    public int selectStage{ get; private set; }

    public delegate void SelectWorldChanged(int world);
    public event SelectWorldChanged OnSelectWorldChanged;

    public delegate void SelectStageChanged(int stage);
    public event SelectStageChanged OnSelectStageChanged;

    public int WorldCount
    {
        get { return info.WorldCount; }
    }

    public int StageCountAtNowWorld
    {
        get { return info.StageCount(selectWorld); }
    }

    public string NowSelectedWorldName
    {
        get { return info.worldList[selectWorld].worldName; }
    }

    public bool IsMaxWorld
    {
        get { return selectWorld >= WorldCount-1; }
    }

    public bool IsMinWorld
    {
        get { return selectWorld <= 0; }
    }

    public bool IsMaxStageAtNowWorld
    {
        get { return selectStage >= StageCountAtNowWorld; }
    }

    public bool IsMinStageAtNowWorld
    {
        get { return selectStage <  0; }
    }

    public StageBasicInfo.StageInfo NowStageSelectInfo
    {
        get { return info.worldList[selectWorld][selectStage]; }
    }

    public void WorldSelectNext()
    {
        selectWorld++;

        //配列の範囲外に出ないように
        selectStage = 0;
        selectWorld = Mathf.Min(selectWorld, WorldCount-1);

        SendOnSelectWorldChangedEvent();
    }

    public void WorldSelectPrev()
    {
        selectWorld--;

        //配列の範囲外に出ないように
        selectStage = 0;
        selectWorld = Mathf.Max(selectWorld, 0);

        SendOnSelectWorldChangedEvent();
    }

    public void StageSelectNext()
    {
        selectStage++;
        selectStage = (int)Mathf.Repeat(selectStage, StageCountAtNowWorld);

        SendOnSelectStageChangedEvent();
    }

    public void StageSelectPrev()
    {
        selectStage--;
        selectStage = (int)Mathf.Repeat(selectStage, StageCountAtNowWorld);

        SendOnSelectStageChangedEvent();
    }

    private void Awake()
    {
        info = GetComponent<StageBasicInfo>();
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void Start()
    {
        selectWorld = PlayerPrefs.GetInt(PrefsDataName.SelectedWorld);
        Mathf.Clamp(selectWorld, 0, WorldCount-1);
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        PlayerPrefs.SetInt(PrefsDataName.SelectedWorld, selectWorld);
    }

    private void SendOnSelectWorldChangedEvent()
    {
        if(OnSelectWorldChanged != null)
        {
            OnSelectWorldChanged(selectWorld);
        }
    }

    private void SendOnSelectStageChangedEvent()
    {
        if(OnSelectStageChanged != null)
        {
            OnSelectStageChanged(selectStage);
        }
    }
}
