using UnityEngine;
using UnityEngine.UI;

public class NextStageData : MonoBehaviour
{
    SceneChangeFade sceneChange;
    Button          button;
    string          sceneName;

    private void Reset()
    {
        sceneChange = FindObjectOfType<SceneChangeFade>();
    }

    private void Awake()
    {
        if(sceneChange == null)
        { 
            sceneChange = FindObjectOfType<SceneChangeFade>();
        }

        button    = GetComponent<Button>();
        sceneName = PlayerPrefs.GetString(PrefsDataName.NextSene);

        //何も設定されてなければなにもしない
        if(sceneName == "")
        { 
            button.interactable = false;
        }
        else
        {
            button.onClick.AddListener(ChangeNextStage);
        }
    }

    public void ChangeNextStage()
    {
        sceneChange.SceneChange(sceneName);
    }
}
