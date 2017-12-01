using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlock : BlockBase {

    [SerializeField, TooltipAttribute("壁をはじいた時の力")]
    public float m_impactPower = 5;

    public override void StickEnter(GameObject arm)
    {
        var force = -arm.transform.up * m_impactPower;
        arm.GetComponent<NeedleArm>().PlayerAddForce(force);
        Sound.PlaySe("NonPickBlock");
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
