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


    [SerializeField, TooltipAttribute("腕の最大の長さ")]
    public float m_ArmMaxLength;
    float m_ArmCurrentLenght;

    int m_Ignorelayer = ~(1 << 8);

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

    /// <summary>
    /// 腕の方向を保持しておくための変数
    ///（ スティックが入力されてない時の対策）
    /// </summary>
    private Vector3 m_ArmDirection;

    /// <summary>
    /// 腕が壁に当たっているか？
    /// </summary>
    bool ishit = false;
    /// <summary>
    /// あたった場所を保存しておく
    /// </summary>
    private Vector3 m_HitPoint;
    /// <summary>
    /// 刺さったベクトル
    /// </summary>
    private Vector3 m_HitVector;
    /// <summary>
    /// あたった場所に置く物理オブジェクト
    /// </summary>
    public GameObject m_HitObjectPrefab;
    /// <summary>
    /// 当たった場所に置かれたオブジェクト
    /// </summary>
    private GameObject m_CurrentHitObject;
    /// <summary>
    /// 当たった場所の最初のアンカーポイント（Local）
    /// </summary>
    private Vector3 m_FastAnchor = Vector3.zero;


    public void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        m_ArmDirection = transform.forward;
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

        //m_rb.AddTorque(Vector3.forward * 50 * angle, ForceMode.Acceleration);

        ////現在の腕の向きを更新
        //if (stickdir != Vector3.zero) mArmDirection = stickdir;

        //Debug.DrawRay(transform.position, mArmDirection.normalized * mArmCurrentLenght, mDebugColor);

        ////腕の当たり判定のシュミレーションを行い押し出し判定
        //Vector3 next = ArmRotateColision(mArmDirection);

        //transform.localScale = new Vector3(1, 1, mArmCurrentLenght);
        //next = Vector3.Lerp(transform.forward, next, 0.5f);
        //transform.rotation = Quaternion.LookRotation(next.normalized);
        //mHand.position = transform.position + (transform.forward * mArmCurrentLenght);
        ////壁にハンド部分が刺さったかどうかの判定
        //RaycastHit hit;
        //if(Physics.Raycast(transform.position,next.normalized,out hit,mArmCurrentLenght+0.4f,mIgnorelayer))
        //{
        //    mHitPoint = hit.point;
        //    mHitVector = stickdir;
        //    mPlayer.GetComponent<Rigidbody>().isKinematic = true;
        //    ishit = true;
        //}
    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        m_ArmCurrentLenght = defeated * m_ArmMaxLength;

        if(defeated < 0.2f)
        {
            Destroy(m_CurrentHitObject);
            ishit = false;
            return;
        }
        var hinge = m_CurrentHitObject.GetComponent<HingeJoint>();
        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = m_FastAnchor.normalized * m_ArmCurrentLenght;

        float len = Vector3.Distance(m_HitPoint,transform.position); 
        m_Arm.localScale = new Vector3(1, 1, len);
        m_Hand.position = m_HitPoint;

        float angle = Mathf.Sign(Vector2Cross(m_Arm.forward, stickdir.normalized));
        Debug.Log("パワー:" + angle);
        m_CurrentHitObject.GetComponent<Rigidbody>().angularVelocity =Vector3.forward * ((300 * angle) * m_ArmCurrentLenght);



        //var hinge = gameObject.GetComponent<HingeJoint>();
        //hinge.anchor = transform.localPosition + (-m_Arm.forward * m_ArmCurrentLenght);
        //hinge.connectedAnchor = Vector3.zero;

        //Debug.DrawRay(mCurrentHitObject.transform.position, -mHitVector.normalized * mArmCurrentLenght, Color.yellow);
        //Debug.DrawRay(mHitPoint, -stickdir.normalized * mArmCurrentLenght, Color.green);

        //Vector3 next = -stickdir.normalized;
        //int angle = (int)Vector2.Angle(-transform.forward, next.normalized);
        //Vector3 start = m_HitPoint;
        //Vector3 checkvector = -transform.forward;
        ////左右のチェック
        //float LRCheck = -Mathf.Sign(Vector2Cross(transform.forward, next));
        //Debug.DrawLine(start + (checkvector.normalized * 0.5f), start + (checkvector * (m_ArmCurrentLenght - 0.4f)), Color.green);
        ////移動できるか１度ずつ調べてシュミレーションする
        //for (int i = 0; angle > i; i++)
        //{
        //    if (Physics.CheckCapsule(start + (checkvector.normalized * 0.5f), start + (checkvector * (m_ArmCurrentLenght - 0.4f)), 0.2f, m_Ignorelayer))
        //    {
        //        break;
        //    }
        //    next = checkvector;
        //    checkvector = Quaternion.AngleAxis(LRCheck, Vector3.forward) * checkvector.normalized;
        //}
        //next = Vector3.Lerp(transform.forward, -next, 0.5f);

        ////腕を伸ばすことが出来るか検索
        //RaycastHit hit;
        //if (Physics.Raycast(m_HitPoint, -next.normalized, out hit, m_ArmCurrentLenght, m_Ignorelayer))
        //{
        //    m_ArmCurrentLenght = Vector3.Distance(m_HitPoint, hit.point);
        //}


        //m_Player.localPosition = m_HitPoint + (-next.normalized * m_ArmCurrentLenght);
        //transform.localScale = new Vector3(1, 1, m_ArmCurrentLenght);
        //transform.rotation = Quaternion.LookRotation(next.normalized);
        //m_Hand.position = transform.position + (transform.forward * m_ArmCurrentLenght);
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

    /// <summary>
    /// 腕の回転時の押し出し処理
    /// </summary>
    /// <param name="next"></param>
    Vector3 ArmRotateColision(Vector3 next)
    {
        int angle = (int)Vector2.Angle(transform.forward, next.normalized);
        Vector3 start = transform.position;
        Vector3 checkvector = transform.forward;
        //左右のチェック
        float LRCheck = Mathf.Sign(Vector2Cross(transform.forward, next));
        //移動できるか１度ずつ調べてシュミレーションする
        for (int i = 0; angle > i; i++)
        {
            Debug.DrawLine(start, start + (checkvector * (m_ArmCurrentLenght - 0.4f)), Color.green);
            if (Physics.CheckCapsule(start, start + (checkvector * (m_ArmCurrentLenght - 1)), 0.2f, m_Ignorelayer))
            {
                return next;
            }
            next = checkvector;
            checkvector = Quaternion.AngleAxis(LRCheck, Vector3.forward) * checkvector.normalized;
        }
        return next;
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
