using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PushButtonSE : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    [SerializeField]
    [FormerlySerializedAs("seName")]
    string submitSEName;

    [SerializeField]
    string cancelSEName;

    public void OnSubmit(BaseEventData eventData)
    {
        if(submitSEName != "")
        {
            Sound.PlaySe(submitSEName);
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        if(cancelSEName != "")
        {
            Sound.PlaySe(cancelSEName);
        }
    }
}
