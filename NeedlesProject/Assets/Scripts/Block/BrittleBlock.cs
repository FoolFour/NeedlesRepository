using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleBlock : BlockBase,IRespawnMessage
{
    public void RespawnInit()
    {
#warning　ブロックが軽量化されたら行う
    }

    public override void StickEnter(GameObject arm)
    {
        Destroy(gameObject);
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }

}
