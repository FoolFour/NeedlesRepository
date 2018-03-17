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

    private void Start()
    {
        float height = Screen.height;
        float width  = Screen.width;
        Rect texRect = GetComponent<Image>().sprite.textureRect;

        float imageWidth  = texRect.width;
        float imageHeight = texRect.height;

        Vector2 pos;
        pos.x = imageWidth /10.0f + width /20.0f;
        pos.x *= -1;
        pos.y = imageHeight/10.0f + height/20.0f;

        Debug.Log(pos);

        

        GetComponent<RectTransform>().anchoredPosition = pos;
    }

    private void Update()
    {
        float alpha = graphic.color.a;

        switch (transition.FadeState)
        {
            case TransitionBase.FadeType.In:
                alpha = 1.0f;
                break;

            case TransitionBase.FadeType.Out:
                alpha = 0.0f;
                break;

            case TransitionBase.FadeType.FadeIn:
                alpha = transition.Amount;
                break;

            case TransitionBase.FadeType.FadeOut:
                alpha = transition.Amount;
                break;
        }

        Color col = graphic.color;
        col.a = alpha;
        graphic.color = col;
    }
}
