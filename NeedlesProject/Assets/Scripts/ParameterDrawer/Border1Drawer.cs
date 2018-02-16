﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Border1Drawer : ParametersDrawerBase
{
    Text text;

    private IEnumerator Start()
    {
        text = GetComponent<Text>();
        yield return null;

        text.text = ConvertTime(data.border1);
    }
}
