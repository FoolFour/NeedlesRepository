using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripBlock : BlockBase
{

    public override void StickEnter(GameObject arm)
    {
        Sound.PlaySe("Grip");
        base.StickEnter(arm);
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        stickpoint.gameObject.transform.localPosition = Vector3.zero;
        transform.GetChild(0).up = arm.transform.up;
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
