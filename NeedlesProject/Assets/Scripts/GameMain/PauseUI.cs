using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseUI : MonoBehaviour
{
    SceneChangeFade  changeFade;
    SceneChangeRetry changeRetry;

    //////////////////
    // 関数(public)　/
    ////////////////

    public void OnClickReturnGameButton()
    {
        //ポーズを解除する処理
        Pauser.Resume();
    }

    public void OnClickRetryButton()
    {
        //リトライ
        changeRetry.SceneChange();
    }

    public void OnClickStageSelectButton()
    {
        //ステージセレクト処理
        changeFade.SceneChange("StageSelect");
    }

    ///////////////////
    // 関数(private)　/
    /////////////////

    private void Awake()
    {
        changeFade  = FindObjectOfType<SceneChangeFade>();
        changeRetry = FindObjectOfType<SceneChangeRetry>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
