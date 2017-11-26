using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBlock : MonoBehaviour,IRespawnMessage{

    [Tooltip("処理を遅らせる時間")]
    public float m_delayTime = 0;
    [Tooltip("実体化する時間")]
    public float m_existTime = 5;
    [Tooltip("透明化している時間")]
    public float m_notExistTime = 3;

    private float m_timer = 0;
    private bool isdelay = true;
    private bool isActive = true;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;
        if(!isdelay)
        {
            float Time = 0;
            if (isActive) Time = m_existTime;
            else Time = m_notExistTime;

            if (m_timer >= Time)
            {
                isActive = !isActive;
                FlipColliderandRenderer();
                m_timer = 0;
            }
        }
        if (m_timer >= m_delayTime && isdelay)
        {
            m_timer = 0;
            isdelay = false;
        }
	}

    /// <summary>
    /// 反対にする
    /// </summary>
    /// <param name="flag"></param>
    void FlipColliderandRenderer()
    {
        var boxc = GetComponent<BoxCollider>();
        boxc.enabled = !boxc.enabled;
        var renderer = GetComponent<MeshRenderer>();
        renderer.enabled = !renderer.enabled;
    }

    public void RespawnInit()
    {
        m_timer = 0;
        var boxc = GetComponent<BoxCollider>();
        boxc.enabled = true;
        var renderer = GetComponent<MeshRenderer>();
        renderer.enabled = true;
        isdelay = true;
        isActive = true;
    }
}
