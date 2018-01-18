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

    ////////////////////////
    // プロパティ(public)  /
    /////////////////////

    /// <summary>ワールド数</summary>
    public int WorldCount
    {
        get { return info.WorldCount; }
    }

    /// <summary>現在選択されているワールドのステージ数</summary>
    public int StageCountAtNowWorld
    {
        get { return info.StageCount(selectWorld); }
    }

    /// <summary>現在選択されているワールドの名前</summary>
    public string NowSelectedWorldName
    {
        get { return info.worldList[selectWorld].worldName; }
    }

    /// <summary>現在選択されているワールドが最後のワールドか</summary>
    public bool IsLastWorld
    {
        get { return selectWorld >= WorldCount-1; }
    }

    /// <summary>現在選択されているワールドが最初のワールドか</summary>
    public bool IsFirstWorld
    {
        get { return selectWorld <= 0; }
    }

    /// <summary>現在選択されているステージが現在のワールドの最後のステージか</summary>
    public bool IsLastStageAtNowWorld
    {
        get { return selectStage >= StageCountAtNowWorld; }
    }

    /// <summary>現在選択されているステージが現在のワールドの最初のステージか</summary>
    public bool IsFirstStageAtNowWorld
    {
        get { return selectStage <  0; }
    }

    /// <summary>現在選択されているステージの情報</summary>
    public StageBasicInfo.StageInfo NowStageSelectInfo
    {
        get { return info.worldList[selectWorld][selectStage]; }
    }

    /// <summary>次のワールドに進む</summary>
    public void WorldSelectNext()
    {
        selectWorld++;

        //配列の範囲外に出ないように
        selectStage = 0;
        selectWorld = Mathf.Min(selectWorld, WorldCount-1);

        SendOnSelectWorldChangedEvent();
    }

    /// <summary>前のワールドに進む</summary>
    public void WorldSelectPrev()
    {
        selectWorld--;

        //配列の範囲外に出ないように
        selectStage = 0;
        selectWorld = Mathf.Max(selectWorld, 0);

        SendOnSelectWorldChangedEvent();
    }

    /// <summary>次のステージに進む</summary>
    public void StageSelectNext()
    {
        selectStage++;
        selectStage = (int)Mathf.Repeat(selectStage, StageCountAtNowWorld);

        SendOnSelectStageChangedEvent();
    }

    /// <summary>前のステージに進む</summary>
    public void StageSelectPrev()
    {
        selectStage--;
        selectStage = (int)Mathf.Repeat(selectStage, StageCountAtNowWorld);

        SendOnSelectStageChangedEvent();
    }

    private void Awake()
    {
        info        = GetComponent<StageBasicInfo>();
        selectWorld = PlayerPrefs.GetInt(PrefsDataName.SelectedWorld);
        Debug.Log(selectWorld);
        Mathf.Clamp(selectWorld, 0, WorldCount-1);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    /// <summary>シーンが切り替わった時のイベント</summary>
    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        Debug.Log(selectWorld);
        PlayerPrefs.SetInt(PrefsDataName.SelectedWorld, selectWorld);
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        Destroy(gameObject);
    }

    /// <summary>ワールドが切り替わった時に送るイベント</summary>
    private void SendOnSelectWorldChangedEvent()
    {
        if(OnSelectWorldChanged != null)
        {
            OnSelectWorldChanged(selectWorld);
        }
    }

    /// <summary>ステージが切り替わった時に送るイベント</summary>
    private void SendOnSelectStageChangedEvent()
    {
        if(OnSelectStageChanged != null)
        {
            OnSelectStageChanged(selectStage);
        }
    }
}
