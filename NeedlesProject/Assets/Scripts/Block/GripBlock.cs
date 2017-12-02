using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripBlock : BlockBase
{
    private GameObject m_Arm;

    public override void StickEnter(GameObject arm)
    {
        m_Arm = arm;
        Sound.PlaySe("Grip");
        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        m_Arm = null;
        base.StickExit();
    }

    public void Update()
    {
        if (m_Arm)
        {
            transform.GetChild(0).up = m_Arm.transform.up;
            var spoint = transform.GetComponentsInChildren<StickPoint>();
            if (spoint.Length == 0) m_Arm = null;
            foreach (var sp in spoint)
            {
                sp.gameObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}
