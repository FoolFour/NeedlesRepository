using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera          mainCamera;
      
    [SerializeField]
    int             current;

    [SerializeField]
    List<Transform> controlPoint;

    private void OnValidate()
    {
        current = Mathf.Max(0, current);
    }

    private void Reset()
    {
        mainCamera = Camera.main;

        foreach(Transform child in transform)
        {
            controlPoint.Add(child);
        }
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = controlPoint[current].position;
    }

    public void ChangeControlPoint(int num)
    {
#warning TODO
    }
}
