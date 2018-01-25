using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeatScale : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    float speed;

    float amount;
    float time;

    private void Update()
    {
        time += Time.deltaTime * speed;
        time  = Mathf.Repeat(time, 1.0f);
        float val = curve.Evaluate(time);

        Vector3 scale = transform.localScale;
        scale.x = val;
        transform.localScale = scale;
    }
}
