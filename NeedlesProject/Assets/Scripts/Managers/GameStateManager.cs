using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Ready,
    Play,
    End,
} 

public class GameStateManager : MonoBehaviour {

    public GameState m_gameState;

	// Use this for initialization
	void Start ()
    {
        m_gameState = GameState.Ready;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void StateChange(GameState state)
    {

    }
}
