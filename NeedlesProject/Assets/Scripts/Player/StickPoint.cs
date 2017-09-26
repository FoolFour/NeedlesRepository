using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPoint : MonoBehaviour {

    void OnJointBreak(float breakForce)
    {
        GetComponent<HingeJoint>().connectedBody.velocity = Vector3.zero;
        GetComponent<HingeJoint>().connectedBody.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
