using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeFade : SceneChanger
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////
    [SerializeField]
    private FadeImage image;

    [SerializeField]
    private Color     color;

    private void Reset()
    {
        image = FindObjectOfType<FadeImage>();
    }

    protected override IEnumerator SceneChangePerformance()
    {
        yield return image.FadeInStart(color);
        
        PlayerPrefs.SetFloat(PrefsDataName.Fade_R, color.r);
        PlayerPrefs.SetFloat(PrefsDataName.Fade_G, color.g);
        PlayerPrefs.SetFloat(PrefsDataName.Fade_B, color.b);

        PlayerPrefs.SetInt(PrefsDataName.FadeStart, 1);
    }
}
