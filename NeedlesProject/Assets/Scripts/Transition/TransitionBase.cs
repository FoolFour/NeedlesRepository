using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TransitionBase : MonoBehaviour
{
    [SerializeField]
    private bool  fadeStart;

    [SerializeField]
    private bool  fadeNoStart = false;

    private float amount;

    public float FadeSpeed { get; private set; }

	public enum FadeType
	{
        None,
		FadeIn,
		FadeOut,
	}

    public delegate void OnFadeCompleteHandler(FadeType type);
    public event OnFadeCompleteHandler OnFadeComplete;

    public FadeType FadeState { get; private set; }

    public float Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    /// <summary>フェードインの開始</summary>
    public Coroutine FadeInStart(float fadeSpeed = 1.0f)
    {
        FadeSpeed = fadeSpeed;
        return StartCoroutine(FadeIn(fadeSpeed));
    }

    /// <summary>フェードアウトの開始</summary>
    public Coroutine FadeOutStart(float fadeSpeed = 1.0f)
    {
        FadeSpeed = fadeSpeed;
        return StartCoroutine(FadeOut(fadeSpeed));
    }

    /// <summary>フェードの一時停止</summary>
    public void FadeStop()
    {
        StopAllCoroutines();
        FadeState = FadeType.None;
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
        if (fadeStart && !fadeNoStart)
        {
            FadeOutStart();
        }
        else
        {
            amount = 0.0f;
            ChangeValue(amount);
        }
    }

    /// <summary>フェードイン</summary>
    private IEnumerator FadeIn(float fadeSpeed)
    {
        FadeState = FadeType.FadeIn;
        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            amount = t;
            ChangeValue(amount);
            yield return null;
        }
        amount = 1.0f;
        SendFadeComplete(FadeType.FadeIn);
        FadeState = FadeType.None;
    }

    /// <summary>フェードアウト</summary>
    private IEnumerator FadeOut(float fadeSpeed)
    {
        FadeState = FadeType.FadeOut;
        for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * fadeSpeed)
        {
            amount = t;
            ChangeValue(amount);
            yield return null;
        }
        amount = 0.0f;
        SendFadeComplete(FadeType.FadeOut);
        FadeState = FadeType.None;

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
