using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour {

    NeedleArm m_Arm;
    Vector3 m_FirstPosition;
    public float m_BreakValue = 1;

	// Use this for initialization
	void Start ()
    {
        m_Arm = transform.GetComponent<NeedleArm>();
        m_FirstPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Vector3.Distance(m_FirstPosition, transform.localPosition) > m_BreakValue)
        {
            Debug.Log("腕が外れました");
            m_Arm.PlayerStan(Vector3.zero);
            m_Arm.transform.localPosition = m_FirstPosition;
        }
	}
}
