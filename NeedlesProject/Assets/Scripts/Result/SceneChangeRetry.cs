using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneChangeRetry : MonoBehaviour
{
    private TaskLock       taskLock;

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
        SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
    }

    private IEnumerator SceneChangePerformance()
    {
        yield return image.FadeInStart(color);

        const string Fade = "Fade";
        PlayerPrefs.SetFloat(Fade + "_R", color.r);
        PlayerPrefs.SetFloat(Fade + "_G", color.g);
        PlayerPrefs.SetFloat(Fade + "_B", color.b);
    }
}
