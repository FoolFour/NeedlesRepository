using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 扇形による当たり判定の実装テスト
/// </summary>
public class TEST_sector_Hit : MonoBehaviour {

    public GameObject debugpoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, y, 0);

        float defeated = Mathf.Min(1.0f, (Mathf.Abs(dir.x) + Mathf.Abs(dir.y)));
        float len = defeated * 5;

        Debug.DrawRay(transform.localPosition, dir.normalized * len, Color.black);

        int layerMask = ~(1 << 8);
        Vector3 nextvector = dir.normalized;

        sector_hit(nextvector, len, layerMask);


        if(!Input.GetKey(KeyCode.X)) transform.localRotation = Quaternion.LookRotation(nextvector.normalized);
        transform.localScale = new Vector3(1, 1, len);
    }

    bool sector_hit(Vector3 next,float len,int layerMask)
    {
        var hitcolider = Physics.OverlapSphere(transform.position, len, layerMask);
        if (hitcolider.Length <= 0) return false;
        foreach(var colider in hitcolider)
        {

            if (Vector2Cross(transform.forward, next) > 0)
            {
                Debug.Log("左に存在");
                Vector3 closetpoint = colider.ClosestPointOnBounds(transform.forward * len);
                debugpoint.transform.position = closetpoint;
                float angle = Vector2Cross(transform.forward, closetpoint);
                //Debug.Log(angle);
                if (angle > 0) return false;
                angle = Vector2Cross(next.normalized, closetpoint);
                //Debug.Log(angle);
                if (angle < 0) return false;
            }
            else
            {
                Debug.Log("右に存在");
                Vector3 closetpoint = colider.ClosestPointOnBounds(transform.forward * len);
                debugpoint.transform.position = closetpoint;
                float angle = Vector2Cross(transform.forward, closetpoint);
                //Debug.Log(angle);
                if (angle > 0) return true;
                angle = Vector2Cross(next.normalized, closetpoint);
                //Debug.Log(angle);
                if (angle < 0) return true;

            }
        }

        return true;
    }


    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
