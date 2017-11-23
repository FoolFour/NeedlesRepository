using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayBlock : MonoBehaviour
{
    GameObject m_parent = null;

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
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_parent.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
