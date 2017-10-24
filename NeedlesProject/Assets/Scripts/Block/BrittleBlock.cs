using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleBlock : BlockBase {

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
