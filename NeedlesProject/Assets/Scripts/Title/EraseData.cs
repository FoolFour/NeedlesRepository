using UnityEngine;
using UnityEngine.EventSystems;

public class EraseData : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        PlayerPrefs.DeleteAll();
    }
}
