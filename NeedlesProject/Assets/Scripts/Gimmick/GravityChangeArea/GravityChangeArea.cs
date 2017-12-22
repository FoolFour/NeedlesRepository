using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeArea : MonoBehaviour
{
    public Vector3 m_nextGravity = new Vector3(0, 9.81f, 0);
    private Vector3 m_prevGravity;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_prevGravity = Physics.gravity;
            Physics.gravity = m_nextGravity;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Physics.gravity = m_prevGravity;
        }
    }
}
