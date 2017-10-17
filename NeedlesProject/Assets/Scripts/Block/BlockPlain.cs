using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlain : BlockBase {


    public override void StickEnter()
    {
        Debug.Log("plainに当たった");
        base.StickEnter();
    }

    public override void StickExit()
    {
        Debug.Log("plainから外れた");
        base.StickExit();
    }
}
