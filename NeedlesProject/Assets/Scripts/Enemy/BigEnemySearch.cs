using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemySearch : MonoBehaviour
{
    public BigFloatEnemy m_bigEnemy;

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_bigEnemy.GetComponent<MoveBlock>().SwitchOn();
            Destroy(gameObject);
        }
    }
}
