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

        PlayerPrefs.SetString(PrefsDataName.FadeStart, bool.TrueString);
    }
}
