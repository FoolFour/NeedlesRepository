using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    /////////////////////////////
    // 変数(NonSerializeField) /
    ///////////////////////////
    private string         sceneName;
    private LoadSceneMode  sceneMode;

    private TaskLock       taskLock;

    //////////////////
    // 関数(public) /
    ////////////////

    /// <summary>シーンを切り替えます</summary>
    public void SceneChange(string name)
    {
        sceneName = name;
        sceneMode = LoadSceneMode.Single;
        taskLock.Run(Change);
    }

    /// <summary>シーンを切り替えます</summary>
    public void SceneChange(string name, LoadSceneMode mode)
    {
        sceneName = name;
        sceneMode = mode;
        taskLock.Run(Change);
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    protected virtual void Awake()
    {
        taskLock = GetComponent<TaskLock>();
        if(taskLock == null)
        {
            taskLock = gameObject.AddComponent<TaskLock>();
        }
    }

    private IEnumerator Change()
    {
        yield return StartCoroutine(SceneChangePerformance());
        SceneManager.LoadScene(sceneName, sceneMode);
    }

    protected virtual IEnumerator SceneChangePerformance()
    {
        yield return null;
    }
}
