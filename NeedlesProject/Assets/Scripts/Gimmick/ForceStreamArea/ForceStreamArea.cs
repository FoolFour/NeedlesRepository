using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceStreamArea : MonoBehaviour
{
    [SerializeField,Tooltip("気流の力")]
    public float m_Power　= 10;
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * m_Power, ForceMode.Acceleration);
        }
    }
}
