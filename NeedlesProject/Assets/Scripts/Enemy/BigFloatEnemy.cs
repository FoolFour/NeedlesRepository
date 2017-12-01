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

        if (other.gameObject.tag.Contains("Player"))
        {
            m_SpawnManager.ReSpawn();
        }
    }

}
