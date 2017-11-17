using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour {

    bool isPause = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (!isPause)
            {
                isPause = true;
                Pauser.Pause();
            }
            else
            {
                isPause = false;
                Pauser.Resume();
            }
        }	
	}
}
