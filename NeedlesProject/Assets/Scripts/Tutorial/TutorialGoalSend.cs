using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoalSend : BlockBase
{
    private GameObject m_VideoObject;

    public void Start()
    {
        m_VideoObject = GameObject.Find("VideoImage");
    }

    public override void StickHit(GameObject arm, GameObject stickpoint, RaycastHit hitdata)
    {
        Destroy(m_VideoObject);
        base.StickHit(arm, stickpoint, hitdata);
    }
}
