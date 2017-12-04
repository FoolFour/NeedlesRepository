using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float m_Power = 30;
    public void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            var dir = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * m_Power, ForceMode.VelocityChange);
        }
    }
}
