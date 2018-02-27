using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class OnSubmitPlaySE : MonoBehaviour, ISubmitHandler
{
    [SerializeField]
    string seName;

    public void OnSubmit(BaseEventData eventData)
    {
        Sound.PlaySe(seName);
    }
}
