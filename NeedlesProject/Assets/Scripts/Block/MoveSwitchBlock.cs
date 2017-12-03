using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSwitchBlock : BlockBase,IRespawnMessage
{

    [SerializeField, Tooltip("動かすscript")]
    public MoveBlock m_moveBlock;

    MeshRenderer m_meshRenderer;

    public void RespawnInit()
    {
        GetComponent<RemoveComponent>().SwitchActive(true);
        m_meshRenderer.material.color = Color.red;
    }

    public void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.color = Color.red;
    }

    public override void StickEnter(GameObject arm)
    {
        m_meshRenderer.material.color = Color.green;
        m_moveBlock.SwitchOn();
        base.StickEnter(arm);
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        GetComponent<RemoveComponent>().SwitchActive(false);
        if (!GetComponent<BoxCollider>().enabled) arm.GetComponent<NeedleArm>().Return_Arm();
    }
}
