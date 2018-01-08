using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeUIGraphic : MonoBehaviour
{
    List<Graphic> list;
    List<float>   defaultAlpha;
    
    private void Awake()
    {
        list = new List<Graphic>(GetComponentsInChildren<Graphic>());
        defaultAlpha = new List<float>();
        list.ForEach((item)=>
        {
            defaultAlpha.Add(item.color.a);
        });
    }

    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        ApplyAlpha(0.0f);
        yield return null;
        
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime * 0.5f)
        {
            ApplyAlpha(t);
            yield return null;
        }
    }

    private void ApplyAlpha(float a)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Color c       = list[i].color;
            c.a           = Mathf.Lerp(0.0f, defaultAlpha[i], a);
            list[i].color = c;
        }
    }
}
