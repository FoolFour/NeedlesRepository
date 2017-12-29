using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Border1Drawer : ParametersDrawer
{
    Text text;

	private void Start()
	{
		text = GetComponent<Text>();
		text.text = ConvertTime(data.border1);
	}
}
