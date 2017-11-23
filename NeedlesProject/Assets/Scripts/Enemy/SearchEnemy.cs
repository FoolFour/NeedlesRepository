using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemy : BlockBase
{
    /*プレイヤーを追いかける敵*/

    //プレイヤーの位置    
    Vector3 P_pos;
    //プレイヤーの見失った位置  
    Vector3 Lostpos;

    //敵の位置
    public Vector3 E_pos;

    //初期生成位置
    public Vector3 StartPosition;

    //プレイヤーまでの距離
    public float P_distance;
    //プレイヤーのいる方向
    Vector3 P_distanceV;

    //索敵範囲
    public float r;

    //プレイヤーまでに障害物があるか
    public Ray ray;

    //Blockに当たりそうか
    public GameObject ray2;
    public GameObject ray3;
    //
    public bool ishit2;
    public bool ishit3;

    //
    public float RayMeter;

    //
    LayerMask mask;

    //オブジェクトが当たっているか
    public RaycastHit ishit;
    //当たっているオブジェクトの名前
    public string hit_tag;

    //プレイヤーを見失ってからの時間
    public float LostTime;
    //初期位置に戻るまでの時間
    public int SearchlimitTime;

    //移動速度
    public float move;

    //自身の状態
    public enum State
    {
        //索敵
        STEAT_SEARCH,
        //追尾
        STEAT_CHASE,
        //警戒
        STEAT_WARNING,
        //帰還
        STEAT_RETURN,
        //待機状態
        STEAT_IDLE
    }

    public State state;
    void Start()
    {
        state = State.STEAT_SEARCH;
        LostTime = 0;
        StartPosition = gameObject.transform.position;
    }

    void Update()
    {
        //プレイヤーの位置
        P_pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().gameObject.transform.position;
        P_pos = P_pos + new Vector3(0, 0.5f, 0);

        //敵の位置
        E_pos = gameObject.transform.position;

        //プレイヤーとの距離
        P_distance = Vector2.Distance(E_pos, P_pos);
        //プレイヤーがいる方向
        P_distanceV = (P_pos - E_pos).normalized;
        //プレイヤーまでに障害物があるか
        ray = new Ray(transform.position, P_distanceV);

        //表示
        Debug.DrawRay(ray.origin, ray.direction * r, Color.red);
        Debug.DrawRay(ray.origin, Lostpos, Color.blue);

        //
        ishit2 = ray2.GetComponent<ForwardRay>().ishitUnder;
        ishit3 = ray3.GetComponent<ForwardRay>().ishitUnder;

        //何かに当たっているなら名前を所得
        if (Physics.Raycast(ray, out ishit) == true)
        {
            hit_tag = ishit.collider.gameObject.tag;
        }

        //プレイヤーを見つけたら
        if (P_distance <= r && hit_tag == "Player")
        {
            state = State.STEAT_CHASE;
        }

        /*状態遷移*/
        //索敵
        if (state == State.STEAT_SEARCH)
        {
            Search();
        }
        else if (state == State.STEAT_CHASE)  //追跡
        {
            Chase();
        }
        else if (state == State.STEAT_WARNING) //警戒
        {
            Warning();
        }
        else if (state == State.STEAT_RETURN)
        {
            returnStartPosition();
        }


        if (state == State.STEAT_CHASE || state == State.STEAT_WARNING)
        {
            if (E_pos != P_pos)
            {
                gameObject.transform.position += gameObject.transform.forward * move * Time.deltaTime;
                if (ishit2 == true && ishit3 == true)
                {
                    //両方当たっている場合
                }
                else if (ishit2 == true)
                {
                    //Debug.Log("上にブロックがある");
                    gameObject.transform.position += new Vector3(0, -2, 0) * Time.deltaTime;
                }
                else if (ishit3 == true)
                {
                    //Debug.Log("下にブロックがある");
                    gameObject.transform.position += new Vector3(0, 2, 0) * Time.deltaTime;
                    //gameObject.GetComponent<Rigidbody>().AddForce( gameObject.transform.up*100.0f, ForceMode.VelocityChange);
                }
                else if (ishit2 == false && ishit3 == false)
                {
                    gameObject.transform.position += gameObject.transform.forward * move * Time.deltaTime;
                }
            }
        }

    }

    //プレイヤーを索敵
    void Search()
    {
        LostTime += Time.deltaTime;

        //時間が来たら初期位置に戻る
        if (LostTime >= SearchlimitTime)
        {
            state = State.STEAT_RETURN;
        }
    }

    //プレイヤーの追跡
    void Chase()
    {
        LostTime = 0;
        gameObject.transform.LookAt(P_pos);

        //追いかける
        if (hit_tag != "Player" || hit_tag != "PlayerArm")
        {
            Lostpos = ishit.point;
            state = State.STEAT_WARNING;
        }
    }
    //プレイヤーを見失った
    void Warning()
    {
        LostTime = 0;
        gameObject.transform.LookAt(Lostpos);

        //見失った場所まで行く
        if (E_pos != Lostpos)
        {
            gameObject.transform.position += gameObject.transform.forward * move * Time.deltaTime;
        }

        if (E_pos.x + 0.5f >= Lostpos.x && E_pos.x - 0.5f <= Lostpos.x)
        {
            if (E_pos.y + 0.5f >= Lostpos.y && E_pos.y - 0.5f <= Lostpos.y)
            {
                state = State.STEAT_SEARCH;
            }
        }
    }

    /*生成位置に帰る*/
    //カメラの範囲外に出たとき
    //プレイヤーが逃げ切った時
    void returnStartPosition()
    {
        LostTime = 0;
        transform.LookAt(StartPosition);

        if (E_pos != StartPosition)
        {
            gameObject.transform.position += gameObject.transform.forward * move * Time.deltaTime;
        }

        if (E_pos.x + 1.5f >=StartPosition.x && E_pos.x - 1.5f <= StartPosition.x)
        {
            if (E_pos.y + 1.5f >=StartPosition.y && E_pos.y - 1.5f <= StartPosition.y)
            {
                state = State.STEAT_IDLE;
            }
        }

    }

    void Idol()
    {
        LostTime = 0;
    }

    //--------------------------------------------------------------------
    //プレイヤーと当たった場合
    public override void StickEnter(GameObject arm)
    {
        Debug.Log("敵に当たった");
        Destroy(gameObject);
        base.StickEnter(arm);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

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
}