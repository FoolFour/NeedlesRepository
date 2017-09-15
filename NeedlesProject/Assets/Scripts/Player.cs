using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アームやインプット情報を管理しプレイヤーと連携するオブジェクトに指示するクラス
/// </summary>
public class Player : MonoBehaviour {

    private PlayerData mData;

    // Use this for initialization
    void Start ()
    {
        mData = GetComponent<PlayerData>();
    }

	// Update is called once per frame
	void Update ()
    {

        if (mData.mRArm.IsHit() && mData.mLArm.IsHit())
        {
            mData.mRArm.StopPhysics();
            mData.mLArm.StopPhysics();
        }

        //左スティック
        float x = Input.GetAxis(GamePad.Horizontal);
        float y = Input.GetAxis(GamePad.Vertical);
        Vector3 dir = new Vector3(x, y, 0);
        if (!mData.mLArm.IsHit()) mData.mLArm.ArmExtend(dir);
        else mData.mLArm.StickArmRotation(dir);

        //右
        float x2 = Input.GetAxis(GamePad.Horizontal2);
        float y2 = Input.GetAxis(GamePad.Vertical2);
        Vector2 dir2 = new Vector3(x2, y2, 0);
        if (!mData.mRArm.IsHit()) mData.mRArm.ArmExtend(dir2);
        else mData.mRArm.StickArmRotation(dir2);

        Vector3 temp = transform.position;
        temp.z = 0;
        transform.position = temp; 
    }
}
