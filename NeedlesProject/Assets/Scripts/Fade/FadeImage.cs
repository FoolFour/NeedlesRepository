using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum FadeType
{
    FadeIn,
    FadeOut,
}

/// <summary>フェード用画像</summary>
public class FadeImage : Image
{
    //////////////
    // イベント /
    ////////////

    //フェード完了
    public delegate void OnFadeCompleteHandler(FadeType type);
    public event OnFadeCompleteHandler OnFadeComplete;



    //////////
    // 変数 /
    ////////

    private float fadeSpeed_ = 1.0f;
    public  float fadeSpeed
    {
        get { return fadeSpeed_; }
        set { fadeSpeed_ = value; }
    }

    private IEnumerator fadeCoroutine_;



    //////////////////
    // 関数(public) /
    ////////////////

    /// <summary>アルファ値を変更します</summary>
    public void SetAlpha(float alpha)
    {
        Color col = color;
        col.a = alpha;
        color = col;
    }

    /// <summary>フェードインの開始</summary>
    public void FadeInStart()
    {
        fadeCoroutine_ = FadeIn();
        StartCoroutine(fadeCoroutine_);
    }

    /// <summary>フェードインの開始</summary>
    public void FadeInStart(Color c)
    {
        color = c;
        FadeInStart();
    }

    /// <summary>フェードアウトの開始</summary>
    public void FadeOutStart()
    {
        fadeCoroutine_ = FadeOut();
        StartCoroutine(fadeCoroutine_);
    }

    /// <summary>フェードアウトの開始</summary>
    public void FadeOutStart(Color c)
    {
        color = c;
        FadeOutStart();
    }

    /// <summary>フェードの一時停止</summary>
    public void FadePause()
    {
        StopCoroutine(fadeCoroutine_);
    }

    /// <summary>フェードの再開</summary>
    public void FadeResume()
    {
        StartCoroutine(fadeCoroutine_);
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    /// <summary>フェードイン</summary>
    private IEnumerator FadeIn()
    {
        for(float t = 0.0f; t <= 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(1.0f);
        OnFadeComplete(FadeType.FadeIn);
    }

    /// <summary>フェードアウト</summary>
    private IEnumerator FadeOut()
    {
        for(float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * fadeSpeed)
        {
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(0.0f);
        OnFadeComplete(FadeType.FadeOut);
    }
}
