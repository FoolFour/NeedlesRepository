using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFloatEnemy : MonoBehaviour
{
    private SpawnManager m_SpawnManager;

    // Use this for initialization
    void Start()
    {
        m_SpawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerArm"))
        {
            var power = other.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            Debug.Log(other.gameObject.name);
            other.transform.parent.GetComponent<NeedleArm>().PlayerStan(power);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            m_SpawnManager.ReSpawn();
        }
    }

}
