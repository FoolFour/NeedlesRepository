using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCameraControl : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField)　/
    ////////////////////////

    [SerializeField]
    WorldSelect    worldSelect;

    [SerializeField]
    CameraControl  cameraControl;

    [SerializeField]
    [Header("カメラが移動するスピード")]
    float          pointChangeSpeed = 1.0f;


    [SerializeField]
    [Header("ワールドセレクト時にアクティブにするオブジェクト")]
    private List<GameObject> worldSelectActive;
    
    [SerializeField]
    [Header("ステージセレクト時にアクティブにするオブジェクト")]
    private List<GameObject> stageSelectActive;

    enum State
    {
        WorldSelect,
        WorldToStage,
        StageSelect,
        StageToWorld,
        StageToStart
    }
    private State state;

    private void Reset()
    {
        worldSelect   = FindObjectOfType<WorldSelect>();
        cameraControl = FindObjectOfType<CameraControl>();
    }

    private void Start()
    {
        state = State.WorldSelect;
        cameraControl.OnChangeComplete += OnChangeComplete;
    }

    private void OnChangeComplete(int newControlPoint)
    {
        switch (state)
        {
            case State.WorldToStage:
                state = State.StageSelect;
                SetActiveStageObject(true);
                break;

            case State.StageToWorld:
                state = State.WorldSelect;
                SetActiveWorldObject(true);
                break;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WorldSelect: OnWorldSelect(); break;
            case State.StageSelect: OnStageSelect(); break;
        }
    }

    private void OnWorldSelect()
    {
        if(worldSelect.IsChangeAnimation) { return; }

        if(Input.GetButtonDown(GamePad.Submit))
        {
            ChangeStageSelect();
        }
    }

    private void OnStageSelect()
    {
        if(Input.GetButtonDown(GamePad.Submit))
        {
            state = State.StageToStart;
        }

        if(Input.GetButtonDown(GamePad.Cancel))
        {
            ChangeWorldSelect();
        }
    }

    private void ChangeStageSelect()
    {
        state = State.WorldToStage;
        int num = worldSelect.SelectWorld;
        cameraControl.ChangeControlPointLiner(num, pointChangeSpeed);

        SetActiveWorldObject(false);
    }

    private void ChangeWorldSelect()
    {
        state = State.StageToWorld;
        const int worldPoint = 0;
        cameraControl.ChangeControlPointLiner(worldPoint, pointChangeSpeed);
        
        SetActiveStageObject(false);
    }


    private void SetActiveWorldObject(bool active)
    {
        foreach (GameObject item in worldSelectActive)
        {
            item.SetActive(active);
        }
    }

    private void SetActiveStageObject(bool active)
    {
        foreach (GameObject item in stageSelectActive)
        {
            item.SetActive(active);
        }
    }
}
