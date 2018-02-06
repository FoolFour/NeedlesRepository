using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            StartCoroutine(DelaySceneChange(m_delay));
            this.enabled = false;
        }	
	}

    IEnumerator DelaySceneChange(float second)
    {
        yield return new WaitForSeconds(second);
        sceneChanger.SceneChange(m_Scene);
    }
}
