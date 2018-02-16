using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeRepeat : MonoBehaviour
{
    [SerializeField]
    float speed;
    Graphic[] graphics;
    Shadow shadow;

    private void Awake()
    {
        graphics = GetComponents<Graphic>();
        shadow   = GetComponent<Shadow>();
    }

    private IEnumerator Start()
    {
        while(true)
        {
            for (float i = 0.0f; i <= 1.0f; i+=Time.deltaTime * speed)
            {
                SetAlpha(i);
                yield return null;
            }

            for (float i = 0.0f; i <= 1.0f; i+=Time.deltaTime * speed)
            {
                SetAlpha(1.0f - i);
                yield return null;
            }
        }
    }

    private void SetAlpha(float a)
    {
        foreach (var graphic in graphics)
        {
            Color col = graphic.color;
            col.a = a;
            graphic.color = col;
        }
        {
            Color col = shadow.effectColor;
            col.a = a;
            shadow.effectColor = col;
        }
    }
}
