using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float m_Power = 10;

    Animator m_Animator;

    public void Start()
    {
        m_Animator = GetComponentInParent<Animator>();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Animator.SetTrigger("JumpTr");
            other.GetComponent<Rigidbody>().AddForce(transform.up * m_Power, ForceMode.VelocityChange);

        }
    }
}
