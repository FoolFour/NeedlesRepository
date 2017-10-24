using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatenemy : BlockBase
{
    //移動速度
    public float movespeed;
    //移動範囲
    public float min;
    public float max;
    //
    float movenum;

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
        base.StickEnter(arm);
    }
}