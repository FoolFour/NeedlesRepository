using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float m_Power = 10;
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * m_Power, ForceMode.VelocityChange);
        }
    }
}
