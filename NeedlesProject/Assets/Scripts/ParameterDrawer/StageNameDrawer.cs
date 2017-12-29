using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageNameDrawer : ParametersDrawer
{
	Text text;

    private void Start()
    {
		text = GetComponent<Text>();
		text.text = data.stageName;
    }
}
