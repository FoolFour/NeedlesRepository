using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 途中で消えるタイプのブロック
/// </summary>
public class ColliderInvalidBlock : BlockBase {

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        if (!GetComponent<BoxCollider>().enabled) arm.GetComponent<NeedleArm>().Return_Arm();
    }

}
