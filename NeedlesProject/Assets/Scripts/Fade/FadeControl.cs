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
        int tmp = PlayerPrefs.GetInt(PrefsDataName.FadeStart);
        if(tmp == 1)
        {
            isFadeStart = true;
        }
    }

    void Start()
    {
        if (isFadeStart)
        {
            float r = PlayerPrefs.GetFloat(PrefsDataName.Fade_R);
            float g = PlayerPrefs.GetFloat(PrefsDataName.Fade_G);
            float b = PlayerPrefs.GetFloat(PrefsDataName.Fade_B);

            fadeImage.FadeOutStart(new Color(r, g, b, 1.0f));
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
