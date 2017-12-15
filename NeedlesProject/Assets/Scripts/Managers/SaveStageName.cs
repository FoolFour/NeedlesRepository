using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveStageName : MonoBehaviour
{
    public string nextStage;

    private void Awake()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString(PrefsDataName.Scene,    activeScene.name);

        PlayerPrefs.SetString(PrefsDataName.NextSene, nextStage);

        Destroy(this);
    }
}
