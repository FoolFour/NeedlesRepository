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

    public void LateUpdate()
    {
        //スケールと回転（ｘとｙ）を動かないようにする処理
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        var temp = transform.localEulerAngles;
        temp.x = 0;
        temp.y = 0;
        transform.localEulerAngles = temp;

    }

    void OnJointBreak(float breakForce)
    {
        m_Action();
        m_Action = () => { };
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
