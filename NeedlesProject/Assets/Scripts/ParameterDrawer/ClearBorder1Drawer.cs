using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClearBorder1Drawer : ParametersDrawer
{
	private bool        showFlag;
	private UIBehaviour image;

	private void Reset()
	{
		data = FindObjectOfType<StageData>();
	}

	private void Start()
	{
		image = GetComponent<UIBehaviour>();
		image.enabled = data.isBorder1Clear;
	}
}
