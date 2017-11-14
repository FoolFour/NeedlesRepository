using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float m_MinValue;
    public Transform m_Player;
    public float speed = 1;
    private float m_FirstYPosition = 0;

    private float m_CurrentY;
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
            if(m_Player) m_FirstYPosition = m_Player.position.y;
            return;
        }

        m_CurrentY = (m_FirstYPosition - m_Player.position.y) * speed;
        m_CurrentY = Mathf.Clamp(m_CurrentY, m_MinValue, 0);

        GetComponent<RectTransform>().localPosition = new Vector3(0, m_CurrentY, 0);

    }
}
