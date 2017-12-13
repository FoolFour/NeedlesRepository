using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeBlock : BlockBase {

    [Tooltip("変化する重力")]
    public Vector2 m_Gravity;

    public override void StickEnter(GameObject arm)
    {
        Physics.gravity = m_Gravity;
    }
}
