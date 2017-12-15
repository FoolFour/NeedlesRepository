﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

public class Goal : BlockBase
{
    [SerializeField]
    public SceneChanger sceneChanger;

    [SerializeField]
    public Player       player;

    private void Reset()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
        player       = FindObjectOfType<Player>();
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        stickpoint.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        player.Goal();
        sceneChanger.SceneChange("Result", LoadSceneMode.Additive);
        GetComponent<BoxCollider>().isTrigger = true;
        GameManagers.Instance.GameStateManager.StateChange(GameState.End);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
