using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UpButtonSE : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    [SerializeField]
    string submitSEName;

    [SerializeField]
    string cancelSEName;

    public void OnSubmit(BaseEventData eventData)
    {
        StartCoroutine(WaitSubmitButtonUp());
    }

    public void OnCancel(BaseEventData eventData)
    {
        StartCoroutine(WaitCancelButtonUp());
    }

    public IEnumerator WaitSubmitButtonUp()
    {
        while (Input.GetButton(GamePad.Submit))
        {
            yield return null;
        }
        
        if(submitSEName != "")
        {
            Sound.PlaySe(submitSEName);
        }
    }

    public IEnumerator WaitCancelButtonUp()
    {
        while (Input.GetButton(GamePad.Cancel))
        {
            yield return null;
        }

        if(cancelSEName != "")
        {
            Sound.PlaySe(cancelSEName);
        }
    }
}
