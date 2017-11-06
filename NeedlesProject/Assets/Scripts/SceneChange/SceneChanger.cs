using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////

    [SerializeField]
    [Header("単位:秒")]
    private uint           changeTimer;

    /////////////////////////////
    // 変数(NonSerializeField) /
    ///////////////////////////
    private string         sceneName;
    private LoadSceneMode  sceneMode;

    private WaitForSeconds wait;

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

    private void Awake()
    {
        wait = new WaitForSeconds(changeTimer);
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
