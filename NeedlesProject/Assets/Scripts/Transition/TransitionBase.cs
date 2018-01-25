using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TransitionBase : MonoBehaviour
{
    [SerializeField]
    private bool  fadeStart;
    private float amount;

	public enum FadeType
	{
		FadeIn,
		FadeOut,
	}

    public delegate void OnFadeCompleteHandler(FadeType type);
    public event OnFadeCompleteHandler OnFadeComplete;

    public float Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    /// <summary>フェードインの開始</summary>
    public Coroutine FadeInStart(float fadeSpeed = 1.0f)
    {
        return StartCoroutine(FadeIn(fadeSpeed));
    }

    /// <summary>フェードアウトの開始</summary>
    public Coroutine FadeOutStart(float fadeSpeed = 1.0f)
    {
        return StartCoroutine(FadeOut(fadeSpeed));
    }

    /// <summary>フェードの一時停止</summary>
    public void FadeStop()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        string tmp = PlayerPrefs.GetString(PrefsDataName.FadeStart);
        if (tmp == bool.TrueString)
        {
            fadeStart = true;
        }
    }

    protected virtual void Start()
    {
        if (fadeStart)
        {
            FadeOutStart();
        }
        else
        {
            Amount = 0.0f;
        }
    }

    /// <summary>フェードイン</summary>
    private IEnumerator FadeIn(float fadeSpeed)
    {
        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            amount = t;
            ChangeValue(amount);
            yield return null;
        }
        amount = 1.0f;
        SendFadeComplete(FadeType.FadeIn);
    }

    /// <summary>フェードアウト</summary>
    private IEnumerator FadeOut(float fadeSpeed)
    {
        for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * fadeSpeed)
        {
            amount = t;
            ChangeValue(amount);
            yield return null;
        }
        amount = 0.0f;
        SendFadeComplete(FadeType.FadeOut);
    }

    protected abstract void ChangeValue(float amount);

    private void SendFadeComplete(FadeType fadeType)
    {
        if (OnFadeComplete != null)
        {
            OnFadeComplete(fadeType);
        }
    }
}
