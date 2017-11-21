using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{

    private SpawnManager m_SpawnManager;

    // Use this for initialization
    void Start()
    {
        m_SpawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            m_SpawnManager.ReSpawn();
        }
    }
}
