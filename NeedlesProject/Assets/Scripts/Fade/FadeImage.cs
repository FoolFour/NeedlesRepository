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

    //////////
    // 変数 /
    ////////

    private bool  isFadeStart_;
    public  bool  isFadeStart
    {
        get { return isFadeStart_;  }
        set { isFadeStart_ = value; }
    }

    private float fadeSpeed_ = 1.0f;
    public  float fadeSpeed
    {
        get { return fadeSpeed_;  }
        set { fadeSpeed_ = value; }
    }


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
    [ContextMenu("FadeIn")]
    public Coroutine FadeInStart()
    {
        return StartCoroutine(FadeIn());
    }

    /// <summary>フェードインの開始</summary>
    public Coroutine FadeInStart(Color c)
    {
        color = c;
        return FadeInStart();
    }

    /// <summary>フェードアウトの開始</summary>
    [ContextMenu("FadeOut")]
    public Coroutine FadeOutStart()
    {
        return StartCoroutine(FadeOut());
    }

    /// <summary>フェードアウトの開始</summary>
    public Coroutine FadeOutStart(Color c)
    {
        color = c;
        return FadeOutStart();
    }

    /// <summary>フェードの一時停止</summary>
    public void FadeStop()
    {
        StopAllCoroutines();
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    protected override void Start()
    {
        if (isFadeStart)
        {
            const string Fade = "Fade";
            float r = PlayerPrefs.GetFloat(Fade + "_R");
            float g = PlayerPrefs.GetFloat(Fade + "_G");
            float b = PlayerPrefs.GetFloat(Fade + "_B");

            FadeOutStart(new Color(r, g, b));
        }
        else
        {
            SetAlpha(0.0f);
        }
    }

    /// <summary>フェードイン</summary>
    private IEnumerator FadeIn()
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
    private IEnumerator FadeOut()
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
