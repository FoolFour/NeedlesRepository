using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextDrawer : MonoBehaviour
{
    Text text;
    string defaultText;

    private void Awake()
    {
        text = GetComponent<Text>();
        defaultText = text.text;
    }

    private void Update()
    {
        text.text = defaultText;
    }
}
