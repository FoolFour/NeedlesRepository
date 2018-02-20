using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour,IRespawnMessage {

    public  float         m_MinValue;
    private Transform     m_Player;
    public  float         speed = 1;
    public  float         m_xScrollSpeed = 10;
    private Vector2       m_FirstPlayerPosition;
    private Vector2       m_CurrentPosition;
    private float         m_playerPrevx = 0;

    private RectTransform m_rectTransform;

    static readonly int Width = 1280;

    // Use this for initialization
    void Start ()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Player)
        {
            m_Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
            if (m_Player)
            {
                m_FirstPlayerPosition = m_Player.position;
                m_playerPrevx = m_Player.position.x;
            }
            return;
        }

        m_CurrentPosition.y = (m_FirstPlayerPosition.y - m_Player.position.y) * speed;
        m_CurrentPosition.y = Mathf.Clamp(m_CurrentPosition.y, m_MinValue, 0);

        float move = m_playerPrevx - m_Player.position.x;
        m_playerPrevx = m_Player.position.x;

        //ゴール時に動かないようにする
        var state = GameManagers.Instance.GameStateManager.m_gameState;
        if(state == GameState.End)
        {
            var temp  = m_rectTransform.localPosition;
            temp.y    = m_CurrentPosition.y;
            m_rectTransform.localPosition = temp;
        }
        else
        {
            var temp  = m_rectTransform.localPosition;
            temp.x   += move * m_xScrollSpeed;
            temp.y    = m_CurrentPosition.y;
            m_rectTransform.localPosition = temp;
        }

        float halfWidth = Width / 2;
        if(m_rectTransform.localPosition.x >= Width + halfWidth)
        {
            var temp =m_rectTransform.localPosition;
            temp.x = SearchRightEndmost().localPosition.x - Width;
            m_rectTransform.localPosition = temp;
        }
        else if(m_rectTransform.localPosition.x <= -Width - halfWidth)
        {
            var temp = m_rectTransform.localPosition;
            temp.x = SearchLeftEndmost().localPosition.x + Width;
            m_rectTransform.localPosition = temp;
        }

    }

    /// <summary>
    /// 兄弟で一番右端を見つける（プラス側）
    /// </summary>
    private RectTransform SearchRightEndmost()
    {
        float end = 0;
        RectTransform endmost = null;

        foreach(var rt in transform.parent.GetComponentsInChildren<RectTransform>())
        {
            if (end > rt.localPosition.x)
            {
                end = rt.localPosition.x;
                endmost = rt;
            }
        }
        //Debug.Log(endmost.name);
        return endmost;
    }

    /// <summary>
    /// 兄弟で一番左端を見つける（マイナス側）
    /// </summary>
    /// <returns></returns>
    private RectTransform SearchLeftEndmost()
    {
        float end = 0;
        RectTransform endmost = null;
        foreach (var rt in transform.parent.GetComponentsInChildren<RectTransform>())
        {
            if (end < rt.localPosition.x)
            {
                end = rt.localPosition.x;
                endmost = rt;
            }
        }
        //Debug.Log(endmost.name);
        return endmost;
    }

    public void RespawnInit()
    {
        m_Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
        m_playerPrevx = m_Player.position.x;
    }
}
