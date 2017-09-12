using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヒンジジョイントのコネクト部分をアップデートさせる
/// （時々物理がスリープするので動かない時があるための処理）
/// </summary>
public class HingeManualConnectPointUpdate : MonoBehaviour
{
    private HingeJoint mHinge;

    public void Start()
    {
        mHinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        //Barの先に球を付けるための数値
        mHinge.connectedAnchor = Vector3.forward;
    }
}
