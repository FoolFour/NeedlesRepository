using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeFade : SceneChanger
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////
    [SerializeField]
    private BlockTiling tiling;

    private void Reset()
    {
        tiling = FindObjectOfType<BlockTiling>();
    }

    protected override IEnumerator SceneChangePerformance()
    {
        yield return tiling.FadeInStart();

        PlayerPrefs.SetString(PrefsDataName.FadeStart, bool.TrueString);
    }
}
