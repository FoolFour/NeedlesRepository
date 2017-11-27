using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : BlockBase, IRespawnMessage
{
    [SerializeField, TooltipAttribute("移動速度")]
    float movespeed;

    [SerializeField, TooltipAttribute("斜め前確認用")]
    public GameObject ray;
    [SerializeField, TooltipAttribute("下確認用")]
    public GameObject ray2;
    [SerializeField, TooltipAttribute("一番後ろの下確認用")]
    public GameObject ray3;
    [SerializeField, TooltipAttribute("前確認")]
    Ray ray4;

    //rayの長さ
    float distance = 1.0f;
    //当たっているならtrue当たってないならfalse
    //前下確認用のray
    public bool ishit;
    //下1確認用のray
    public bool ishit2;
    //下２確認用のray
    public bool ishit3;
    //進行方向の障害物確認
    public bool ishit4;

    //
    Rigidbody rig;
    private RaycastHit hit;

    //回転開始時の角度
    float StartAngle;
    //終了時の角度
    float EndAngle;
    //角度の計算格納
    float Angle_Z;
    //現在の角度
    Vector3 eulerAngles;

    //当たり判定を取りたいLayer
    public LayerMask mask;
    //rayの判定距離
    public float raymater;
    //回転前の角度
    private float PointangleZ;
   
    //デバック確認用
    bool debuglog;

    //初期化用の数値
    //初期位置
    Vector3 FirstPos;
    //初期角度
    Vector3 FirstAngle;

    public enum State
    {
        MOVE,
        ROTATIONM_N,
        ROTATIONM_R,
        IDOL,
        DESTORY
    }
    public State state_;

    void Start()
    {
        eulerAngles =new Vector3 (0,0,0);
        FirstPos = transform.position;
        FirstAngle = transform.eulerAngles;
    }

    void Update()
    {
        if (debuglog == true)
        {
            Debug.Log(state_);
        }

        eulerAngles = gameObject.transform.eulerAngles;
        rig = gameObject.GetComponent<Rigidbody>();
        
        //rayの所得
        ishit = ray.GetComponent<Diagonallybelow>().ishitsecond;
        //rayの所得
        ishit2 = ray2.GetComponent<ForwardRay>().ishitUnder;
        //rayの所得
        ishit3 = ray3.GetComponent<ForwardRay>().ishitUnder;
        //前方の
        ray4 = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray4.origin, ray4.direction * distance, Color.blue);
        ishit4 = Physics.Raycast(ray4, out hit, raymater, mask);
        
        //状態遷移の判定
        if (state_ != State.DESTORY)
        {            
            if (ishit == false && ishit2 == false && ishit3 == false)
            {
                //Debug.Log("下向きに回転");
                state_ = State.ROTATIONM_N;
            }
            //上向きに回転
            if (ishit == true && ishit2 == true && ishit3 == true && ishit4 == true)
            {              
                state_ = State.ROTATIONM_R;
            }
            //移動かいし
            if ((ishit == true || ishit2 == true)&& state_ == State.IDOL)
            {
                state_ = State.MOVE;
            }
        }

        //移動中
        if (state_ == State.MOVE)
        {
            //使わないところの固定
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ
                            | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

            StartAngle = Mathf.Floor( transform.eulerAngles.z);

            gameObject.transform.position +=transform.right * movespeed * Time.deltaTime;
        }
        //回転
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
        //回転
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

        if (state_==State.DESTORY)
        {
            GetComponent<RemoveComponent>().SwitchActive(false);
        }
    }
    
    //リスポーン
    void respown()
    {
        gameObject.transform.position = FirstPos;
        gameObject.transform.eulerAngles = FirstAngle;
        GetComponent<RemoveComponent>().SwitchActive(true);
        state_ = State.IDOL;
    }

    //プレイヤーと当たった場合
    public override void StickEnter(GameObject arm)
    {
        //Debug.Log("敵に当たった");
        //Destroy(gameObject);
        //state_ = State.DESTORY;
        base.StickEnter(arm);
    }

  

    //プレイヤー本体に当たると死亡する
    //アーム（先端）とアームを当てるとプレイヤーがスタンする
    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag ==  "PlayerArm"||
            collision.gameObject.name==  "HandVesselLeft"||
            collision.gameObject.name == "HandVesselRight")
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


        if (collision.gameObject.tag == "Player")
        {
            state_ = State.DESTORY;
        }
    }

    public void RespawnInit()
    {
        respown();
    }
}