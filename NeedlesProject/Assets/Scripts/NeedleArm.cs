using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの腕クラス
/// </summary>
public class NeedleArm : MonoBehaviour
{
    //デバッグ用変数　後で消す
    //-------------------------

    public Color mDebugColor;
    [SerializeField, TooltipAttribute("腕の最大の長さ")]
    public float mArmMaxLength;
    float mArmCurrentLenght;

    int mIgnorelayer = ~(1 << 8);

    /// <summary>
    /// 腕の方向を保持しておくための変数
    ///（ スティックが入力されてない時の対策）
    /// </summary>
    private Vector3 mArmDirection;

    /// <summary>
    /// 腕が壁に当たっているか？
    /// </summary>
    bool ishit = false;
    /// <summary>
    /// あたった場所を保存しておく
    /// </summary>
    private Vector3 mHitPoint;
    /// <summary>
    /// 刺さったベクトル
    /// </summary>
    private Vector3 mHitVector;
    /// <summary>
    /// あたった場所に置く物理オブジェクト
    /// </summary>
    public GameObject mHitObjectPrefab;
    /// <summary>
    /// 当たった場所に置かれたオブジェクト
    /// </summary>
    private GameObject mCurrentHitObject;
    /// <summary>
    /// 当たった場所の最初のアンカーポイント（Local）
    /// </summary>
    private Vector3 mFastAnchor = Vector3.zero;

    /// <summary>
    /// プレイヤーの情報
    /// </summary>
    public Transform mPlayer;
    /// <summary>
    /// 手の部分のオブジェクト
    /// </summary>
    public Transform mHand;

    public void Start()
    {
        mArmDirection = transform.forward;
    }

    /// <summary>
    /// 腕を伸ばす処理
    /// </summary>
    /// <param name="stickdir"></param>
    public void ArmExtend(Vector3 stickdir)
    {
        //スティックが倒されている強さを計算する
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        mArmCurrentLenght = defeated * mArmMaxLength;

        //現在の腕の向きを更新
        if (stickdir != Vector3.zero) mArmDirection = stickdir;

        Debug.DrawRay(transform.position, mArmDirection.normalized * mArmCurrentLenght, mDebugColor);

        //腕の当たり判定のシュミレーションを行い押し出し判定
        Vector3 next = ArmRotateColision(mArmDirection);

        transform.localScale = new Vector3(1, 1, mArmCurrentLenght);
        transform.rotation = Quaternion.LookRotation(next.normalized);
        mHand.position = mPlayer.localPosition + (transform.forward * mArmCurrentLenght);

        //壁にハンド部分が刺さったかどうかの判定
        RaycastHit hit;
        if(Physics.Raycast(transform.position,next.normalized,out hit,mArmCurrentLenght+0.4f,mIgnorelayer))
        {
            mCurrentHitObject = (GameObject)Instantiate(mHitObjectPrefab, hit.point, Quaternion.identity);

            var hinge = mCurrentHitObject.GetComponent<HingeJoint>();
            hinge.connectedBody = mPlayer.GetComponent<Rigidbody>();
            mFastAnchor = hinge.connectedAnchor;
            mHitPoint = hit.point;
            mHitVector = stickdir;
            ishit = true;
        }
    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        mArmCurrentLenght = defeated * mArmMaxLength;

        Debug.DrawRay(mCurrentHitObject.transform.position, -mHitVector.normalized * mArmCurrentLenght, Color.yellow);
        Debug.DrawRay(transform.position, stickdir.normalized * mArmCurrentLenght, Color.green);

        Vector3 playervec = mCurrentHitObject.transform.position - gameObject.transform.position;
        float angle = Vector3.Dot(Quaternion.AngleAxis(90, Vector3.forward) * mHitVector.normalized, stickdir.normalized);
        mHitVector = playervec.normalized;

        Vector3 start = transform.position;
        Vector3 checkvector = transform.forward;

        mCurrentHitObject.GetComponent<Rigidbody>().angularVelocity = (Vector3.forward * (angle * 100));

        var hinge = mCurrentHitObject.GetComponent<HingeJoint>();
        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = mFastAnchor.normalized * mArmCurrentLenght;

        transform.localScale = new Vector3(1, 1, mArmCurrentLenght);
        transform.rotation = Quaternion.LookRotation(mHitVector.normalized);
        mHand.position = mPlayer.localPosition + (transform.forward * mArmCurrentLenght);

        if (defeated < 0.2f)
        {
            hinge.breakForce = 0;
            hinge.breakTorque = 0;
            Destroy(mCurrentHitObject);
            mCurrentHitObject = null;
            ishit = false;
        }
    }

    /// <summary>
    /// ２つ刺さった時に物理の動きを止める用の関数
    /// </summary>
    public void StopPhysics()
    {
        if (mCurrentHitObject == null) return;
        var rb = mCurrentHitObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
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
            Debug.DrawLine(start, start + (checkvector * (mArmCurrentLenght - 0.4f)), Color.green);
            if (Physics.CheckCapsule(start, start + (checkvector * (mArmCurrentLenght - 1)), 0.2f, mIgnorelayer))
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
