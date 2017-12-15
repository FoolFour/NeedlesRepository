using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventSystemControl : MonoBehaviour
{
    [SerializeField]
    private GameObject selectGameObject;

    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button nextStageButton;
    [SerializeField]
    private Button worldSelectButton;
    
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

        if(!nextStageButton.interactable)
        {
            eventSystem.SetSelectedGameObject(worldSelectButton.gameObject);

            Navigation navigation;

            navigation = retryButton.navigation;
            navigation.selectOnLeft  = worldSelectButton;
            retryButton.navigation   = navigation;

            navigation = worldSelectButton.navigation;
            navigation.selectOnRight     = retryButton;
            worldSelectButton.navigation = navigation;
        }
    }

    private void Update()
    {

    }
}
