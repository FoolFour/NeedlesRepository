using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityChangeMaterial : MonoBehaviour
{
    static readonly int front = 2;
    static readonly int top   = 0;
    static readonly int left  = 1;

    private void Start()
    {
        Vector3 scale = transform.localScale;
        var mats = GetComponent<Renderer>().materials;

        mats[front].SetFloat("_NWidth" , scale.x/2.5f);
        mats[front].SetFloat("_NHeight", scale.y/2.5f);
        mats[front].SetFloat("_NOffset", Random.Range(-100.0f, 100.0f));

        mats[  top].SetFloat("_NWidth" , scale.x/2.5f);
        mats[  top].SetFloat("_NHeight", scale.z/2.5f);
        mats[  top].SetFloat("_NOffset", Random.Range(-100.0f, 100.0f));

        mats[ left].SetFloat("_NWidth" , scale.z/2.5f);
        mats[ left].SetFloat("_NHeight", scale.y/2.5f);
        mats[ left].SetFloat("_NOffset", Random.Range(-100.0f, 100.0f));
    }
}
