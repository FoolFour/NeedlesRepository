using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float m_MinValue;
    private Transform m_Player;
    public float speed = 1;
    public float m_xScrollSpeed = 10;
    private Vector2 m_FirstPlayerPosition;
    private Vector2 m_CurrentPosition;

    private float m_playerPrevx = 0;


    // Use this for initialization
    void Start ()
    {

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

        {
            var temp = GetComponent<RectTransform>().localPosition;
            temp.x += move * m_xScrollSpeed;
            temp.y = m_CurrentPosition.y;
            GetComponent<RectTransform>().localPosition = temp;
        }

        float halfWidth = 1280 / 2;
        if(GetComponent<RectTransform>().localPosition.x >= 1280 + halfWidth)
        {
            var temp = GetComponent<RectTransform>().localPosition;
            temp.x = SearchRightEndmost().localPosition.x - 1280;
            GetComponent<RectTransform>().localPosition = temp;
        }
        else if(GetComponent<RectTransform>().localPosition.x <= -1280 - halfWidth)
        {
            var temp = GetComponent<RectTransform>().localPosition;
            temp.x = SearchLeftEndmost().localPosition.x + 1280;
            GetComponent<RectTransform>().localPosition = temp;
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
        Debug.Log(endmost.name);
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
        Debug.Log(endmost.name);
        return endmost;
    }
}
