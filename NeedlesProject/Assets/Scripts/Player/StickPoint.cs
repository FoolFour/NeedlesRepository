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
        m_Arm = GetComponent<HingeJoint>().connectedBody;
        m_Player = GetComponent<HingeJoint>().connectedBody.transform.parent.GetComponent<Rigidbody>();

        m_Arm.velocity = Vector3.zero;
        m_Player.velocity = Vector3.zero;
        m_Player.isKinematic = false;
        StartCoroutine(DelayMethod(() => { m_Player.AddForce(m_Centrifugalforce, ForceMode.Impulse); }));
    }

    private IEnumerator DelayMethod(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(m_Centrifugalforce);
        action();
    }
}
