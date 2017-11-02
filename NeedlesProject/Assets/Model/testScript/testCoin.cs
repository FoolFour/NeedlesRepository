using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCoin : MonoBehaviour {

    private Transform _child;
    // Use this for initialization
    void Start () {
        _child = transform.FindChild("default");

    }

    // Update is called once per frame
    void Update () {
        _child.transform.Rotate(new Vector3(0, 5, 0));

    }
    public void OnTriggerEnter(Collider collision)
    {
        Destroy(transform.gameObject);
        
    }
}
