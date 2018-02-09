using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageNameDrawer : ParametersDrawerBase
{
	Text text;

    private void Start()
    {
		text = GetComponent<Text>();
    }

    private void Update()
    {
        if(text.text == data.stageName) { return; }
        text.text = data.stageName;
    }
}
