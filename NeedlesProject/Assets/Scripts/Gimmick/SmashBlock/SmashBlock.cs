using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBlock : MonoBehaviour
{

    public bool isHit = false;

    public SmashBlock m_Partner;

    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
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
        if(isHit && collision.gameObject.tag == "Player")
        {
            isHit = false;
        }
    }
}
