using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripBlock : BlockBase
{
    private GameObject m_Arm;

    public override void StickEnter(GameObject arm)
    {
        m_Arm = arm;
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        m_Arm = null;
        base.StickExit();
    }

    public void Update()
    {
#warning ゲットコンポーネントしすぎ
        var spoint = transform.GetComponentsInChildren<StickPoint>();
        foreach(var sp in spoint)
        {
            sp.gameObject.transform.localPosition = Vector3.zero;
        }

        if(m_Arm) transform.up = m_Arm.transform.up;
    }
}
