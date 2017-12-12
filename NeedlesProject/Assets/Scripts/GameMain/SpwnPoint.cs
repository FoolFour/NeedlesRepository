using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwnPoint : MonoBehaviour
{

    private SpawnManager m_Spawn;
    private Animator[] m_animator;

    public void Start()
    {
        m_Spawn = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        m_animator = GetComponentsInChildren<Animator>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            Sound.PlaySe("CheckPoint");
            m_animator[0].SetTrigger("Trigger");
            m_animator[1].SetTrigger("Trigger");
            m_Spawn.CurrentSpawnChange(transform.position);
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
