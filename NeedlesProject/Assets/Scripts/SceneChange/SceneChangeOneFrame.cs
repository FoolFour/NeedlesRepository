using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChangeOneFrame : MonoBehaviour
{
    [SerializeField]
    string sceneName;

    private IEnumerator Start()
    {
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
