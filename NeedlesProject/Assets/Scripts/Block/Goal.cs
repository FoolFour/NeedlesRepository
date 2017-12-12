using System.Collections;
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

    public override void StickHit(GameObject arm, GameObject stickpoint)
    {
        player.Goal();
        sceneChanger.SceneChange("Result", LoadSceneMode.Additive);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
