using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwnPoint : MonoBehaviour
{

    private SpawnManager m_Spawn;
    private Animator m_animator;

    public void Start()
    {
        m_Spawn = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        m_animator = GetComponentInChildren<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            m_animator.SetTrigger("Trigger");
            m_Spawn.CurrentSpawnChange(transform.position);
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
