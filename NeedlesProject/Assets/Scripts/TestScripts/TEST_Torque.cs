using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Torque : MonoBehaviour {

    public Rigidbody rb;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("torque上昇");
            rb.angularVelocity += Vector3.forward * 100;
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("torque減少");
            rb.angularVelocity += Vector3.forward * -100;
        }
    }
}
