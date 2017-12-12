using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryFishEnemy : MonoBehaviour,IRespawnMessage {

    public enum State
    {
        Normal,
        Shock,
        Dead,
    }
    public State m_state;
    public float m_NormalTime = 3;
    public float m_ShockTime = 3;
    public Color m_ShockColor = Color.red;


    public GameObject m_Thunder;
    public Renderer[] m_Renderer;

    private Color m_NormalColor;
    private float m_Timer = 0;

	// Use this for initialization
	void Start ()
    {
        m_state = State.Normal;
        m_Timer = 0;
        m_Thunder.SetActive(false);
        m_NormalColor = m_Renderer[0].material.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_Timer += Time.deltaTime;
        switch(m_state)
        {
            case State.Normal:Normal(); break;
            case State.Shock:Shock(); break;
            case State.Dead: Dead();break;
        }
	}

    void Normal()
    {
        if(m_Timer >= m_NormalTime)
        {
            m_Timer = 0;
            m_state = State.Shock;

            m_Thunder.SetActive(true);
            foreach (var renderer in m_Renderer)
            {
                renderer.material.color = m_ShockColor;
            }
        }
    }

    void Shock()
    {
        if (m_Timer >= m_ShockTime)
        {
            m_Timer = 0;
            m_state = State.Normal;

            m_Thunder.SetActive(false);
            foreach (var renderer in m_Renderer)
            {
                renderer.material.color = m_NormalColor;
            }
        }
    }

    void Dead()
    {
        m_Thunder.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var power = collision.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            collision.gameObject.GetComponent<Player>().StanMode(power);
        }
        if (collision.gameObject.CompareTag("PlayerArm"))
        {
            var power = collision.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            collision.gameObject.GetComponent<NeedleArm>().PlayerStan(power);
        }
    }

    public void RespawnInit()
    {
        m_state = State.Normal;
        m_Timer = 0;
        foreach (var renderer in m_Renderer)
        {
            renderer.material.color = m_NormalColor;
        }
    }
}
