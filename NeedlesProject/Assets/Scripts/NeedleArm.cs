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

    public float mCentrifugalforce;

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
        next = Vector3.Lerp(transform.forward, next, 0.5f);
        transform.rotation = Quaternion.LookRotation(next.normalized);
        mHand.position = transform.position + (transform.forward * mArmCurrentLenght);
        //壁にハンド部分が刺さったかどうかの判定
        RaycastHit hit;
        if(Physics.Raycast(transform.position,next.normalized,out hit,mArmCurrentLenght+0.4f,mIgnorelayer))
        {
            mHitPoint = hit.point;
            mHitVector = stickdir;
            mPlayer.GetComponent<Rigidbody>().isKinematic = true;
            ishit = true;
        }
    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        mArmCurrentLenght = defeated * mArmMaxLength;

        if (defeated < 0.2f)
        {
            mPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mPlayer.GetComponent<Rigidbody>().isKinematic = false;
            ishit = false;
            return;
        }

        //Debug.DrawRay(mCurrentHitObject.transform.position, -mHitVector.normalized * mArmCurrentLenght, Color.yellow);
        //Debug.DrawRay(mHitPoint, -stickdir.normalized * mArmCurrentLenght, Color.green);

        Vector3 next = -stickdir.normalized;
        int angle = (int)Vector2.Angle(-transform.forward, next.normalized);
        Vector3 start = mHitPoint;
        Vector3 checkvector = -transform.forward;
        //左右のチェック
        float LRCheck = -Mathf.Sign(Vector2Cross(transform.forward, next));
        Debug.DrawLine(start + (checkvector.normalized * 0.5f), start + (checkvector * (mArmCurrentLenght - 0.4f)), Color.green);
        //移動できるか１度ずつ調べてシュミレーションする
        for (int i = 0; angle > i; i++)
        {
            if (Physics.CheckCapsule(start + (checkvector.normalized * 0.5f), start + (checkvector * (mArmCurrentLenght - 0.4f)), 0.2f, mIgnorelayer))
            {
                break;
            }
            next = checkvector;
            checkvector = Quaternion.AngleAxis(LRCheck, Vector3.forward) * checkvector.normalized;
        }
        next = Vector3.Lerp(transform.forward, -next, 0.5f);

        //腕を伸ばすことが出来るか検索
        RaycastHit hit;
        if (Physics.Raycast(mHitPoint, -next.normalized, out hit, mArmCurrentLenght, mIgnorelayer))
        {
            mArmCurrentLenght = Vector3.Distance(mHitPoint, hit.point);
        }


        mPlayer.localPosition = mHitPoint + (-next.normalized * mArmCurrentLenght);
        transform.localScale = new Vector3(1, 1, mArmCurrentLenght);
        transform.rotation = Quaternion.LookRotation(next.normalized);
        mHand.position = transform.position + (transform.forward * mArmCurrentLenght);
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
