using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Result
{
    public class ResultData : MonoBehaviour
    {
        ///////////////////
        // 変数(private) /
        /////////////////
        private float        time;
        private int          coin;

        private GameTimer    timer;

        private CoinCounting counting;

        private string       stage;
        private string       missionInfo1;
        private string       missionInfo2;

        ////////////////////////
        // プロパティ(public) /
        //////////////////////
        public float  clearTime
        {
            get { return timer.gameTimeNoPauseTime; }
        }

        public int    resultGetCoin
        {
            get { return counting.playerGetCoinNum; }
        }

        public string stageName
        {
            get { return stage; }
        }

        public string mission1
        {
            get { return missionInfo1; }
        }

        public string mission2
        {
            get { return missionInfo2; }
        }

        ///////////////////
        // 関数(private) /
        /////////////////

        private void Awake()
        {
            string sceneName = PlayerPrefs.GetString(PrefsDataName.Scene);

            //FindSceneObjectOfTypeは現在のクラスの関数

            timer    = FindSceneObjectOfType<GameTimer>(sceneName);
            time     = timer.gameTimeNoPauseTime;

            counting = FindSceneObjectOfType<CoinCounting>(sceneName);
            coin     = counting.playerGetCoinNum;
            
            stage        = PlayerPrefs.GetString(PrefsDataName.StageName);
            missionInfo1 = PlayerPrefs.GetString(PrefsDataName.Mission1);
            missionInfo2 = PlayerPrefs.GetString(PrefsDataName.Mission2);
        }

        private T FindSceneObjectOfType<T>(string sceneName)
        {
            Scene stageScene = SceneManager.GetSceneByName(sceneName);
            GameObject[] obj = stageScene.GetRootGameObjects();

            foreach (var item in obj)
            {
                var component = item.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return default(T);
        }
    }
}