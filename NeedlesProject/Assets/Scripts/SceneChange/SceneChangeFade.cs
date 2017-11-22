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

        const string Fade = "Fade";
        PlayerPrefs.SetFloat(Fade + "_R", color.r);
        PlayerPrefs.SetFloat(Fade + "_G", color.g);
        PlayerPrefs.SetFloat(Fade + "_B", color.b);
    }
}
