using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveStageName : MonoBehaviour
{
    private void Awake()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString(PrefsDataName.Scene, activeScene.name);
    }
}
