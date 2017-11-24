using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveBlock : MonoBehaviour {

    [SerializeField,Tooltip("軌道")]
    public Transform[] m_MovePoint;
    public float m_MoveSpeed;

    private float m_Timer = 0.0f;
    private int p1 = 0;
    private int p2 = 1;

	// Use this for initialization
	void Start ()
    {
        Assert.IsFalse(m_MovePoint.Length <= 1, "少な過ぎる!");
        transform.position = m_MovePoint[0].position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_Timer += m_MoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(m_MovePoint[p1].position, m_MovePoint[p2].position,m_Timer);
        if(m_Timer >= 1)
        {
            m_Timer = 0;
            p1 = (p1 + 1) % m_MovePoint.Length;
            p2 = (p2 + 1) % m_MovePoint.Length;
        }
	}
}