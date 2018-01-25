using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeUI : TransitionBase
{
    Graphics graphics;

    protected override void Awake()
    {
        base.Awake();

        graphics = GetComponent<Graphics>();
    }

    private void Update()
    {

    }

    protected override void ChangeValue(float amount)
    {
        throw new System.NotImplementedException();
    }
}
