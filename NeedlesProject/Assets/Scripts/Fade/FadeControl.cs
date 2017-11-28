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
        int tmp = PlayerPrefs.GetInt("FadeStart");
        if(tmp == 1)
        {
            isFadeStart = true;
        }
    }

    void Start()
    {
        if (isFadeStart)
        {
            const string Fade = "Fade";
            float r = PlayerPrefs.GetFloat(Fade + "_R");
            float g = PlayerPrefs.GetFloat(Fade + "_G");
            float b = PlayerPrefs.GetFloat(Fade + "_B");

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
