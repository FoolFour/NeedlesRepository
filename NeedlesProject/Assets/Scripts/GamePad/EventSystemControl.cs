using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventSystemControl : MonoBehaviour
{
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button nextStageButton;
    [SerializeField]
    private Button worldSelectButton;
    
    private EventSystem eventSystem;
    
    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    public void Start()
    {
        Debug.Log("EventSystemの設定");

        EventSystem.current = eventSystem;
        EventSystem.current.UpdateModules();

        if(nextStageButton.interactable)
        {
            EventSystem.current.SetSelectedGameObject(nextStageButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(worldSelectButton.gameObject);

            Navigation navigation;

            navigation = retryButton.navigation;
            navigation.selectOnLeft  = worldSelectButton;
            retryButton.navigation   = navigation;

            navigation = worldSelectButton.navigation;
            navigation.selectOnRight     = retryButton;
            worldSelectButton.navigation = navigation;
        }
    }
}
