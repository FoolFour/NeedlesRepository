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
        if (!IsGamePlay()) return;
        if (Input.GetButtonDown(GamePad.Pause))
        {
            if (isPause)
            {
                isPause = false;
                Pauser.Resume();
            }
            else
            {
                isPause = true;
                Pauser.Pause();
            }
        }
    }

    bool IsGamePlay()
    {
        return GameManagers.Instance.GameStateManager.GetCurrentGameState() == GameState.Play;
    }
}
