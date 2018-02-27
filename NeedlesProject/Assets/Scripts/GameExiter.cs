using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExiter : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(GamePad.IsHoldBackAndStart())
        {
            var fade = FindObjectOfType<SceneChangeFade>();

            if(fade == null)
            {
                SceneManager.LoadScene("title");
            }
            else
            {
                fade.SceneChange("title");
            }
        }
    }
}
