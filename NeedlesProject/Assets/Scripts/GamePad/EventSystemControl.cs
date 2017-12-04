using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventSystemControl : MonoBehaviour
{
    [SerializeField]
    private GameObject selectGameObject;

    private EventSystem eventSystem;
    private StandaloneInputModule module;

    private void Awake()
    {
        module = GetComponent<StandaloneInputModule>();
        eventSystem = GetComponent<EventSystem>();
    }

    private void Start()
    {
        module.horizontalAxis = GamePad.Horizontal;
        module.verticalAxis   = GamePad.Vertical;

        eventSystem.SetSelectedGameObject(selectGameObject);
        EventSystem.current = eventSystem;
        eventSystem.UpdateModules();
    }

    private void Update()
    {

    }
}
