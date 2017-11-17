using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRay : MonoBehaviour {
    //地面と当たっているか
    Ray ray;
    public RaycastHit hit;
    //rayの長さ
    private float distance = 1.5f;
    //当たっているならtrue当たってないならfalse
    public bool ishitUnder;
    //何に対して当たるようにするか
    public LayerMask mask;

    void Start()
    {
    }

    void Update()
    {
        ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        ishitUnder = Physics.Raycast(ray, out hit, 1.5f, mask);
    }
}
