using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseUI : Pauser
{
    SceneChangeFade  changeFade;
    SceneChangeRetry changeRetry;

    public string pauseSE;
    public string disableSE;

    //////////////////
    // 関数(public)　/
    ////////////////

    public void OnClickReturnGameButton()
    {
        //ポーズを解除する処理
        Resume();
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

    protected override void OnPause()
    {
        Sound.PlaySe(pauseSE);
        base.OnPause();
    }

    protected override void OnResume()
    {
        Sound.PlaySe(disableSE);
        base.OnResume();
    }
}
