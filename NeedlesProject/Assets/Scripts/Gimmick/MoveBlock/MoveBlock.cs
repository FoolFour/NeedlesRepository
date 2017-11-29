using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveBlock : MonoBehaviour,IRespawnMessage {

    [SerializeField,Tooltip("軌道")]
    public Transform[] m_MovePoint;
    public float m_MoveSpeed;
    [SerializeField, Tooltip("スイッチ使って起動するタイプか？")]
    public bool isSwitchType = false;
    public bool isLoop = true;

    private float m_Timer = 0.0f;
    private int p1 = 0;
    private int p2 = 1;
    private bool isSwitchMode;

    // Use this for initialization
    void Start ()
    {
        Assert.IsFalse(m_MovePoint.Length <= 1, "少な過ぎる!");
        transform.position = m_MovePoint[0].position;
        isSwitchMode = isSwitchType;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isSwitchMode) return;
        m_Timer += m_MoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(m_MovePoint[p1].position, m_MovePoint[p2].position,m_Timer);
        if(m_Timer >= 1)
        {
            if (!isLoop && p2 == m_MovePoint.Length - 1) return;
            m_Timer = 0;
            p1 = (p1 + 1) % m_MovePoint.Length;
            p2 = (p2 + 1) % m_MovePoint.Length;
        }
	}

    public void SwitchOn()
    {
        isSwitchMode = false;
    }

    public void RespawnInit()
    {
        transform.position = m_MovePoint[0].position;
        isSwitchMode = isSwitchType;
        m_Timer = 0;
    }
}