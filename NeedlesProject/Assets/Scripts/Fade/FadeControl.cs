using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeControl : MonoBehaviour
{
    //////////
    // 変数　/
    ////////
    public bool  isFadeStart = false;
    
    public float fadeSpeed   = 1.0f;

    FadeImage    fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<FadeImage>();
        string tmp = PlayerPrefs.GetString(PrefsDataName.FadeStart);
        if(tmp == bool.TrueString)
        {
            isFadeStart = true;
        }
    }

    void Start()
    {
        if (isFadeStart)
        {
            fadeImage.FadeOutStart();
        }
        else
        {
            fadeImage.SetAlpha(0.0f);
        }
    }

    private void Update()
    {

    }
}
