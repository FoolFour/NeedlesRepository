using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アームやインプット情報を管理しプレイヤーと連携するオブジェクトに指示するクラス
/// </summary>
public class Player : MonoBehaviour {

    public float mStanTime = 1;
    public GameObject mStanEffect;

    private PlayerData mData;
    private bool mStan = false;
    private float mStanTimer = 0;

    int mIgnorelayer = 1 << 9; //ブロックのみ当たる

    // Use this for initialization
    void Start ()
    {
        mData = GetComponent<PlayerData>();
    }

    void Update()
    {
        if (mStan)
        {
            mStanTimer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(mStan)
        {
            mData.mLArm.StanMode();
            mData.mRArm.StanMode();
            if (mStanTimer > mStanTime)
            {
                mStan = false;
                mStanTimer = 0;
            }
        }

        //左スティック
        float x = Input.GetAxis(GamePad.Horizontal);
        float y = Input.GetAxis(GamePad.Vertical);
        Vector3 dir = new Vector3(x, y, 0);
        if (!mData.mLArm.IsHit()){ mData.mLArm.ArmExtend(dir);}
        else{ mData.mLArm.StickArmRotation(dir);}

        //右
        float x2 = Input.GetAxis(GamePad.Horizontal2);
        float y2 = Input.GetAxis(GamePad.Vertical2);
        Vector2 dir2 = new Vector3(x2, y2, 0);
        if (!mData.mRArm.IsHit()){mData.mRArm.ArmExtend(dir2);}
        else{mData.mRArm.StickArmRotation(dir2);}
    }

    public void StanMode(Vector3 velocity)
    {
        Debug.Log("Stan");
        Instantiate(mStanEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
        mStan = true;
        mData.mrb.velocity = Vector3.zero;
        mData.mrb.AddForce(velocity, ForceMode.VelocityChange);
    }

    public bool HitCheck()
    {
        return mData.mLArm.IsHit() || mData.mRArm.IsHit();
    }

    public bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 1, mIgnorelayer))
        {
            return true;
        }
        return false;
    }

    public void Dead()
    {
        mData.mLArm.Dead();
        mData.mRArm.Dead();
        mStan = false;
        mStanTimer = 0;
    }

    public void Stop()
    {
        this.enabled = false;
    }
}
