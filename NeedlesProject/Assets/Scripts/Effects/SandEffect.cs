using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandEffect : MonoBehaviour {

    [Tooltip("回転力")]
    public Vector3 m_Rotate;
        	
	// Update is called once per frame
	void Update () {
        transform.Rotate(m_Rotate);
		
	}
}
