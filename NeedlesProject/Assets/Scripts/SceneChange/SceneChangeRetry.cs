using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneChangeRetry : MonoBehaviour
{
    private TaskLock  taskLock;

    [SerializeField]
    private BlockTiling tiling;

    public void SceneChange()
    {
        taskLock.Run(Change);
    }

    private void Reset()
    {
        tiling = FindObjectOfType<BlockTiling>();
    }

    private void Awake()
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
        SceneManager.LoadScene(PlayerPrefs.GetString(PrefsDataName.Scene));
    }

    private IEnumerator SceneChangePerformance()
    {
        yield return tiling.FadeInStart();

        PlayerPrefs.SetString(PrefsDataName.FadeStart, bool.TrueString);
    }
}
