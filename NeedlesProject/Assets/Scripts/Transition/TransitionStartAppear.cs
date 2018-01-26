using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TransitionStartAppear : MonoBehaviour
{
    [SerializeField]
    TransitionBase transition;

    Graphic graphic;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void Update()
    {
        float alpha = graphic.color.a;

        if(transition.FadeState == TransitionBase.FadeType.FadeIn)
        {
            alpha += Time.deltaTime * transition.FadeSpeed;
        }
        else if(transition.FadeState == TransitionBase.FadeType.FadeOut)
        {
            alpha -= Time.deltaTime * transition.FadeSpeed; 
        }

        Color col = graphic.color;
        col.a = alpha;
        graphic.color = col;
    }
}
