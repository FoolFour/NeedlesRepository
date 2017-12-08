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
        private float        border1;
        private float        border2;

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

        public float  Border1
        {
            get { return border1; }
        }

        public float  Border2
        {
            get { return border2; }
        }

        ///////////////////
        // 関数(private)　/
        /////////////////

        private void Awake()
        {
            string sceneName = PlayerPrefs.GetString(PrefsDataName.Scene);

            //FindSceneObjectOfTypeは現在のクラスの関数

            timer    = FindSceneObjectOfType<GameTimer>(sceneName);
            time     = timer.gameTimeNoPauseTime;


            //counting = FindSceneObjectOfType<CoinCounting>(sceneName);
            //coin     = counting.playerGetCoinNum;
            
            stage   = PlayerPrefs.GetString(PrefsDataName.StageName);
            border1 = PlayerPrefs.GetFloat(PrefsDataName.Border1);
            border2 = PlayerPrefs.GetFloat(PrefsDataName.Border2);

            SubmitBestTIme(stageName, time);
            SubmitStageClear(stageName);

            PlayerPrefs.Save();
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

        private void SubmitBestTIme(string stageName, float new_time)
        {
            var   tmp_prefsName = PrefsDataName.StageTime(stageName);
            float old_time      = PlayerPrefs.GetFloat(tmp_prefsName);

            if(new_time < old_time)
            {
                //最速クリア更新!
                PlayerPrefs.SetFloat(tmp_prefsName, new_time);
            }
        }

        private void SubmitStageClear(string stageName)
        {
            //ステージクリアにする
            PlayerPrefs.SetString(PrefsDataName.StageClearFrag(stageName), bool.TrueString);
        }
    }
}