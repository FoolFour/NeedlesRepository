using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnEnableButtonReselect : MonoBehaviour
{
    [SerializeField]
    EventSystem eventSystem;

    GameObject  reselect;

    private void Start()
    {
        reselect = eventSystem.firstSelectedGameObject;
    }

    private void OnEnable()
    {
        StartCoroutine(Reselect());
    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    private IEnumerator Reselect()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(reselect);
    }
}
