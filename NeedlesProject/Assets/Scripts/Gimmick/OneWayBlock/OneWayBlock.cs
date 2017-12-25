using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayBlock : MonoBehaviour
{
    GameObject m_parent = null;
    public Animator m_animator;

    // Use this for initialization
    void Start()
    {
        m_parent = transform.parent.gameObject;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_parent.GetComponent<BoxCollider>().enabled = false;
            m_animator.SetBool("Open", true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_parent.GetComponent<BoxCollider>().enabled = true;
            m_animator.SetBool("Open",false);
        }
    }
}
