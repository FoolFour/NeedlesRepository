﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの腕クラス
/// </summary>
public class NeedleArm : MonoBehaviour
{
    //デバッグ用変数　後で消す
    public Color mDebugColor;
    //-------------------------

    //プレイヤーのトランスフォーム類----------------------------------------------
    /// <summary>
    /// 自身のRigidbody
    /// </summary>
    private Rigidbody m_rb;
    /// <summary>
    /// プレイヤーモデル
    /// </summary>
    public Transform m_Player;
    /// <summary>
    /// 腕の部分モデル
    /// </summary>
    public Transform m_Arm;
    /// <summary>
    /// 手の部分モデル
    /// </summary>
    public Transform m_Hand;
    //----------------------------------------------------------------------------

    //プレハブ系--------------------------------------------------------------
    [SerializeField, TooltipAttribute("StickPointのプレハブを入れる")]
    public GameObject m_HitObjectPrefab;
    //↑の実体を保存する変数
    private GameObject m_CurrentHitObject;
    //----------------------------------------------------------------------------

    //壁に刺さった時の判定系------------------------------------------------------
    /// <summary>
    /// 腕が壁に当たっているか？
    /// </summary>
    bool ishit = false;
    /// <summary>
    /// あたった場所を保存しておく
    /// </summary>
    private Vector3 m_HitPoint;
    /// <summary>
    /// 当たった場所の最初のアンカーポイント（Local）
    /// </summary>
    private Vector3 m_FastAnchor = Vector3.zero;
    //---------------------------------------------------------------------------

    //数値データ------------------------------------------------------------------
    [SerializeField, TooltipAttribute("腕の最大の長さ")]
    public float m_ArmMaxLength = 5;
    [SerializeField, TooltipAttribute("トルクの回転力")]
    public float m_TorquePower = 300;
    //----------------------------------------------------------------------------

    float m_ArmCurrentLenght;
    int m_Ignorelayer = ~(1 << 8);


    public void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 腕を伸ばす処理
    /// </summary>
    /// <param name="stickdir"></param>
    public void ArmExtend(Vector3 stickdir)
    {
        ////スティックが倒されている強さを計算する
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        if (stickdir != Vector3.zero && m_ArmCurrentLenght == 0)
        {
            float dir = Mathf.Sign(Vector2Cross(Vector3.right, stickdir.normalized));
            float rotate = Vector3.Angle(Vector3.right, stickdir.normalized);
            m_rb.rotation = Quaternion.AngleAxis(dir * rotate,Vector3.forward);
        }
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;

        Debug.DrawRay(transform.position, stickdir.normalized * m_ArmCurrentLenght, mDebugColor);
        Debug.DrawRay(transform.position, m_Arm.forward * m_ArmCurrentLenght, mDebugColor);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_Arm.forward, out hit, m_ArmCurrentLenght + 0.4f, m_Ignorelayer))
        {
            m_HitPoint = hit.point;
            m_CurrentHitObject = (GameObject)Instantiate(m_HitObjectPrefab, m_HitPoint, Quaternion.identity);
            var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();
            hinge.connectedBody = m_rb;
            m_FastAnchor = hinge.connectedAnchor;
            ishit = true;
        }

        m_Arm.localScale = new Vector3(1, 1, m_ArmCurrentLenght);
        float angle = Vector2Cross(transform.right, stickdir.normalized);
        m_rb.centerOfMass = transform.localPosition;
        m_rb.angularVelocity = Vector3.forward * 50 * angle;
        m_Hand.position = m_Arm.position + (m_Arm.forward.normalized * (m_Arm.localScale.z));

    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;
        var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();

        if (defeated < 0.4f)
        {
            m_Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Vector3 dir = Vector3.Cross(m_Arm.forward,Vector3.forward);
            //float power = m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity.z;
            //Debug.Log(power);
            //m_Player.GetComponent<Rigidbody>().AddForce(dir * (power * 50 * m_ArmCurrentLenght),ForceMode.Impulse);
            Destroy(m_CurrentHitObject);
            ishit = false;
            return;
        }

        float powers = m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity.z;
        Debug.Log(powers);

        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = m_FastAnchor.normalized * m_ArmCurrentLenght;

        float len = Vector3.Distance(m_HitPoint,transform.position); 
        m_Arm.localScale = new Vector3(1, 1, len);
        m_Hand.position = m_HitPoint;

        float angle = Mathf.Sign(Vector2Cross(m_Arm.forward, stickdir.normalized));
        m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity =Vector3.forward * ((m_TorquePower * angle) * m_ArmCurrentLenght);
    }

    /// <summary>
    /// ２つ刺さった時に物理の動きを止める用の関数
    /// </summary>
    public void StopPhysics()
    {
        //if (mCurrentHitObject == null) return;
        //var rb = mCurrentHitObject.GetComponent<Rigidbody>();
        //rb.angularVelocity = Vector3.zero;
        //rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 針が壁に刺さったか？
    /// </summary>
    /// <returns>trueなら刺さっている</returns>
    public bool IsHit()
    {
        return ishit;
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
