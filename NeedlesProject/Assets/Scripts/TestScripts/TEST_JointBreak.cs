using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_JointBreak : MonoBehaviour {

    public HingeJoint hinge;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            hinge.breakForce = 0;
            hinge.breakTorque = 0;
        }
	}
}
