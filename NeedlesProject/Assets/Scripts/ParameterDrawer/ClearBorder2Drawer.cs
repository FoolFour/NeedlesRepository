using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClearBorder2Drawer : ParametersDrawerBase
{
	private UIBehaviour image;

	private void OnEnable()
	{
		image = GetComponent<UIBehaviour>();
        image.enabled = data.isBorder2Clear;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(data.isBorder2Clear);
        }
	}
}
