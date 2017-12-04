using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDownArea : MonoBehaviour
{
    public float m_decelerationPower = 0.1f;
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var rb = other.GetComponent<Rigidbody>();
            rb.velocity -= rb.velocity * m_decelerationPower;
        }
    }
}
