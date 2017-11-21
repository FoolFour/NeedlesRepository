﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushButtonToScenechange : MonoBehaviour
{
    [SerializeField]
    string buttonName;

    [SerializeField]
    string  sceneName;

    [SerializeField]
    UnityEngine.SceneManagement.LoadSceneMode mode;

    SceneChanger sceneChanger;

    private void Awake()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if(Input.GetButtonDown(buttonName))
        {
            sceneChanger.SceneChange(sceneName, mode);
        }
    }
}
