using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public abstract class SceneChanger : MonoBehaviour
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
    public void SceneChange(string name, LoadSceneMode mode = LoadSceneMode.Single)
    {
        sceneName = name;
        sceneMode = mode;
        taskLock.Run(Change);
    }

    public void SceneChangeAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
    {
        sceneName = name;
        sceneMode = mode;
        taskLock.Run(ChangeAsync);
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

    private IEnumerator ChangeAsync()
    {
        yield return StartCoroutine(SceneChangePerformance());
        SceneManager.LoadSceneAsync(sceneName, sceneMode);
    }

    protected abstract IEnumerator SceneChangePerformance();
}
