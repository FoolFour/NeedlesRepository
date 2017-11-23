using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : BlockBase,IRespawnMessage
{
    //移動速度
    public float movespeed;
    //斜め前と後ろ確認用
    public GameObject ray;
    public GameObject ray2;
    //前と後ろ確認用
    public GameObject ray3;
    Ray ray4;

    //rayの長さ
    float distance = 1.0f;
    //当たっているならtrue当たってないならfalse
    public bool ishit;
    public bool ishit2;
    public bool ishit3;
    public bool ishit4;

    public float StartAngle;
    public float EndAngle;
    public float Angle_Z;
    //
    Vector3 eulerAngles;
    //
    public LayerMask mask;
    //回転前の角度
    private float PointangleZ;
    //
    Rigidbody rig;
    private RaycastHit hit;
    //
    public bool debuglog;

    public enum State
    {
        MOVE,
        ROTATIONM_N,
        ROTATIONM_R,
        IDOL
    }
    public State state_;

    void Start()
    {
        eulerAngles =new Vector3 (0,0,0);
        
    }

    void Update()
    {
        if (debuglog == true)
        {
            Debug.Log(state_);
        }

        eulerAngles = gameObject.transform.eulerAngles;
        rig = gameObject.GetComponent<Rigidbody>();

        //前下確認用のray
        ishit = ray.GetComponent<Diagonallybelow>().ishitsecond;
        //ishit = Physics.Raycast(ray, out hit, ray_Meter, mask);

        //下確認用のray
        ishit2 = ray2.GetComponent<ForwardRay>().ishitUnder;
        //ishit2 = Physics.Raycast(ray2, out hit, ray_Meter, mask);

        //前方確認用のray
        //ray3 = new Ray(transform.position, transform.right);      
        ishit3 = ray3.GetComponent<ForwardRay>().ishitUnder;

        //進行方向の障害物確認
        ray4 = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray4.origin, ray4.direction * distance, Color.blue);
        ishit4 = Physics.Raycast(ray4, out hit, 0.5f, mask);

        //状態の判定
        if (ishit == false && ishit2 == false && ishit3 == false)
        {
            state_ = State.ROTATIONM_N;
        }
        else if (ishit == true && ishit2 == true && ishit3 == true && ishit4 == true)
        {
            state_ = State.ROTATIONM_R;
        }
        else if ((ishit == true || ishit2 == true) && state_ == State.IDOL)
        {
            state_ = State.MOVE;
        }

        //移動中
        if (state_ == State.MOVE)
        {
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ
                            | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            StartAngle = Mathf.Floor( transform.eulerAngles.z);     
            gameObject.transform.position +=transform.right * movespeed * Time.deltaTime;
        }

        else if (state_ == State.ROTATIONM_N)
        {
            rig.constraints = RigidbodyConstraints.None;

            EndAngle = StartAngle - 90f;

            Angle_Z = Mathf.LerpAngle(StartAngle, EndAngle, 60f);

            transform.eulerAngles = new Vector3(0, 0, Angle_Z);

            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ
                           | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

            state_ = State.IDOL;
        }
        else if (state_ == State.ROTATIONM_R)
        {

            rig.constraints = RigidbodyConstraints.None;

            EndAngle = StartAngle + 90f;

            Angle_Z = Mathf.LerpAngle(StartAngle, EndAngle, 60f);

            transform.eulerAngles = new Vector3(0, 0, Angle_Z);

            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ
                            | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

            state_ = State.IDOL;
        }

    }
    //プレイヤーと当たった場合
    public override void StickEnter(GameObject arm)
    {
        Debug.Log("敵に当たった");
        Destroy(gameObject);
        base.StickEnter(arm);
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "PlayerArm")
        {
            Vector3 temp = collision.gameObject.transform.position - transform.position;
            temp.y = 1;
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().StanMode(temp.normalized * 10);
            }
            else
            {
                collision.gameObject.transform.parent.GetComponent<Player>().StanMode(temp.normalized * 10);
            }
        }
    }

    public void RespawnInit()
    {
#warning 初期化処理書いたら消していいぞ
        //Debug.Log("初期化処理書けこの野郎");
    }
}