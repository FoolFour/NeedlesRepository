using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseUIData : MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

    private float     borderTime1_;
    private float     borderTime2_;
    
    public float gameTime
    {
        get { return gameTimer.gameTimeNoPauseTime; }
    }

    public float borderTime1
    {
        get { return borderTime1_; }
    }

    public float borderTime2
    {
        get { return borderTime2_; }
    }

    ///////////////////
    // 関数(private)　/
    /////////////////

    private void Reset()
    {
        gameTimer = FindObjectOfType<GameTimer>();
    }
    
    private void Awake()
    {
        if(gameTimer == null)
        {
            gameTimer = FindObjectOfType<GameTimer>();
        }

        borderTime1_ = PlayerPrefs.GetFloat(PrefsDataName.Border1);
        borderTime2_ = PlayerPrefs.GetFloat(PrefsDataName.Border2);
    }
}
