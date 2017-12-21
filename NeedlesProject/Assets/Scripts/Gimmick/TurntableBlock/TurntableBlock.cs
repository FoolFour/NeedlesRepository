using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableBlock : BlockBase, IRespawnMessage
{

    [Tooltip("回転スピード")]
    public float m_rotationSpeed;

    private Quaternion m_firstRotate;

    public void Start()
    {
        m_firstRotate = transform.rotation;
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        transform.Rotate(Vector3.forward * m_rotationSpeed);
    }

    public void RespawnInit()
    {
        transform.rotation = m_firstRotate;
    }
}
