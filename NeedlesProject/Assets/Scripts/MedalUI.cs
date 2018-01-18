using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedalUI : MonoBehaviour
{
    [SerializeField]
    StageBasicInfoManager info;
    Animator              animator;

    private void Awake()
    {
        info.OnSelectStageChanged += OnSelectStageChanged;

        animator = GetComponent<Animator>();
    }

    private void OnSelectStageChanged(int stage)
    {
        const int baseLayer = 0;
        animator.Play("highlight", baseLayer, 0.0f);
    }
}
