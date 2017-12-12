using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushButtonToScenechange : MonoBehaviour
{
    [SerializeField]
    string buttonName;

    [SerializeField]
    string  sceneName;

    [SerializeField]
    GameObject eraseObj;

    [SerializeField]
    UnityEngine.SceneManagement.LoadSceneMode mode;

    SceneChangeTimer sceneChanger;

    private void Awake()
    {
        sceneChanger = GetComponent<SceneChangeTimer>();
    }

    private void Update()
    {
        if(Input.GetButtonDown(buttonName))
        {
            Destroy(eraseObj);
            sceneChanger.SceneChange(sceneName, mode);
            Sound.PlaySe("TitleDecision");
        }
    }
}
