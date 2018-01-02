using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightingSetter : MonoBehaviour
{
    [SerializeField]
    Material skyboxMaterial;

    [SerializeField]
    Color    skyColor;
    
    [SerializeField]
    Color    equatorColor;

    [SerializeField]
    Color    groundColor;

    private void Awake()
    {
        RenderSettings.skybox              = skyboxMaterial;
        
        RenderSettings.ambientMode         = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor     = skyColor;
        RenderSettings.ambientEquatorColor = equatorColor;
        RenderSettings.ambientGroundColor  = groundColor;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
