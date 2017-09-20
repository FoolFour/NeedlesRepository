﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageStart : MonoBehaviour
{
    public  float              initNearValue;
    public  float              speed;
    private UnityEngine.Camera cam;

    private void Awake()
    {
        cam = GetComponent<UnityEngine.Camera>();
    }

    private void Start()
    {
        cam.nearClipPlane = initNearValue;
    }

    private void Update()
    {
        cam.nearClipPlane -= speed;
        if (cam.nearClipPlane < 0.3f)
        {
            cam.nearClipPlane = 0.3f;
            Destroy(this);
        }
    }
}
