using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stan : MonoBehaviour {

    public GameObject m_stanEffect;

    GameObject m_effectObj;
    Player m_playerscript;
    bool m_prev = false;

	// Use this for initialization
	void Start ()
    {
        m_playerscript = GetComponent<Player>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_playerscript.isStan() != m_prev)
        {
            if (m_playerscript.isStan())
            {
                m_effectObj = (GameObject)Instantiate(m_stanEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
            }
            else
            {
                if (m_effectObj)
                {
                    Destroy(m_effectObj);
                    m_effectObj = null;
                }
            }
            m_prev = m_playerscript.isStan();
        }
    }
}
