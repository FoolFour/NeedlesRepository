using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_2Dcross : MonoBehaviour {

    public Transform dir1;
    public Transform dir2;
    public Transform point;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        float c1 = Vector2Cross(dir1.forward, point.position);
        float c2 = Vector2Cross(dir2.forward, point.position);

        if(c1 > 0)
        {
            Debug.Log("C1から左");
        }
        else
        {
            Debug.Log("C1から右");
        }

        if (c2 > 0)
        {
            Debug.Log("C2から左");
        }
        else
        {
            Debug.Log("C2から右");
        }
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
