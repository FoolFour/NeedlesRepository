using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UpButtonSE : MonoBehaviour
{
    [SerializeField]
    string submitSEName;

    [SerializeField]
    string cancelSEName;

    private void Update()
    {
        if(Input.GetButtonUp(GamePad.Submit))
        {
            if(submitSEName != "")
            {
                Sound.PlaySeOne(submitSEName);
            }
        }

        if(Input.GetButtonUp(GamePad.Cancel))
        {
            if(cancelSEName != "")
            {
                Sound.PlaySeOne(cancelSEName);
            }
        }
    }
}
