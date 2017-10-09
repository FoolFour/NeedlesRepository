﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WorldUI : MonoBehaviour
{
    [SerializeField]
    WorldSelect worldSelect;

    Text        text;

    float       alpha;

    private void Reset()
    {
        worldSelect = FindObjectOfType<WorldSelect>();
    }

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (worldSelect.IsChangeAnimation)
        {
            alpha = 0.0f;
        }
        else
        {
            alpha += Time.deltaTime;
            text.text  = "World " + worldSelect.SelectWorld;
        }
        Color col = text.color;
        col.a = alpha;
        text.color = col;
    }
}
