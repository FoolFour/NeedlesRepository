using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SyncSkyBoxRotToObjMove : MonoBehaviour
{
    [SerializeField]
    float    rotateRate;

    float    prevX;

    private void Update()
    {
        const string _Rotation = "_Rotation";

        float nowX = transform.position.x;

        float rot = RenderSettings.skybox.GetFloat(_Rotation);
        
        rot += (nowX - prevX) * rotateRate;
        rot  = Mathf.Repeat(rot, 360.0f);

        RenderSettings.skybox.SetFloat(_Rotation, rot);

        prevX = nowX;
    }
}
