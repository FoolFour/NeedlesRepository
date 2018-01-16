using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotation : MonoBehaviour {

    public float RotationSpeed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, RotationSpeed, 0);
    }
}
