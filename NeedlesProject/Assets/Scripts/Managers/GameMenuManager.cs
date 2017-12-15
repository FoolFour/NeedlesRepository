using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsGamePlay()) return;
        if (Input.GetButtonDown(GamePad.Pause))
        {
            if (Pauser.isPause)
            {
                Pauser.Resume();
            }
            else
            {
                Pauser.Pause();
            }
        }
    }

    bool IsGamePlay()
    {
        return GameManagers.Instance.GameStateManager.GetCurrentGameState() == GameState.Play;
    }
}
