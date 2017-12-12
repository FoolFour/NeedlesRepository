using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アームやインプット情報を管理しプレイヤーと連携するオブジェクトに指示するクラス
/// </summary>
public class Player : MonoBehaviour {

    public float mStanTime = 1;
    public float mMaxSpeed = 100;

    private PlayerData mData;
    private bool mStan = false;
    private float mStanTimer = 0;

    private bool mWait = false;

    int mIgnorelayer = 1 << 9; //ブロックのみ当たる
    private bool isDead = false;

    [SerializeField, Tooltip("プレイヤーが志望した時のパーティクル")]
    public GameObject m_deadParticle;
    private GameObject m_currentDeadEffect;

    // Use this for initialization
    void Start()
    {
        mData = GetComponent<PlayerData>();
        m_currentDeadEffect = (GameObject)Instantiate(m_deadParticle, transform.position, Quaternion.identity);
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
        mData.mrb.isKinematic = isDead;
        if (GameManagers.Instance.GameStateManager.GetCurrentGameState() != GameState.Play) return;

        if (mStan)
        {
            if (mWait) Flash();
            mData.mLArm.Return_Arm();
            mData.mRArm.Return_Arm();
            if (mStanTimer > mStanTime)
            {
                mStan = false;
                mStanTimer = 0;

                if (mWait) FlashEnd();
                mWait = false;
            }
            return;
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

        MaxSpeed();
    }

    /// <summary>
    /// スタンさせる
    /// </summary>
    /// <param name="velocity"></param>
    public void StanMode(Vector3 velocity)
    {
        Debug.Log("Stan");
        mStan = true;
        mData.mrb.velocity = Vector3.zero;
        mData.mrb.AddForce(velocity, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 腕のどっちかが当たっているか判定
    /// </summary>
    /// <returns></returns>
    public bool HitCheck()
    {
        return mData.mLArm.IsHit() || mData.mRArm.IsHit();
    }

    /// <summary>
    /// 地面に設置している
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        int layerMask = 1 << 9 | 1 << 11;
        if (Physics.Raycast(ray, 1, layerMask,QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 志望時の処理
    /// </summary>
    public void Dead()
    {
        mData.mLArm.Dead();
        mData.mRArm.Dead();
        mData.mrb.velocity = Vector3.zero;
        mStan = true;
        mWait = true;
        mStanTimer = 1;
    }

    /// <summary>
    /// Goal時の処理
    /// 止まる
    /// </summary>
    public void Goal()
    {
        mData.mLArm.Goal();
        mData.mRArm.Goal();
        mStan = false;
        mStanTimer = 0;
        this.enabled = false;
    }

    /// <summary>
    /// 点滅処理
    /// </summary>
    public void Flash()
    {
        var mrs = transform.GetComponentsInChildren<MeshRenderer>();
        foreach(var mr in mrs)
        {
            mr.enabled = !mr.enabled;
        }
    }
    /// <summary>
    /// 点滅終了処理
    /// </summary>
    public void FlashEnd()
    {
        var mrs = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in mrs)
        {
            mr.enabled = true;
        }
    }
    /// <summary>
    /// スタンしているか
    /// waitが真の時は違う
    /// </summary>
    /// <returns></returns>
    public bool isStan()
    {
        return mStan && !mWait;
    }

    /// <summary>
    ///物理の速度抑制処理 
    /// </summary>
    private void MaxSpeed()
    {
        var temp = mData.mrb.velocity;
        temp.x = Mathf.Clamp(temp.x, -mMaxSpeed,mMaxSpeed);
        temp.y = Mathf.Clamp(temp.y, -mMaxSpeed, mMaxSpeed);
        temp.z = Mathf.Clamp(temp.z, -mMaxSpeed, mMaxSpeed);
        mData.mrb.velocity = temp;

        mData.mLArm.MaxSpeed(mMaxSpeed);
        mData.mRArm.MaxSpeed(mMaxSpeed);
    }

    public void ExplosionEffect()
    {
        m_currentDeadEffect.transform.position = transform.position;
        m_currentDeadEffect.GetComponent<PLayerDead_effect>().ParticleStart();
        Sound.PlaySe("Explosion");
    }

    public void SwitchColliderandRender(bool enable)
    {
        GetComponent<RemoveComponent>().SwitchActive(enable);
        isDead = !enable;
    }
}
