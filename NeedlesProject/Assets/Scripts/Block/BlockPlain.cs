using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlain : BlockBase {

    public GameObject m_SmokePrefab;

    public override void StickEnter(GameObject arm)
    {
        Sound.PlaySe("Pick");
        base.StickEnter(arm);
    }

    public override void StickHit(GameObject arm, GameObject stickpoint,RaycastHit hitdata)
    {
        if (!m_SmokePrefab) return;
        GameObject smoke = (GameObject)Instantiate(m_SmokePrefab, stickpoint.transform.position, Quaternion.identity);
        smoke.transform.up = hitdata.normal;
        Destroy(smoke, 2.0f);
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
