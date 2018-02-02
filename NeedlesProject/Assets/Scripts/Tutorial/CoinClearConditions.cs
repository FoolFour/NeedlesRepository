using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinClearConditions : IConditions {

	// Use this for initialization
	void Start ()
    {
        isClear = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(transform.childCount == 0)
        {
            isClear = true;
            Debug.Log("クリア");
        }	
	}
}
