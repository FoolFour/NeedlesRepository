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

    public GameState GetCurrentGameState()
    {
        return m_gameState;
    }

    public void StateChange(GameState state)
    {
        m_gameState = state;
    }
}
