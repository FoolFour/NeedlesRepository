using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_blockmove : MonoBehaviour {

    public float timer = 0;
    public float move = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += Vector3.left * move;
        if(timer > 2)
        {
            timer = 0;
            move *= -1;
        }
        timer += Time.deltaTime;
	}
}
