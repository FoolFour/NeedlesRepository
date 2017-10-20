using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlock : BlockBase {

    public override void StickEnter(GameObject arm)
    {
        arm.GetComponent<NeedleArm>().PlayerAddForce();
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        Debug.Assert(true);
        base.StickExit();
    }
}
