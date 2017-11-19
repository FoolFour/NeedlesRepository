using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ResultData : MonoBehaviour
{
    private float        time;
    private int          coin;

    private GameTimer    timer;

    private CoinCounting counting;

    public float clearTime
    {
        get { return timer.gameTimeNoPauseTime; }
    }

    public int   resultGetCoin
    {
        get { return counting.playerGetCoinNum; }
    }

    private IEnumerator Start()
    {
        string sceneName = PlayerPrefs.GetString("Scene");
        Scene stageScene = SceneManager.GetSceneByName(sceneName);
        GameObject[] obj = stageScene.GetRootGameObjects();

        foreach (var item in obj)
        {
            timer = item.GetComponent<GameTimer>();
            if(timer    != null)
            {
                time = timer.gameTimeNoPauseTime;
            }
        }

        yield return null;

        foreach (var item in obj)
        {
            counting = item.GetComponent<CoinCounting>();
            if(counting != null)
            {
                coin = counting.playerGetCoinNum;
            }
        }
    }
}
