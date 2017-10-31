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

    public LayerMask mask;

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
        if (movenum >= 3)
        {
            leftright = LR.RIGHT;
        }
        else if(movenum <= -3)
        {
            leftright = LR.LEFT;
        }

        if (leftright==LR.LEFT)
        {
            movenum += 0.1f * Time.deltaTime;
            gameObject.transform.position += transform.right * movespeed * Time.deltaTime;
        }
        else
        {
            movenum -= 0.1f * Time.deltaTime;
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
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("敵にあたった");
            Vector3 temp = collision.gameObject.transform.position - transform.position;
            temp.y = 1;
            collision.gameObject.GetComponent<Player>().StanMode(temp.normalized * 10);
        }
    }
}