using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPoint : MonoBehaviour
{

    public Vector3 m_Centrifugalforce = Vector3.zero;

    private Rigidbody m_Arm;
    private Rigidbody m_Player;

    private System.Action m_Action = () => { };

    public void Start()
    {

    }

    void OnJointBreak(float breakForce)
    {
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        m_Action();
        m_Action = () => { };
    }

    public void BreakAction(System.Action action)
    {
        m_Action = action;
    }

    private IEnumerator DelayMethod(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(m_Centrifugalforce);
        action();
    }
}
