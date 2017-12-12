using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleBlock : BlockBase,IRespawnMessage
{
    public GameObject m_breakParticlePrefab;

    public void RespawnInit()
    {
        GetComponent<RemoveComponent>().SwitchActive(true);
    }

    public override void StickEnter(GameObject arm)
    {
        Sound.PlaySe("BlockBreak");
        var go = Instantiate(m_breakParticlePrefab, transform.position, Quaternion.identity);
        Destroy(go, 1f);
        GetComponent<RemoveComponent>().SwitchActive(false);
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
