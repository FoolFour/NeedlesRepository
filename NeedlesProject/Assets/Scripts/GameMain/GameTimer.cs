using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameTimer : Pauser
{
    ///////////////////
    // 変数(private) /
    /////////////////

    private float gameTime_;
    private float pauseTime_;

    [SerializeField]
    private bool isPause_;

    ////////////////////////
    // プロパティ(public) /
    //////////////////////

    /// <summary>ゲームの経過時間(ポーズを含める)</summary>
    public float gameTime
    {
        get         { return gameTime_;  }
        private set { gameTime_ = value; }
    }

    /// <summary>ゲームの経過時間(ポーズを含めない)</summary>
    public float gameTimeNoPauseTime
    {
        get         { return gameTime_ - pauseTime_; }
    }

    /// <summary>ポーズの経過時間</summary>
    public float pauseTime
    {
        get         { return pauseTime_;  }
        private set { pauseTime_ = value; }
    }

    /// <summary>ポーズするか</summary>
    public bool isPause
    {
        get         { return isPause_;  }
        set         { isPause_ = value; }
    }

    //////////////////
    // 関数(public)　/
    ////////////////

    public IEnumerator SetAlarm(float currentTime, UnityAction action)
    {
        while (currentTime < gameTime)
        {
            yield return null;
        }
        action();
    }

    /////////////////////
    // 関数(protected)　/
    ///////////////////

    protected override void OnPause()
    {
        isPause = true;
    }

    protected override void OnResume()
    {
        isPause = false;
    }

    ///////////////////
    // 関数(private)　/
    /////////////////

    private void Awake()
    {
        gameTime  = 0.0f;
        pauseTime = 0.0f;
    }

    private void Update()
    {
        gameTime  += Time.deltaTime;

        if (isPause)
        {
            pauseTime += Time.deltaTime;
        }
    }
}
