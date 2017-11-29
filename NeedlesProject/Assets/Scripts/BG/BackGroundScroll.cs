using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float m_MinValue;
    private Transform m_Player;
    public float speed = 1;
    private Vector2 m_FirstPosition;

    private Vector2 m_CurrentPosition;

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
            if(m_Player) m_FirstPosition = m_Player.position;
            return;
        }

        m_CurrentPosition.y = (m_FirstPosition.y - m_Player.position.y) * speed;
        m_CurrentPosition.y = Mathf.Clamp(m_CurrentPosition.y, m_MinValue, 0);

        m_CurrentPosition.x = (m_FirstPosition.x - m_Player.position.x) * speed;

        GetComponent<RectTransform>().localPosition = m_CurrentPosition;

    }
}
