using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedalUI : MonoBehaviour
{
    Animator       animator;

    private void Awake()
    {
        FindObjectOfType<StageBasicInfoManager>().OnSelectStageChanged
            += OnSelectStageChanged;
        animator = GetComponent<Animator>();
    }

    private void OnSelectStageChanged(int stage)
    {
        const int baseLayer = 0;
        animator.Play("highlight", baseLayer, 0.0f);
    }
}
