using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : BlockBase, IRespawnMessage
{
    float m_timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= 10) Destroy(gameObject);
    }
    public override void StickEnter(GameObject arm)
    {
        Destroy(gameObject);
        base.StickEnter(arm);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
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
        Destroy(gameObject);
    }

    public void RespawnInit()
    {
        Destroy(gameObject);
    }
}
