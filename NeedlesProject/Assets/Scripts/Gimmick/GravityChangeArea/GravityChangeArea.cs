using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeArea : MonoBehaviour, IRespawnMessage
{
    public Vector3 m_nextGravity = new Vector3(0, 9.81f, 0);
    private Vector3 m_prevGravity;
    private bool isInside; //プレイヤーが中にいることを証明する

    public void Start()
    {
        m_prevGravity = Physics.gravity;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            m_prevGravity = Physics.gravity;
            Physics.gravity = m_nextGravity;
        }
    }

    public void Update()
    {
        if (!isInside) return;
        var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player")) return;
        }
        Physics.gravity = m_prevGravity;
        isInside = false;
    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Physics.gravity = m_prevGravity;
    //    }
   // }

    public void RespawnInit()
    {
        Physics.gravity = m_prevGravity;
    }
}
