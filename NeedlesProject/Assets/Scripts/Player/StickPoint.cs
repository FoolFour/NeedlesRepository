using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPoint : MonoBehaviour
{

    public Vector3 m_Centrifugalforce = Vector3.zero;

    private Rigidbody m_Arm;
    private Rigidbody m_Player;

    public void Start()
    {

    }

    void OnJointBreak(float breakForce)
    {
        ////m_Arm = GetComponent<HingeJoint>().connectedBody;
        //m_Player = GetComponent<HingeJoint>().connectedBody.transform.parent.GetComponent<Rigidbody>();

        //m_Player.isKinematic = false;
        //StartCoroutine(DelayMethod(() => 
        //{
        //    Debug.Log(m_Centrifugalforce.magnitude);
        //    Debug.DrawRay(transform.position, m_Centrifugalforce);
        //    m_Player.AddForce(m_Centrifugalforce, ForceMode.VelocityChange);
        //    m_Centrifugalforce = Vector3.zero;
        //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //}));
    }

    private IEnumerator DelayMethod(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(m_Centrifugalforce);
        action();
    }
}
