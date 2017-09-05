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

        int layerMask = ~(1 << 8);
        Vector3 nextvector = dir.normalized;
        RaycastHit hit;
        if(Physics.CapsuleCast(transform.position, nextvector.normalized, 0.2f, nextvector.normalized,out hit,len - 1,layerMask))
        {

            Vector3 rightsearch = nextvector;
            Vector3 leftsearch = nextvector;
            for (int i = 0; i < 180; i++)
            {
                rightsearch = Quaternion.AngleAxis(1, Vector3.forward) * rightsearch.normalized;
                if (!Physics.CapsuleCast(transform.position, rightsearch.normalized, 0.2f, rightsearch.normalized, out hit, len - 1, layerMask))
                {
                    nextvector = rightsearch;
                    break;
                }

                leftsearch = Quaternion.AngleAxis(1, Vector3.back) * leftsearch.normalized;
                if (!Physics.CapsuleCast(transform.position, leftsearch.normalized, 0.2f, leftsearch.normalized, out hit, len - 1, layerMask))
                {
                    nextvector = leftsearch;
                    break;
                }
            }
        }

        transform.transform.localScale = new Vector3(1, 1, len);

        //扇の当たり判定（関数化予定)
        var hitcolider = Physics.OverlapSphere(transform.position, len, layerMask);
        foreach (var colider in hitcolider)
        {
            if (sector_hit(colider, nextvector, len)) return;
        }
        if (!Input.GetKey(KeyCode.X)) transform.localRotation = Quaternion.LookRotation(nextvector.normalized);
    }

    bool sector_hit(Collider colider,Vector3 next,float len)
    {
        if (Vector2Cross(transform.forward, next) < 0)
        {
            Vector3 closetpoint = colider.ClosestPointOnBounds(transform.forward * len);
            float angle = Vector2Cross(transform.forward, closetpoint);
            if (angle > 0) return false;
            angle = Vector2Cross(next.normalized, closetpoint);
            if (angle < 0) return false;
            return true;
        }
        else
        {
            Vector3 closetpoint = colider.ClosestPointOnBounds(transform.forward * len);
            float angle = Vector2Cross(transform.forward, closetpoint);
            if (angle < 0) return false;
            angle = Vector2Cross(next.normalized, closetpoint);
            if (angle > 0) return false;
        }
        return true;
    }

    float Vector2Cross(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }
}
