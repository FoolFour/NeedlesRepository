using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : BlockBase
{
    //移動速度
    public float movespeed;
    //斜め前と後ろ確認用
    Ray ray;
    Ray ray2;
    //前と後ろ確認用
    Ray ray3;

    //rayの長さ
    float distance = 1.0f;
    //当たっているならtrue当たってないならfalse
    public bool ishit;
    public bool ishit2;
    public bool ishit3;

    float StartAngle;
    float EndAngle;
    float Angle_Z;
    //
    Vector3 eulerAngles;
    //
    public LayerMask mask;
    //回転前の角度
    private float PointangleZ;  
    //
    Rigidbody rig;

     enum State
    {
        MOVE,
        ROTATIONM,
        IDOL
    }
    State state_;

    void Start()
    {
    }

    void Update()
    {
        eulerAngles = gameObject.transform.eulerAngles;
        rig = gameObject.GetComponent<Rigidbody>();
        //
       

        //前下確認用のray
        ishit = GameObject.Find("DiagonallRay").GetComponent<Diagonallybelow>().ishitsecond;
        //ray = new Ray(transform.position, transform.right + new Vector3(0, -1, 0));
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        //ishit = Physics.Raycast(ray, out hit, ray_Meter, mask);

        //下確認用のray
        ishit2 = GameObject.Find("UnderRay").GetComponent<ForwardRay>().ishitUnder;
        //ray2 = new Ray(transform.position+new Vector3(-0.5f,0,0), -transform.up);
        //Debug.DrawRay(ray2.origin, ray2.direction * distance, Color.red);
        //ishit2 = Physics.Raycast(ray2, out hit, ray_Meter, mask);

        //前方確認用のray
        //ray3 = new Ray(transform.position, transform.right);
        //Debug.DrawRay(ray3.origin, ray3.direction * distance, Color.red);
        ishit3 = GameObject.Find("CenterRay").GetComponent<ForwardRay>().ishitUnder;

        //状態の判定
        if (ishit == false && ishit2 == false&&ishit3==false)
        {          
            state_ = State.ROTATIONM;     
        }
        else if (ishit == true && state_ == State.IDOL )
        {           
            state_ = State.MOVE; 
        }  

        //移動中
        if (state_ == State.MOVE)
        {
            rig.constraints = RigidbodyConstraints.FreezeRotationX| RigidbodyConstraints.FreezeRotationZ 
                            | RigidbodyConstraints.FreezeRotationY| RigidbodyConstraints.FreezePositionZ;
            StartAngle = transform.eulerAngles.z;
            EndAngle = StartAngle - 90f;           
            gameObject.transform.position += transform.right * movespeed * Time.deltaTime;            
        }
      
        else if (state_ == State.ROTATIONM)
        {
            rig.constraints = RigidbodyConstraints.None;

            Angle_Z = Mathf.LerpAngle(StartAngle, EndAngle,300f);
            transform.eulerAngles = new Vector3(0, 0, Angle_Z);

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
        if (collision.gameObject.tag == "Player")
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
}