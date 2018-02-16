using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoalSend2 : BlockBase
{
    private GameObject m_VideoObject;

    // Use this for initialization
    void Start ()
    {
        m_VideoObject = GameObject.Find("VideoCanvas");
        if (m_VideoObject == null) Destroy(this);
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        Destroy(m_VideoObject);
    }


}
