using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float m_MinValue;
    private Transform m_Player;
    public float speed = 1;
    private Vector2 m_FirstPosition;

    private Vector2 m_CurrentPosition;

    private Transform m_leftImage;
    private Transform m_rightImage;

    private float m_plusPosition = 300;
    private float m_minusPosition = -300;

    // Use this for initialization
    void Start ()
    {
        m_rightImage = transform.GetChild(0);
        m_rightImage.localPosition = new Vector3(600, 0, 0);
        m_leftImage = transform.GetChild(1);
        m_leftImage.localPosition = new Vector3(-600, 0, 0);
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

        m_CurrentPosition.x = (m_FirstPosition.x - m_Player.position.x);

        GetComponent<RectTransform>().localPosition = m_CurrentPosition;

    }
}
