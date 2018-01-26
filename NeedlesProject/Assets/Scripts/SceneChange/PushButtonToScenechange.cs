using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushButtonToScenechange : MonoBehaviour
{
    [SerializeField]
    string  sceneName;

    [SerializeField]
    GameObject eraseObj;

    [SerializeField]
    UnityEngine.SceneManagement.LoadSceneMode mode;

    SceneChangeFade sceneChanger;

    private void Awake()
    {
        sceneChanger = GetComponent<SceneChangeFade>();
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            Destroy(eraseObj);
            sceneChanger.SceneChange(sceneName, mode);
            Sound.PlaySe("TitleDecision");
            PlayerPrefs.SetInt(PrefsDataName.SelectedWorld, 0);
        }
    }

    public void SceneChange()
    {
        Destroy(eraseObj);
        sceneChanger.SceneChange(sceneName, mode);
        Sound.PlaySe("TitleDecision");
    }
}
