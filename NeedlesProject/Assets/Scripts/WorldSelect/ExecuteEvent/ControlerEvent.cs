using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public interface OnAxisHorizontalEvent : IEventSystemHandler
{
    //イベントを送信
    void OnAxisHorizontalEvent(float axis);
}
