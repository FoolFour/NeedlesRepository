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
    // イベント  /
    ////////////

    //フェード完了
    public delegate void OnFadeCompleteHandler(FadeType type);
    public event OnFadeCompleteHandler OnFadeComplete;


    //////////////////
    // 関数(public)　/
    ////////////////

    /// <summary>アルファ値を変更します</summary>
    public void SetAlpha(float alpha)
    {
        Color col = color;
        col.a     = alpha;
        color     = col;
    }

    /// <summary>フェードインの開始</summary>
    public Coroutine FadeInStart(float fadeSpeed = 1.0f)
    {
        return StartCoroutine(FadeIn(fadeSpeed));
    }

    /// <summary>フェードインの開始</summary>
    public Coroutine FadeInStart(Color c, float fadeSpeed = 1.0f)
    {
        color = c;
        return FadeInStart(fadeSpeed);
    }

    /// <summary>フェードアウトの開始</summary>
    public Coroutine FadeOutStart(float fadeSpeed = 1.0f)
    {
        return StartCoroutine(FadeOut(fadeSpeed));
    }

    /// <summary>フェードアウトの開始</summary>
    public Coroutine FadeOutStart(Color c, float fadeSpeed = 1.0f)
    {
        color = c;
        return FadeOutStart(fadeSpeed);
    }

    /// <summary>フェードの一時停止</summary>
    public void FadeStop()
    {
        StopAllCoroutines();
    }

    ///////////////////
    // 関数(private)　/
    /////////////////

    /// <summary>フェードイン</summary>
    private IEnumerator FadeIn(float fadeSpeed)
    {
        for(float t = 0.0f; t <= 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(1.0f);
        SendFadeComplete(FadeType.FadeIn);
    }

    /// <summary>フェードアウト</summary>
    private IEnumerator FadeOut(float fadeSpeed)
    {
        for(float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * fadeSpeed)
        {
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(0.0f);
        SendFadeComplete(FadeType.FadeOut);
    }

    private void SendFadeComplete(FadeType fadeType)
    {
        if(OnFadeComplete != null)
        {
            OnFadeComplete(fadeType);
        }
    }
}
