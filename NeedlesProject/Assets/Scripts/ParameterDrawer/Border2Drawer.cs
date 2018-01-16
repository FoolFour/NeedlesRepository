using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Border2Drawer : ParametersDrawerBase
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = ConvertTime(data.border2);
    }
}
