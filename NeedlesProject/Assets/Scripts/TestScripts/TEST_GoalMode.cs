using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_GoalMode : BlockBase
{
    public override void StickEnter(GameObject arm)
    {
        arm.transform.parent.GetComponent<Player>().Stop();
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
