using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveBlock : MonoBehaviour,IRespawnMessage {

    public enum MoveMode
    {
        Lerp,
        Speed,
    }

    [SerializeField,Tooltip("軌道")]
    public Transform[] m_MovePoint;
    [SerializeField, Tooltip("基本スピード")]
    public float m_MoveSpeed;
    [SerializeField, Tooltip("途中でスピードを変える場合入力")]
    public float[] m_MoveSpeeds;
    [SerializeField, Tooltip("スイッチ使って起動するタイプか？")]
    public bool isSwitchType = false;
    [SerializeField, Tooltip("移動タイプ")]
    public MoveMode m_moveMode = MoveMode.Lerp;
    public bool isLoop = true;

    private float m_Timer = 0.0f;
    private int p1 = 0;
    private int p2 = 1;
    private int speedIndex = 0;
    private bool isSwitchMode;

    // Use this for initialization
    void Start ()
    {
        Assert.IsFalse(m_MovePoint.Length <= 1, "少な過ぎる!");
        transform.position = m_MovePoint[0].position;
        isSwitchMode = isSwitchType;
        if (m_MoveSpeeds.Length > 0) m_MoveSpeed = m_MoveSpeeds[speedIndex];

        var line = GetComponentInChildren<LineSetting>();
        line.SetVertex(m_MovePoint.Length);
        foreach(var point in m_MovePoint)
        {
            line.AddPoint(point.position);
        }
        line.Loop(isLoop);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isSwitchMode) return;
        switch(m_moveMode)
        {
            case MoveMode.Lerp: MoveModeLerp();break;
            case MoveMode.Speed:MoveModeSpeed(); break;
        }
	}

    private void MoveModeLerp()
    {
        m_Timer += m_MoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(m_MovePoint[p1].position, m_MovePoint[p2].position, m_Timer);
        if (m_Timer >= 1)
        {
            if (!isLoop && p2 == m_MovePoint.Length - 1) return;
            m_Timer = 0;
            p1 = (p1 + 1) % m_MovePoint.Length;
            p2 = (p2 + 1) % m_MovePoint.Length;
            if (m_MoveSpeeds.Length > 0)
            {
                speedIndex = (speedIndex + 1) % m_MoveSpeeds.Length;
                m_MoveSpeed = m_MoveSpeeds[speedIndex];
            }
        }
    }

    private void MoveModeSpeed()
    {
        var velocity = (m_MovePoint[p2].position - transform.position).normalized * m_MoveSpeed;
        var distance = Vector3.Distance(m_MovePoint[p2].position, transform.position);
        if(velocity.magnitude >= distance)
        {
            if (!isLoop && p2 == m_MovePoint.Length - 1) return;
            transform.position = m_MovePoint[p2].position;
            p2 = (p2 + 1) % m_MovePoint.Length;
            if (m_MoveSpeeds.Length > 0)
            {
                speedIndex = (speedIndex + 1) % m_MoveSpeeds.Length;
                m_MoveSpeed = m_MoveSpeeds[speedIndex];
            }
            return;
        }
        transform.position += velocity;
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
        p1 = 0;
        p2 = 1;
        if (m_MoveSpeeds.Length > 0) m_MoveSpeed = m_MoveSpeeds[0];
    }
}