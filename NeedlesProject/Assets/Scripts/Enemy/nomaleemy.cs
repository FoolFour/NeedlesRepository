using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomaleemy : BlockBase
{
    //移動速度
    public float movespeed;

    string undername;

    RaycastHit hit;

    public Transform CenterOfBalance;
    void Start()
    {

    }

    void Update()
    {
        gameObject.transform.position += transform.right * movespeed * Time.deltaTime;

        if(Physics.Raycast( 
            CenterOfBalance.position,   
            -transform.up,
            out hit,
            float.PositiveInfinity))
        {
            Quaternion q = Quaternion.FromToRotation(
                transform.up, hit.normal);

            transform.rotation *= q;
        }
    }

    public override void StickEnter(GameObject arm)
    {
        Debug.Log("敵に当たった");
        base.StickEnter(arm);
    }
}