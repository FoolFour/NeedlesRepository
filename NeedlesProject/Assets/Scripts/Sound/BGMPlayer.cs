using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public string m_bgmKey;

    // Use this for initialization
    void Start()
    {
        if (Sound.CheckBGMSame(m_bgmKey))
        {
            Sound.ChangeBgmVolume(1.0f);
        }
        else
        {
            Sound.PlayBgm(m_bgmKey);
        }
    }

    public void FadeOut(float speed = 1.01f)
    {
        StartCoroutine(FadeOutBgm(speed));
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
