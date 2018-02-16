using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CancelButtonEvent : MonoBehaviour, ICancelHandler
{
    [SerializeField]
    UnityEvent unityEvent;

    public void OnCancel(BaseEventData eventData)
    {
        unityEvent.Invoke();
    }
}
