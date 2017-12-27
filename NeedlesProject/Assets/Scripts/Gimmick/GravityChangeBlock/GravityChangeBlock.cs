using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeBlock : BlockBase, IRespawnMessage
{

    [Tooltip("変化する重力")]
    public Vector2 m_Gravity;

    private Vector3 m_NormalGravity;

    public void Start()
    {
        m_NormalGravity = Physics.gravity;
    }

    public void RespawnInit()
    {
        Physics.gravity = m_Gravity;
    }

    public override void StickEnter(GameObject arm)
    {
        m_NormalGravity = Physics.gravity;
        Physics.gravity = m_Gravity;
    }
}
