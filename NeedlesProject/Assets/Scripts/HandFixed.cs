using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手の部分を固定するスクリプト
/// </summary>
public class HandFixed : MonoBehaviour {

    /// <summary>
    /// 腕のオブジェクト
    /// </summary>
    public Transform mArm;

	void Update ()
    {
        transform.localPosition = mArm.localPosition + (mArm.forward * mArm.localScale.z);
	}
}
