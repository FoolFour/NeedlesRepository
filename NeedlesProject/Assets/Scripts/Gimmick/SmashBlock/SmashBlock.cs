using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBlock : BlockBase
{

    bool isHit = false;

    [Tooltip("相手側のブロック")]
    public SmashBlock m_Partner;

    private BoxCollider m_bc;

    public void Start()
    {
        m_bc = GetComponent<BoxCollider>();
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        stickpoint.transform.position = m_bc.ClosestPointOnBounds(stickpoint.transform.position);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHit = true;
            if (m_Partner.isHit)
            {
                Debug.Log("挟まれた");
                GameManagers.Instance.SpawnManager.ReSpawn();
                isHit = false;
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (isHit && collision.gameObject.tag == "Player")
        {
            isHit = false;
        }
    }
}
