using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripBlock : BlockBase
{

    public override void StickEnter(GameObject arm)
    {
        base.StickEnter(arm);
    }

    public void Update()
    {
        var spoint = transform.GetComponentsInChildren<StickPoint>();
        foreach(var sp in spoint)
        {
            sp.gameObject.transform.localPosition = Vector3.zero;
        }
    }
}
