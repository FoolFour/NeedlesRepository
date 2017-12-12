using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour, IRespawnMessage {

    [Tooltip("回転スピード")]
    public float m_rotationSpeed;

    private Quaternion m_firstRotate;

    public void RespawnInit()
    {
        transform.rotation = m_firstRotate;
    }

    // Use this for initialization
    void Start ()
    {
        m_firstRotate = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.forward * m_rotationSpeed);	
	}
}
