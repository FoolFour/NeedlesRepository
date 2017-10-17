using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlock : BlockBase {

    public override void StickEnter()
    {
        Debug.Log("Hardに当たった");
        base.StickEnter();
    }

    public override void StickExit()
    {
        Debug.Log("Hardからはずれた");
        base.StickExit();
    }
}
