using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnJointBreak(float breakForce)
    {
        GetComponent<HingeJoint>().connectedBody.velocity = Vector3.zero;
        GetComponent<HingeJoint>().connectedBody.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
