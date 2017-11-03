using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwnPoint : MonoBehaviour
{

    private SpawnManager m_Spawn;

    public void Start()
    {
        m_Spawn = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Spawn.CurrentSpawnChange(transform.position);
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
