using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlain : BlockBase {


    public override void StickEnter(GameObject arm)
    {
        Debug.Log("plainに当たった");
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        Debug.Log("plainから外れた");
        base.StickExit();
    }
}
