using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoin : BlockBase,IRespawnMessage
{
    [SerializeField, Tooltip("プレイヤー本体と当たる")]
    public bool CheckPlayer;
    [SerializeField, Tooltip("アームと当たる")]
    public bool CheckArm;

    private RemoveComponent m_RCom;
    private bool isDead = false;

    public void Start()
    {
        m_RCom = GetComponent<RemoveComponent>();
        isDead = false;
    }

    public override void StickEnter(GameObject arm)
    {
        if (CheckArm)
        {
            m_RCom.SwitchActive(false);
            isDead = true;
        }
        base.StickEnter(arm);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CheckPlayer)
        {
            if (other.CompareTag("Player"))
            {
                isDead = true;
                m_RCom.SwitchActive(false);
            }
        }
        if (CheckArm)
        {
            if (other.CompareTag("PlayerArm"))
            {
                isDead = true;
                m_RCom.SwitchActive(false);
            }
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void RespawnInit()
    {
        m_RCom.SwitchActive(true);
        isDead = false;
    }
}
