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

    protected override IEnumerator SceneChangePerformance()
    {
        yield return image.FadeInStart(color);
    }
}
