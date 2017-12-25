using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeTimer : SceneChanger
{
    [SerializeField]
    [Header("単位:秒")]
    private uint           changeTimer;

    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        wait = new WaitForSeconds(changeTimer);
    }

    protected override IEnumerator SceneChangePerformance()
    {
        yield return wait;
        PlayerPrefs.SetString(PrefsDataName.FadeStart, bool.FalseString);
    }
}
