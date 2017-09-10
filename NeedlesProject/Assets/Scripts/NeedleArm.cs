﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの腕クラス
/// </summary>
public class NeedleArm : MonoBehaviour
{
    //デバッグ用変数　後で消す
    public GameObject debugobj;
    public GameObject[] debugpoint;
    //-------------------------

    public Color mDebugColor;
    [SerializeField, TooltipAttribute("腕の長さ")]
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
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        mArmCurrentLenght = defeated * mArmMaxLength;

        if (stickdir != Vector3.zero) mArmDirection = stickdir;

        Debug.DrawRay(transform.position, mArmDirection.normalized * mArmCurrentLenght, mDebugColor);

        Vector3 next = ArmRotateColision(mArmDirection);

        transform.transform.localScale = new Vector3(1, 1, mArmCurrentLenght);

        if(!Input.GetKey(KeyCode.C)) transform.localRotation = Quaternion.LookRotation(next.normalized);
    }

    //刺さった腕を回転する
    public void StickArmRotation(Vector3 stickdir)
    {
        float defeated = Mathf.Min(1.0f, (Mathf.Abs(stickdir.x) + Mathf.Abs(stickdir.y)));
        float len = defeated * mArmMaxLength;

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

    /// <summary>
    /// 腕の回転時の押し出し処理
    /// </summary>
    /// <param name="next"></param>
    Vector3 ArmRotateColision(Vector3 next)
    {
        //当たり判定１　入力先に障害物があった時の押し出し処理
        Vector3 start = transform.position;
        Vector3 end = transform.position + (next.normalized * (mArmCurrentLenght - 1));
        if (Physics.CheckCapsule(start, end, 0.2f, mIgnorelayer))
        {
            //１度づつ回転して当たらなくなるまで回転してテスト
            Vector3 checkvector = next.normalized;
            float angle = -Mathf.Sign(Vector2Cross(transform.forward, next));
            for (int i = 0; i < 180; i++)
            {
                checkvector = Quaternion.AngleAxis(angle, Vector3.forward) * checkvector.normalized;
                Debug.DrawLine(start, start + (checkvector * (mArmCurrentLenght - 1)), Color.green);
                if (!Physics.CheckCapsule(start, start + (checkvector * (mArmCurrentLenght - 1)), 0.2f, mIgnorelayer))
                {
                    return checkvector;
                }
            }
        }
        else
        {
            var coliders = Physics.OverlapSphere(transform.position, mArmCurrentLenght, mIgnorelayer);
            int debugi = 0;
            foreach (var c in coliders)
            {
                //デブッグ
                gameObject.GetComponent<CapsuleCollider>();
                Vector3 point = transform.position;
                Vector3 closetpoint = c.ClosestPoint(point);
                debugpoint[debugi].transform.position = c.transform.position;
                debugi++;
                //--------------------------------------------------------

                if (sector_hit(c, next))
                {
                    //Debug.Log(c.name + "HIT");
                    //return transform.forward;
                }
            }
        }
        return next;
    }

    //扇の当たり判定を行い移動ルートに障害物が無いか検索
    bool sector_hit(Collider colider, Vector3 next)
    {
        Vector3 point = transform.position;
        Vector3 closetpoint = colider.transform.position;
        //Vector3 closetpoint = colider.transform.position;
        if (Vector2Cross(transform.forward, next.normalized) < 0)
        {
            float angle = Vector2Cross(transform.position + transform.forward, closetpoint);
            Debug.Log("フォワード側"+ angle);
            if (angle > 0)// return false;
            angle = Vector2Cross(transform.position + next.normalized, closetpoint);
            Debug.Log("スティック側"+ angle);
            if (angle < 0) return false;
            return true;
        }
        //else
        //{
        //    float angle = Vector2Cross(transform.position + transform.forward, closetpoint);
        //    if (angle < 0) return false;
        //    angle = Vector2Cross(transform.position + next.normalized, closetpoint);
        //    if (angle > 0) return false;
        //}
        return false;
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
