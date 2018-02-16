using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ButtonHighLightColorAnim : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    Gradient gradient;

    [SerializeField]
    float    speed;

    Button   button;
    float    time;
    bool     isSelected;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if(!isSelected) { return; }
        
        time += Time.deltaTime * speed;
        time  = Mathf.Repeat(time, 1.0f);

        ColorBlock colors = button.colors;
        colors.highlightedColor = gradient.Evaluate(time);
        button.colors = colors;
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        time = 0;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }
}
