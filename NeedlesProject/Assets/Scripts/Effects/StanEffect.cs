using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanEffect : MonoBehaviour
{
    public void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 3);
    }
}
