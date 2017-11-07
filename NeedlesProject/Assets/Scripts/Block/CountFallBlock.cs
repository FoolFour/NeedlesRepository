using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountFallBlock : BlockBase {

    public override void StickEnter(GameObject arm)
    {
        StartCoroutine(DelayMethod(3.0f, () =>
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }));

        base.StickEnter(arm);
    }

    public override void StickExit()
    {
        base.StickExit();
    }

    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}
