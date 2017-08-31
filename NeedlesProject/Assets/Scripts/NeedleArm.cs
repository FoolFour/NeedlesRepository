using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの腕クラス
/// </summary>
public class NeedleArm : MonoBehaviour
{

    public Color mDebugColor;
    [SerializeField, TooltipAttribute("腕の長さ")]
    public float mArmLength;

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
    /// 伸び縮みする腕のモデル
    /// </summary>
    //public Transform mBarModel;


    /// <summary>
    /// 腕を伸ばす処理
    /// </summary>
    /// <param name="stickdir"></param>
    public void ArmExtend(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        float len = defeated * mArmLength;

        Debug.DrawRay(transform.position, stickdir.normalized * len, mDebugColor);
        //mBarModel.localPosition = transform.position;
        //mBarModel.localRotation = Quaternion.LookRotation(stickdir.normalized);
        //mBarModel.transform.localScale = new Vector3(1, 1, len);

        RaycastHit hit;
        int layerMask = ~(1 << 8);
        if (Physics.Raycast(transform.position, stickdir.normalized, out hit, len,layerMask))
        {
            mCurrentHitObject = (GameObject)Instantiate(mHitObjectPrefab, hit.point, Quaternion.identity);

            var hinge = mCurrentHitObject.GetComponent<HingeJoint>();
            hinge.connectedBody = gameObject.GetComponent<Rigidbody>();
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
        float len = defeated * mArmLength;


        //mBarModel.localRotation = Quaternion.LookRotation(mHitVector.normalized);
        //mBarModel.transform.localScale = new Vector3(1, 1, len);

        Debug.DrawRay(mCurrentHitObject.transform.position, -mHitVector.normalized * len, Color.yellow);
        Debug.DrawRay(transform.position, stickdir.normalized * len, Color.green);

        Vector3 playervec = mCurrentHitObject.transform.position - gameObject.transform.position;
        float angle = Vector3.Dot(Quaternion.AngleAxis(90, Vector3.forward) * mHitVector.normalized, stickdir.normalized);
        mHitVector = playervec.normalized;

        Debug.Log(mCurrentHitObject);
        mCurrentHitObject.GetComponent<Rigidbody>().angularVelocity = (Vector3.forward * (angle * 100));

        var hinge = mCurrentHitObject.GetComponent<HingeJoint>();
        hinge.autoConfigureConnectedAnchor = false;
        hinge.connectedAnchor = mFastAnchor.normalized * len;

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
}
