using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductionBlocks : MonoBehaviour {
    //電導スイッチのブロックの処理

    //スイッチのon,off
    public bool blockSwitch = false; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //エフェクトなど表示するかも
        if (blockSwitch == true)
        {

        }
	}

    void OnTriggerStay(Collider other)
    {
        blockSwitch = true;
    }


    void OnTriggerExit(Collider other)
    {
        blockSwitch = false;
    }
}
