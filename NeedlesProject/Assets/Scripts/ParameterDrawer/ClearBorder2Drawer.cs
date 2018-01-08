﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClearBorder2Drawer : ParametersDrawerBase
{
	private UIBehaviour image;

	private void Start()
	{
		image = GetComponent<UIBehaviour>();
	}

    private void Update()
    {
        image.enabled = data.isBorder2Clear;
    }
}
