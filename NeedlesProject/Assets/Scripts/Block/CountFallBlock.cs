using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountFallBlock : BlockBase, IRespawnMessage
{
    public float m_fallTime = 3;

    Vector3 m_firstPosition;
   Coroutine m_routine;

    public void Start()
    {
        m_firstPosition = transform.position;
    }

    public override void StickEnter(GameObject arm)
    {
        m_routine = StartCoroutine(DelayMethod(m_fallTime, () =>
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }));

        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }

    public override void StickStay(GameObject arm)
    {
        if (!GetComponent<BoxCollider>().enabled) arm.GetComponent<NeedleArm>().Return_Arm();
        base.StickStay(arm);
    }

    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player")) return;
        GetComponent<RemoveComponent>().SwitchActive(false);

    }

    public void RespawnInit()
    {
        transform.position = m_firstPosition;
        GetComponent<Rigidbody>().isKinematic = true;
        if(m_routine != null) StopCoroutine(m_routine);
        GetComponent<RemoveComponent>().SwitchActive(true);
    }
}
