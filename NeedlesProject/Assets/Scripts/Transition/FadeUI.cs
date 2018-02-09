using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FadeUI : TransitionBase
{
    Graphic graphic;

    protected void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    protected override void ChangeValue(float amount)
    {
        Color col     = graphic.color;
        col.a         = amount;
        graphic.color = col;
    }
}
