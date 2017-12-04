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
    public GameObject obj;
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
    /// <summary>
    /// 物理の不正の計測タイマー
    /// </summary>
    private float m_BreakTimer = 0;
    /// <summary>
    /// この時間、腕の芯部分に当たっていると壊れるように
    /// </summary>
    public float m_BreakTime = 3;
    //---------------------------------------------------------------------------

    //数値データ------------------------------------------------------------------
    [SerializeField, TooltipAttribute("腕の最大の長さ")]
    public float m_ArmMaxLength = 5;
    [SerializeField, TooltipAttribute("トルクの回転力")]
    public float m_TorquePower = 30;
    [SerializeField, TooltipAttribute("トルクの最大回転力")]
    public float m_TorqueMaxPower = 30;
    //----------------------------------------------------------------------------

    float m_ArmCurrentLenght;
    int m_Ignorelayer = 1 << 9; //ブロックのみ当たる


    public void OnEnable()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        m_CurrentHitObject = (GameObject)Instantiate(m_HitObjectPrefab, Vector3.zero, Quaternion.identity);
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
            m_ArmCurrentLenght = defeated * m_ArmMaxLength;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, stickdir.normalized);
            Sound.PlaySe("Extend");
            return;

        }
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;

        if (m_ArmCurrentLenght == 0) { m_Arm.GetComponent<CapsuleCollider>().enabled = false; }
        else { m_Arm.GetComponent<CapsuleCollider>().enabled = true; }

        if (BlockHitCheck(defeated))
        {

            //ブロックに当たった時
            m_Hitinfo.collider.GetComponent<BlockBase>().StickEnter(gameObject);
            m_ArmCurrentLenght = Vector3.Distance(m_Hitinfo.point + (m_Hitinfo.normal * 0.5f), transform.position);

            //ブロックが刺さる場合
            if (m_Hitinfo.collider.GetComponent<BlockBase>().isHitBlock)
            {
                m_Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                m_rb.velocity = Vector3.zero;

                m_Hitinfo.point = m_Hitinfo.point + (m_Hitinfo.normal * 0.5f);
                m_CurrentHitObject.transform.parent = m_Hitinfo.collider.transform;
                m_CurrentHitObject.transform.position = m_Hitinfo.point;

                var hinge = m_CurrentHitObject.AddComponent<HingeJoint>();
                hinge.connectedBody = m_rb;
                hinge.autoConfigureConnectedAnchor = false;
                hinge.connectedAnchor = Vector3.up * m_ArmCurrentLenght;

                ishit = true;
                m_PrevDefeated = defeated;

                m_Hitinfo.collider.GetComponent<BlockBase>().StickHit(gameObject,m_CurrentHitObject);
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

    /// <summary>
    /// ブロックに刺さるかチェックする
    /// 最初のチェックはTriggerが真のブロック用
    /// ２回目のチェックは普通のブロックを判定
    /// </summary>
    bool BlockHitCheck(float defeated)
    {
        var armPoint1 = m_Arm.position + (m_Arm.up * m_Arm.localScale.y);
        var armPoint2 = m_Arm.position + (m_Arm.up * (m_ArmCurrentLenght + 1));
        RaycastHit hit;
        if (defeated == 0) return false;
        if (Physics.Linecast(armPoint1, armPoint2, out hit, m_Ignorelayer) && defeated != 0)
        {
            if (hit.collider.isTrigger)
            {
                m_Hitinfo = hit;
                return true;
            }
        }
        if (Physics.Raycast(transform.position, m_Arm.up, out hit, m_ArmCurrentLenght + 1, m_Ignorelayer,QueryTriggerInteraction.Ignore))
        {
            m_Hitinfo = hit;
            return true;
        }
        return false;
    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        //m_ArmCurrentLenght = defeated * m_ArmMaxLength;
        m_ArmCurrentLenght = Mathf.Lerp(m_ArmCurrentLenght, defeated * m_ArmMaxLength, 0.8f);
        var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();

        //傾きの差異を見て針を外す
        if (Mathf.Abs(defeated - m_PrevDefeated) > 0.2f || defeated == 0)
        {
            hinge.breakTorque = 0;
            Sound.PlaySe("Swish");
            m_CurrentHitObject.GetComponent<StickPoint>().BreakAction(() =>
            {
                {
                    var temp = m_Player.GetComponent<Rigidbody>().velocity;
                    temp = m_Arm.InverseTransformVector(temp);
                    temp.y = 0;
                    m_Player.GetComponent<Rigidbody>().velocity = m_Arm.TransformVector(temp);
                }
            }
            );

            m_CurrentHitObject.transform.parent = null;
            ishit = false;
            m_Arm.localRotation = Quaternion.identity;

            m_Hitinfo.collider.GetComponent<BlockBase>().StickExit();
            m_Hitinfo = new RaycastHit();
            return;
        }

        m_PrevDefeated = defeated;

        m_Hand.position = m_CurrentHitObject.transform.position;
        m_Hand.up = -m_Hitinfo.normal;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, (m_Hand.position - m_Arm.position).normalized);

        float len = Vector3.Distance(m_Hand.position, transform.position);
        m_Arm.localScale = new Vector3(3f, len, 1.5f);

        //Armが壊れてないかcheckする
        ArmBreakCheck(len);

        //腕の回転処理
        float angle = Mathf.Sign(Vector2Cross(m_Arm.up, stickdir.normalized));
        m_CurrentHitObject.GetComponent<Rigidbody>().maxAngularVelocity = m_TorqueMaxPower;

        //周りを判定して壁だったら止める処理
        if (CircumferenceCheck(angle, len))
        {
            m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else { m_CurrentHitObject.GetComponent<Rigidbody>().AddTorque(transform.forward * ((m_TorquePower * angle) * m_ArmCurrentLenght)); }
        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = Vector3.up * m_ArmCurrentLenght;

        m_Hitinfo.collider.GetComponent<BlockBase>().StickStay(gameObject,m_CurrentHitObject);
    }

    /// <summary>
    /// アームを元に戻す
    /// </summary>
    public void Return_Arm()
    {
        var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();
        if (hinge) Destroy(hinge);
        m_CurrentHitObject.transform.parent = null;
        m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        m_Hitinfo = new RaycastHit();

        ishit = false;
        m_ArmCurrentLenght = 0;
        m_Arm.localScale = new Vector3(3f, m_ArmCurrentLenght, 1.5f);
        m_Hand.position = transform.position + (m_Arm.up * (m_ArmCurrentLenght));
        m_Hand.up = m_Arm.up;
    }

    public void Dead()
    {
        Return_Arm();
        m_rb.velocity = Vector3.zero;
        m_CurrentHitObject.transform.parent = null;
    }

    public void Goal()
    {
        if(m_Hitinfo.collider.tag != "Finish")
        {
            Return_Arm();
        }
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

    public void PlayerAddForce(Vector3 force)
    {
        m_Player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        m_rb.AddForce(force, ForceMode.Impulse);
    }

    public void PlayerStan(Vector3 velocity)
    {
        m_Player.GetComponent<Player>().StanMode(velocity);
    }

    //周りにブロックがあるか判定する
    private bool CircumferenceCheck(float angle, float len)
    {
        //左右に壁があるか　
        Vector3 startpoint = m_Arm.position + (m_Arm.right * angle * 0.5f);
        Vector3 endpoint = startpoint + m_Arm.up * len;
        Debug.DrawLine(startpoint, endpoint, Color.green);

        //後ろに壁があるか？
        Vector3 temp = m_CurrentHitObject.transform.position + (-transform.up * m_ArmCurrentLenght);
        Debug.DrawLine(temp, transform.position, Color.green);

        return //Physics.Linecast(startpoint, endpoint, m_Ignorelayer,QueryTriggerInteraction.Ignore) ||
            Physics.Linecast(temp, transform.position, m_Ignorelayer, QueryTriggerInteraction.Ignore);
    }

    private void ArmBreakCheck(float len)
    {
        if (len >= m_ArmMaxLength + 6) PlayerStan(Vector3.zero);
        if (Physics.Linecast(m_Arm.transform.position, m_Arm.transform.position + (m_Arm.up * (len - 1)), m_Ignorelayer, QueryTriggerInteraction.Ignore))
        {
            m_BreakTimer += Time.deltaTime;
            if (m_BreakTimer > m_BreakTime)
            {
                Sound.PlaySe("");
                PlayerStan(Vector3.zero);
            }
        }
        else
        {
            m_BreakTimer = 0;
        }
    }

    public void MaxSpeed(float max)
    {
        var temp = m_rb.velocity;
        temp.x = Mathf.Clamp(temp.x, -max,max);
        temp.y = Mathf.Clamp(temp.y, -max, max);
        temp.z = Mathf.Clamp(temp.z, -max, max);
        m_rb.velocity = temp;
    }
}
