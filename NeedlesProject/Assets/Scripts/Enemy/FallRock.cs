using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : BlockBase
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void StickEnter(GameObject arm)
    {
        Destroy(gameObject);
        base.StickEnter(arm);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 temp = other.gameObject.transform.position - transform.position;
            temp.y = 1;
            if (other.gameObject.GetComponent<Player>())
            {
                other.gameObject.GetComponent<Player>().StanMode(temp.normalized * 10);
            }
            else
            {
                other.gameObject.transform.parent.GetComponent<Player>().StanMode(temp.normalized * 10);
            }
        }
    }
}
