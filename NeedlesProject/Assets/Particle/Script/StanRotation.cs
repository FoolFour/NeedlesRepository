using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanRotation : MonoBehaviour {
    
    //回転速度
    public float rotationspead;
    //回転するオブジェクト
    public GameObject gameobj;
    //
    public bool lookatobj;
    //どの方向を向くか
    public Camera camera_;
    void Start () {
        //gameobj = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
	}
	

	void Update () {

        camera_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //回転速度
        transform.eulerAngles += new Vector3(0, 1, 0)*rotationspead;

        //指定されたゲームオブジェクトの方を向く
        if (lookatobj == true)
        {
            gameobj.transform.LookAt(camera_.gameObject.transform);
        }
	}
}
