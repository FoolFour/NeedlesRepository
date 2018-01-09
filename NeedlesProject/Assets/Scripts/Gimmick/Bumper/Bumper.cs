using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float m_Power = 30;

    Animator m_Animator;

    public void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            m_Animator.SetTrigger("BumperOnTr");
            Sound.PlaySe("Spring");
            var dir = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * m_Power, ForceMode.VelocityChange);
        }
    }
}
