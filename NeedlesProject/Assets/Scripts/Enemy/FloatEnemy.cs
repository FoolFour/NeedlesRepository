using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEnemy : BlockBase , IRespawnMessage
{
    //移動速度
    public float movespeed;
    //移動範囲
    public float distance;
    //移動量
    float movenum;
    //public LayerMask mask;
    //初期位置
    Vector3 startPosition;
    //現在位置
    Vector3 currentposition;

    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        RETURN,
        DESTORY
    }

    public Direction direction_;

    /*初期化用の数値*/
    //初期位置
    Vector3 FirstPos;
    //初期角度
    Vector3 FirstAngle;
    //移動方向
    Direction firstdirection;

    void Start()
    {
        startPosition = gameObject.transform.localPosition;
        FirstPos = transform.position;
        FirstAngle = transform.eulerAngles;
        firstdirection = direction_;
    }

    void Update()
    {
        //現在地の更新
        currentposition = gameObject.transform.localPosition;

        if (direction_ == Direction.LEFT)
        {
            movenum += movespeed * Time.deltaTime;
            gameObject.transform.position -= transform.right * movespeed * Time.deltaTime;
            //指定された位置まで行ったら
            if (distance <= movenum)
            {
                direction_ = Direction.RETURN;
            }
        }
        else if (direction_ == Direction.RIGHT)
        {
            movenum += movespeed * Time.deltaTime;
            gameObject.transform.position += transform.right * movespeed * Time.deltaTime;
            //指定された位置まで行ったら
            if (distance <= movenum)
            {
                direction_ = Direction.RETURN;
            }
        }
        else if (direction_ == Direction.UP)
        {
            movenum += movespeed * Time.deltaTime;
            gameObject.transform.position += transform.up * movespeed * Time.deltaTime;
            //指定された位置まで行ったら
            if (distance <= movenum)
            {
                direction_ = Direction.RETURN;
            }
        }
        else if (direction_ == Direction.DOWN)
        {
            movenum += movespeed * Time.deltaTime;
            gameObject.transform.position -= transform.up * movespeed * Time.deltaTime;
            //指定された位置まで行ったら
            if (distance <= movenum)
            {
                direction_ = Direction.RETURN;
            }
        }

        //逆向きに移動
        if (direction_ == Direction.RETURN)
        {
            //左
            if (currentposition.x < startPosition.x)
            {
                //Debug.Log("左側");
                movenum -= movespeed * Time.deltaTime;
                gameObject.transform.position += transform.right * movespeed * Time.deltaTime;

                if (0 >= movenum)
                {
                    direction_ = Direction.LEFT;
                }
            }
            //右
            else if (currentposition.x > startPosition.x)
            {
                //Debug.Log("右側");
                movenum -= movespeed * Time.deltaTime;
                gameObject.transform.position -= transform.right * movespeed * Time.deltaTime;
                if (0 >= movenum)
                {
                    direction_ = Direction.RIGHT;
                }
            }
            //上
            else if (currentposition.y > startPosition.y)
            {
                //Debug.Log("上側");
                movenum -= movespeed * Time.deltaTime;
                gameObject.transform.position -= transform.up * movespeed * Time.deltaTime;

                if (0 >= movenum)
                {
                    direction_ = Direction.UP;
                }
            }
            //下
            else if (currentposition.y < startPosition.y)
            {
                //Debug.Log("下側");
                movenum -= movespeed * Time.deltaTime;
                gameObject.transform.position += transform.up * movespeed * Time.deltaTime;
                if (0 >= movenum)
                {
                    direction_ = Direction.DOWN;
                }
            }
        }
    }

    void Stop()
    {
       GetComponent<RemoveComponent>().SwitchActive(false);
        direction_ = Direction.DESTORY;
    }

    void respown()
    {
        gameObject.transform.position = FirstPos;
        gameObject.transform.eulerAngles = FirstAngle;
        GetComponent<RemoveComponent>().SwitchActive(true);
        direction_ = firstdirection;
    }

    public override void StickEnter(GameObject arm)
    {
        //Debug.Log("敵に当たった");
        //Destroy(gameObject);
        base.StickEnter(arm);
        Stop();
    }

    //アーム（先端）に当たると死亡する
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Player"||
            collision.gameObject.tag == "PlayerArm")
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
        respown();
    }
}