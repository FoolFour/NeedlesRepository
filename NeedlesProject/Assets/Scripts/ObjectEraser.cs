using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectEraser : MonoBehaviour
{
    [SerializeField]
    GameObject eraseObj;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Destroy(eraseObj);
        }
    }
}
