using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportBlock : BlockBase, IRespawnMessage
{
    [Tooltip("最初の地点")]
    public Transform m_from;
    [Tooltip("向かう場所")]
    public Transform m_to;
    [Tooltip("スピード")]
    public float m_speed = 0.3f;

    float m_timer = 0;
    bool isStickHit = false;



    public void RespawnInit()
    {
        m_timer = 0;
        isStickHit = false;
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        isStickHit = true;
    }

    public override void StickExit()
    {
        isStickHit = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!m_from) Debug.LogError("設定ミス");
        if (!m_to) Debug.LogError("設定ミス");
        m_timer = 0;
        transform.position = m_from.position;
        isStickHit = false;

        if (GetComponentInChildren<LineSetting>())
        {
            var line = GetComponentInChildren<LineSetting>();
            line.SetVertex(2);
            line.AddPoint(m_from.position);
            line.AddPoint(m_to.position);
            line.Loop(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(m_from.position, m_to.position, m_timer);
        if (isStickHit) { m_timer += Time.deltaTime * m_speed; }
        else { m_timer -= Time.deltaTime * m_speed; }
        m_timer = Mathf.Clamp(m_timer, 0, 1);
    }
}
