using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlain : BlockBase {


    public override void StickEnter(GameObject arm)
    {
        Sound.PlaySe("Pick");
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
