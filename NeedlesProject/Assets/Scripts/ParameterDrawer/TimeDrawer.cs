using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimeDrawer : ParametersDrawerBase
{
	Text text;

	private void Start()
	{
		text = GetComponent<Text>();
	}

    private void Update()
    {
        text.text = ConvertTime(data.time);
    }
}
