using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Transform _child;
    // Use this for initialization
    void Start () {
        _child = transform.Find("default");

    }

    // Update is called once per frame
    void Update () {
        _child.transform.Rotate(new Vector3(0, 5, 0));

    }
    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag.Contains("Player"))
        Destroy(transform.gameObject);
        
    }
}
