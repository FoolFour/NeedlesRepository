using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour {

    public GameObject gameobj;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = gameobj.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(-1,0,0,ForceMode.Impulse);
	}


}
