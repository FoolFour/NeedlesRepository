using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBlock : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManagers.Instance.SpawnManager.ReSpawn();
        }
    }
}
