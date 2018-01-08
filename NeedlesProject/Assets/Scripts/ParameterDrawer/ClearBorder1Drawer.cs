using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClearBorder1Drawer : ParametersDrawerBase
{
	private UIBehaviour image;

	private void OnEnable()
	{
		image = GetComponent<UIBehaviour>();
	}

    private void Update()
    {
        image.enabled = data.isBorder1Clear;
    }
}
