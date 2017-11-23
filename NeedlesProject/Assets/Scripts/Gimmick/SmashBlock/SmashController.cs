using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashController : MonoBehaviour,IRespawnMessage {

    public float m_Speed = 1;

    float m_Distance = 0;

    Transform m_Smash1;
    Transform m_Smash2;

    float m_Timer = 0;
    float p1 = 1;

    public void RespawnInit()
    {
        m_Smash1.localScale = new Vector3(1, m_Smash1.localScale.y, m_Smash1.localScale.z);
        m_Smash2.localScale = new Vector3(1, m_Smash2.localScale.y, m_Smash1.localScale.z);
        p1 = 1;
        m_Distance = Vector3.Distance(m_Smash1.position, m_Smash2.position) / 2;
        m_Timer = 0;
    }

    // Use this for initialization
    void Start ()
    {
        m_Smash1 = transform.GetChild(0);
        m_Smash2 = transform.GetChild(1);
        m_Distance = Vector3.Distance(m_Smash1.position, m_Smash2.position) / 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_Timer += Time.deltaTime * m_Speed;
        float t = Mathf.Lerp(p1, m_Distance, m_Timer);
        m_Smash1.localScale = new Vector3(t, m_Smash1.localScale.y, m_Smash1.localScale.z);
        m_Smash2.localScale = new Vector3(t, m_Smash2.localScale.y, m_Smash1.localScale.z);
        if (m_Timer >= 1)
        {
            var temp = p1;
            p1 = m_Distance;
            m_Distance = temp;
            m_Timer = 0;
        }

	}
}
