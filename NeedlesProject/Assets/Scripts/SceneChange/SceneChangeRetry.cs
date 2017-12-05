using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneChangeRetry : MonoBehaviour
{
    private TaskLock  taskLock;

    [SerializeField]
    private FadeImage image;

    [SerializeField]
    private Color     color;

    public void SceneChange()
    {
        taskLock.Run(Change);
    }

    private void Reset()
    {
        image = FindObjectOfType<FadeImage>();
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
        yield return image.FadeInStart(color);
        
        PlayerPrefs.SetFloat(PrefsDataName.Fade_R, color.r);
        PlayerPrefs.SetFloat(PrefsDataName.Fade_G, color.g);
        PlayerPrefs.SetFloat(PrefsDataName.Fade_B, color.b);

        PlayerPrefs.SetInt(PrefsDataName.FadeStart, 1);
    }
}
