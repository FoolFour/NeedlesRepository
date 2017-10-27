using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomaleemy : BlockBase
{
    //移動速度
    public float movespeed;
    //
    string undername;
    //
    Ray ray;
    Ray ray2;
    //
    RaycastHit hit;
    //
    float distance = 1.0f;
    //
    bool ishit;
    bool ishit2;
    public Vector3 eulerAngles;

    public enum rotation
    {
        REVERSE01,
        REVERSE02,
        MOVE,
    }
    public rotation rotation_;

    void Start()
    {

    }

    void Update()
    {
        eulerAngles = gameObject.transform.eulerAngles;

        ray = new Ray(transform.position, new Vector3(1, -1, 0));
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        ishit = Physics.Raycast(ray, out hit, Mathf.Infinity);

        ray2 = new Ray(transform.position, new Vector3(-1, -1, 0));
        Debug.DrawRay(ray2.origin, ray2.direction * distance, Color.red);
        ishit2 = Physics.Raycast(ray2, out hit, Mathf.Infinity);

        //Debug.Log(hit.collider.gameObject.name);

        //移動中
        if (rotation_ == rotation.MOVE)
        {
            gameObject.transform.position +=transform.right * movespeed * Time.deltaTime;
        }

        //ブロックがなければ
        if (ishit == false)
        {
            if (eulerAngles.y >= 0)
            {
                Debug.Log("yが0の時");
                rotation_ = rotation.REVERSE01;
            }
        }

        if (ishit2 == false)
        {
            if (eulerAngles.y >= 180)
            {
                Debug.Log("yが180の時");
                rotation_ = rotation.REVERSE02;
            }
        }

        //
        if (rotation_ == rotation.REVERSE01)
        {
            Debug.Log("回転中");

            float angle = Mathf.LerpAngle(0, 180, 10.0f);
            transform.eulerAngles = new Vector3(0, angle, 0);
            if (eulerAngles.y == 180)
            {
                rotation_ = rotation.MOVE;
            }
        }

        if (rotation_ == rotation.REVERSE02)
        {
            float angle = Mathf.LerpAngle(0, 0, 10.0f);
            transform.eulerAngles = new Vector3(0, angle, 0);
            if (eulerAngles.y == 0)
            {
                rotation_ = rotation.MOVE;
            }
        }

    }

    public override void StickEnter(GameObject arm)
    {
        Debug.Log("敵に当たった");
        base.StickEnter(arm);
    }
}