using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIButtonOneShot : MonoBehaviour
{
    Button button;
    bool isOnClick;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        isOnClick = false;
    }

    private void Update()
    {
        if(isOnClick)
        {
            if(Input.GetButtonUp(GamePad.Submit))
            {
                DisableButton();
            }
        }
    }

    private void OnClick()
    {
        isOnClick = true;
    }

    private void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }
}
