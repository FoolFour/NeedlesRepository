using System.Collections;
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
    private RaycastHit m_Hitinfo;
    /// <summary>
    /// 当たった場所の最初のアンカーポイント（Local）
    /// </summary>
    private Vector3 m_FastAnchor = Vector3.zero;
    /// <summary>
    /// 1フレ前のスティックの傾き
    /// </summary>
    private float m_PrevDefeated;
    //---------------------------------------------------------------------------

    //遠心力---------------------------------------------------------------------
    private Vector3 m_PrevRotate;
    public float m_Centrifugalforce = 10;
    //---------------------------------------------------------------------------

    //数値データ------------------------------------------------------------------
    [SerializeField, TooltipAttribute("腕の最大の長さ")]
    public float m_ArmMaxLength = 5;
    [SerializeField, TooltipAttribute("トルクの回転力")]
    public float m_TorquePower = 30;
    [SerializeField, TooltipAttribute("トルクの最大回転力")]
    public float m_TorqueMaxPower = 30;
    //----------------------------------------------------------------------------

    //ゲーム的データ--------------------------------------------------------------
    public float m_MaxGripPower = 100;
    private float m_GripPower = 0;
    public float m_GripLowerPower = 0.1f;
    public float m_ImpactPower = 5;
    //----------------------------------------------------------------------------

    float m_ArmCurrentLenght;
    int m_Ignorelayer = 1 << 9; //ブロックのみ当たる


    public void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_CurrentHitObject = (GameObject)Instantiate(m_HitObjectPrefab,Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 腕を伸ばす処理
    /// </summary>
    /// <param name="stickdir"></param>
    public void ArmExtend(Vector3 stickdir)
    {
        ////スティックが倒されている強さを計算する
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        //腕を急速旋回させるための処理
        if (stickdir != Vector3.zero && m_ArmCurrentLenght == 0)
        {
            float dir = Mathf.Sign(Vector2Cross(Vector3.up, stickdir.normalized));
            float rotate = Vector3.Angle(Vector3.up, stickdir.normalized);
            m_rb.rotation = Quaternion.AngleAxis(dir * rotate, Vector3.forward);
            m_ArmCurrentLenght = defeated * m_ArmMaxLength;
            return;

        }
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;

        if (m_ArmCurrentLenght == 0){ m_Arm.GetComponent<CapsuleCollider>().enabled = false; }
        else { m_Arm.GetComponent<CapsuleCollider>().enabled = true; }

        Debug.DrawRay(transform.position, stickdir.normalized * m_ArmCurrentLenght, mDebugColor);
        Debug.DrawRay(transform.position, m_Arm.up * m_ArmCurrentLenght, mDebugColor);


        if (Physics.Raycast(transform.position, m_Arm.up, out m_Hitinfo, m_ArmCurrentLenght + 1.0f, m_Ignorelayer) && m_ArmCurrentLenght != 0 && defeated > 0.4f)
        {

            //ブロックに当たった時
            m_Hitinfo.collider.GetComponent<BlockBase>().StickEnter(gameObject);
            m_ArmCurrentLenght = Vector3.Distance(m_Hitinfo.point + (m_Hitinfo.normal * 0.5f), transform.position);

            //ブロックが刺さる場合
            if (m_Hitinfo.collider.GetComponent<BlockBase>().isHitBlock)
            {
                m_Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().velocity = Vector3.zero;

                m_Hitinfo.point = m_Hitinfo.point + (m_Hitinfo.normal * 0.5f);
                m_CurrentHitObject.transform.position = m_Hitinfo.point;

                var hinge = m_CurrentHitObject.AddComponent<HingeJoint>();
                hinge.connectedBody = m_rb;
                hinge.autoConfigureConnectedAnchor = false;
                hinge.connectedAnchor = Vector3.up * m_ArmCurrentLenght;

                ishit = true;
                m_PrevRotate = m_Arm.up;
                m_PrevDefeated = defeated;
                return;
            }
        }

        m_Arm.localScale = new Vector3(3f, m_ArmCurrentLenght, 1.5f);
        float angle = Vector2Cross(m_Arm.up, stickdir.normalized);
        m_rb.centerOfMass = transform.localPosition;
        m_rb.angularVelocity = Vector3.forward * 50 * angle;
        m_Hand.position = transform.position + (m_Arm.up * (m_ArmCurrentLenght));
        m_Hand.up = m_Arm.up;

    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;
        var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();
        Debug.Log(Mathf.Abs(defeated - m_PrevDefeated));

        if (Mathf.Abs(defeated - m_PrevDefeated) > 0.2f || defeated == 0)
        {
            //腕が壁から外れる時に遠心力を作る
            //Vector3 dir = Vector3.Cross(m_Arm.up, Vector3.forward);
            //float power = m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity.z * m_ArmCurrentLenght * m_Centrifugalforce;
            //m_CurrentHitObject.GetComponent<StickPoint>().m_Centrifugalforce = dir.normalized * power;
            //m_Player.GetComponent<Rigidbody>().isKinematic = true; //isKinematicを利用して物理を初期化する

            hinge.breakTorque = 0;
            ishit = false;
            m_Arm.localRotation = Quaternion.identity;

            m_Hitinfo.collider.GetComponent<BlockBase>().StickExit();

            return;
        }
        m_PrevDefeated = defeated;
        m_PrevRotate = m_Arm.up;

        m_Hand.position = m_Hitinfo.point;
        m_Hand.up = -m_Hitinfo.normal;
        m_Arm.rotation = Quaternion.LookRotation(Vector3.forward,(m_Hand.position - m_Arm.position).normalized);
        float len = Vector3.Distance(m_Hand.position, transform.position);
        m_Arm.localScale = new Vector3(3f, len, 1.5f);

        //腕の回転処理
        float angle = Mathf.Sign(Vector2Cross(m_Arm.up, stickdir.normalized));
        m_CurrentHitObject.GetComponent<Rigidbody>().maxAngularVelocity = m_TorqueMaxPower;

        //左右に判定して壁だったら止める処理
        Vector3 startpoint = m_Arm.position + (m_Arm.right * angle * 0.5f);
        Vector3 endpoint = startpoint + m_Arm.up * len;
        Debug.DrawLine(startpoint, endpoint, Color.green);
        if (Physics.Linecast(startpoint, endpoint, m_Ignorelayer))
        {
            Debug.Log("当たった");
            m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            m_CurrentHitObject.GetComponent<Rigidbody>().AddTorque(transform.forward * ((m_TorquePower * angle) * m_ArmCurrentLenght), ForceMode.Force);
        }

        hinge.connectedAnchor = Vector3.up * m_ArmCurrentLenght;
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

    public float CurrentGripPower()
    {
        return m_GripPower;
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }

    public void PlayerAddForce()
    {
        if (m_Player.GetComponent<Rigidbody>().velocity.magnitude < 25)
        {
            m_Player.GetComponent<Rigidbody>().AddForce(-m_Arm.up * m_ImpactPower, ForceMode.Impulse);
        }
    }
}
