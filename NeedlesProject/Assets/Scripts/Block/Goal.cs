using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

public class Goal : BlockBase
{
    [SerializeField]
    public Player       player;

    public SceneChanger sceneChanger;

    private void Reset()
    {
        player       = FindObjectOfType<Player>();
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        stickpoint.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        player.Goal();
        //GetComponent<TEST_GoalMove>().StartEvent(); //デバッグ用
        gameObject.GetComponent<GoalAnimation>().StartAnimation();

        GetComponent<BoxCollider>().isTrigger = true;
        GameManagers.Instance.GameStateManager.StateChange(GameState.End);
    }

    public override void StickExit()
    {
        base.StickExit();
    }

    public void SceneChange()
    {
        sceneChanger.SceneChangeAsync("Result", LoadSceneMode.Additive);
    }
}
