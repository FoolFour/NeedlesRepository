using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemySearch : MonoBehaviour, IRespawnMessage
{
    public BigFloatEnemy m_bigEnemy;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_bigEnemy.GetComponent<MoveBlock>().SwitchOn();
            GetComponent<RemoveComponent>().SwitchActive(false);
        }
    }

    public void RespawnInit()
    {
        GetComponent<RemoveComponent>().SwitchActive(true);
    }
}
