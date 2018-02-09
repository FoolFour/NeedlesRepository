using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeFade : SceneChanger
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////
    [SerializeField]
    private TransitionBase tiling;

    private void Reset()
    {
        tiling = FindObjectOfType<TransitionBase>();
    }

    private void Start()
    {
        if(tiling == null)
        {
            tiling    = FindObjectOfType<TransitionBase>();
        }
    }

    protected override IEnumerator SceneChangePerformance()
    {
        yield return StartCoroutine(FadeOutBgm(1.0f));
        yield return tiling.FadeInStart();

        PlayerPrefs.SetString(PrefsDataName.FadeStart, bool.TrueString);
    }

    IEnumerator FadeOutBgm(float speed)
    {
        for(float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * speed)
        {
            Sound.ChangeBgmVolume(t);
            yield return null;
        }
    }
}
