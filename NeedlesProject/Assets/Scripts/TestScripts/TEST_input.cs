using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_input : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, y, 0);

        float defeated = Mathf.Min(1.0f, (Mathf.Abs(dir.x) + Mathf.Abs(dir.y)));
        float len = defeated * 5;

        Debug.DrawRay(transform.localPosition, dir.normalized * len, Color.black);
        transform.localPosition = Vector3.zero;
        float angle = Vector3.Dot(Quaternion.AngleAxis(90, Vector3.forward) * transform.forward, dir.normalized);
        if (dir != Vector3.zero) transform.GetComponent<Rigidbody>().angularVelocity = (Vector3.forward * (angle * 10));
        transform.transform.localScale = new Vector3(1, 1, len);

    }
}
