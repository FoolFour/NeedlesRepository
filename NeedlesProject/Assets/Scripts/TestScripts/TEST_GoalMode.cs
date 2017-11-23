using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_GoalMode : BlockBase,IRespawnMessage
{
    public override void StickEnter(GameObject arm)
    {
        arm.transform.parent.GetComponent<Player>().Goal();
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
    public void RespawnInit()
    {
        Debug.Log("レスポン処理");
    }
}
