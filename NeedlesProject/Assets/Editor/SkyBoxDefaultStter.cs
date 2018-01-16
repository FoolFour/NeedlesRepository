using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[InitializeOnLoad]
public static class SkyBoxDefaultSetter
{
    static SkyBoxDefaultSetter()
    {
        EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged()
    {
        if( EditorApplication.isPlaying && 
           !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Material mat = RenderSettings.skybox;
            mat.SetFloat("_Rotation", 0.0f);
        }
    }
}
