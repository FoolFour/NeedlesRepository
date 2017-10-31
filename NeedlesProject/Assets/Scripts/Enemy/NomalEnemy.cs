using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemy : BlockBase
{
    //移動速度
    public float movespeed;
    //
    string undername;
    //斜め前と後ろ確認用
    Ray ray;
    Ray ray2;
    //前と後ろ確認用
    Ray ray3;
    Ray ray4;
    //
    RaycastHit hit;
    //rayの長さ
    float distance = 2.0f;
    //当たっているならtrue当たってないならfalse
    bool ishit;
    bool ishit2;
    bool ishit3;
    bool ishit4;
    //
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
        //前下確認用のray
        ray = new Ray(transform.position, new Vector3(1, -1, 0));
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        ishit = Physics.Raycast(ray, out hit, Mathf.Infinity);

        //後ろ下確認用のray
        ray2 = new Ray(transform.position, new Vector3(-1, -1, 0));
        Debug.DrawRay(ray2.origin, ray2.direction * distance, Color.red);
        ishit2 = Physics.Raycast(ray2, out hit, Mathf.Infinity);

        //前方確認用のray
        ray3 = new Ray(transform.position, new Vector3(1, 0, 0));
        Debug.DrawRay(ray3.origin, ray3.direction * distance, Color.red);
        ishit3 = Physics.Raycast(ray3, out hit, Mathf.Infinity);

        //後方確認用のray
        ray4 = new Ray(transform.position, new Vector3(-1, 0, 0));
        Debug.DrawRay(ray4.origin, ray4.direction * distance, Color.red);
        ishit4 = Physics.Raycast(ray4, out hit, Mathf.Infinity);

        //移動中
        if (rotation_ == rotation.MOVE)
        {
            gameObject.transform.position += transform.right * movespeed * Time.deltaTime;
        }

        //ブロックがなければ
        if (ishit == false)
        {
            Debug.Log("前下:yが0の時");
            rotation_ = rotation.REVERSE01;
        }
        //else if (ishit3 == true)
        //{
        //    Debug.Log("前:yが0の時");
        //    rotation_ = rotation.REVERSE01;
        //}


        //ブロックがなければ
        if (ishit2 == false)
        {
            Debug.Log("後ろ下:yが180の時");
            rotation_ = rotation.REVERSE02;
        }
        //else if (ishit4 == true)
        //{
        //    Debug.Log("後ろ:yが180の時");
        //    rotation_ = rotation.REVERSE02;
        //}

        //ブロックがない場合回転
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

    public void Destory()
    {
        Destroy(gameObject);
    }
}