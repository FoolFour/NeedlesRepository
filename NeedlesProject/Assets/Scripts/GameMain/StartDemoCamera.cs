﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StartDemoCamera : MonoBehaviour {

    [SerializeField, Tooltip("軌道")]
    public Transform[] m_MovePoint;
    public float m_MoveSpeed;

    private float m_Timer = 0.0f;
    private int p1 = 0;
    private int p2 = 1;

    [SerializeField, Tooltip("デバッグモード")]
    public bool isDebug = true;
    [SerializeField, Tooltip("どれくらい近づくか？")]
    public float m_playerDistance;
    [SerializeField, Tooltip("何秒で近付くか")]
    public float m_playerPanSpeed;
    private Transform Player;

    private bool CoroutineNow = false;

    void Start ()
    {
        if (isDebug)
        {
            Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
            Player.GetComponent<Animator>().SetTrigger("Play");
            Player.GetComponent<Player>().isAnimation = false;

            GetComponent<GameCamera.Camera>().enabled = true;
            Destroy(this);
            return;
        }

        Assert.IsFalse(m_MovePoint.Length <= 1, "少な過ぎる!");
        transform.position = m_MovePoint[0].position;
        GetComponent<GameCamera.Camera>().enabled = false;
    }
	
	void Update ()
    {
        //プレイヤを探す
        if (!Player)
        {
            Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
            return;
        }
        if (CoroutineNow) return;
        m_Timer += m_MoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(m_MovePoint[p1].position, m_MovePoint[p2].position, m_Timer);
        if (m_Timer >= 1)
        {
            if (p2 == m_MovePoint.Length - 1)
            {
                StartCoroutine(CameraPan());
                CoroutineNow = true;
                return; //end
            }
            m_Timer = 0;
            p1 = (p1 + 1) % m_MovePoint.Length;
            p2 = (p2 + 1) % m_MovePoint.Length;
        }
    }

    IEnumerator CameraPan()
    {
        var distance = Vector3.Distance(Player.transform.position, transform.position);
        while (distance > m_playerDistance)
        {
            transform.position += transform.forward * m_playerPanSpeed;
            yield return new WaitForEndOfFrame();
            distance = Vector3.Distance(Player.transform.position, transform.position);
        }

        Player.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(1.5f);
        Player.GetComponent<Player>().isAnimation = false;

        GetComponent<GameCamera.Camera>().enabled = true;
        Destroy(this);
    }
}
