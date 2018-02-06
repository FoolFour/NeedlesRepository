﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クリア条件を満たしているか確認し次のシーンに進めるクラス
/// </summary>
public class ClearChecker : MonoBehaviour {

    /// <summary>
    /// クリア条件を監視するクラス
    /// </summary>
    public IConditions m_Conditions;
    /// <summary>
    /// 次のシーンに進めるか？
    /// </summary>
    public bool isNext = false;

    //プレハブ(fader) or SceneChangeTimerの入ったオブジェクトを入れる
    public SceneChanger sceneChanger;

    public string m_Scene = "none";
    public float m_delay = 1.0f;

    public RectTransform m_ClearImage;

    // Use this for initialization
    void Start ()
    {
        isNext = false;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_Conditions.IsClear())
        {
            Sound.PlaySe("TutorialClear");
            StartCoroutine(DelaySceneChange(m_delay));
            this.enabled = false;
        }	
	}

    IEnumerator DelaySceneChange(float second)
    {
        {
            float t = 0;
            var scalefrom = m_ClearImage.localScale;
            var scaleto = new Vector3(1f, 1f, 1.0f);
            while (t <= 1)
            {
                t += 0.05f;
                m_ClearImage.localScale = Vector3.Lerp(scalefrom, scaleto, Mathf.SmoothStep(0, 1, t));
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(second);
        sceneChanger.SceneChange(m_Scene);
    }
}
