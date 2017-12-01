using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleBlock : BlockBase,IRespawnMessage
{
    public void RespawnInit()
    {
        GetComponent<RemoveComponent>().SwitchActive(true);
    }

    public override void StickEnter(GameObject arm)
    {
        Sound.PlaySe("BlockBreak");
        GetComponent<RemoveComponent>().SwitchActive(false);
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
