using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEnemy : BlockBase
{
    //移動速度
    public float movespeed;
    //移動範囲
    public float min;
    public float max;
    //
    float movenum;

    //public LayerMask mask;

    //向き
    public enum LR
    {
        LEFT,
        RIGHT
    }
    public LR leftright;
    void Start()
    {
        leftright = LR.RIGHT;
    }

    void Update()
    {
        //はしこっまで行ったら逆に移動
        if (movenum >= max)
        {
            leftright = LR.RIGHT;
        }
        else if(movenum <= min)
        {
            leftright = LR.LEFT;
        }

        if (leftright==LR.LEFT)
        {
            movenum += movespeed * Time.deltaTime;
            gameObject.transform.position += transform.right * movespeed * Time.deltaTime;
        }
        else
        {
            movenum -=movespeed * Time.deltaTime;
            gameObject.transform.position -= transform.right * movespeed * Time.deltaTime;
        }

    }

    public override void StickEnter(GameObject arm)
    {
        Debug.Log("敵に当たった");
        Destroy(gameObject);
        base.StickEnter(arm);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
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