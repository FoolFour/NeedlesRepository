using UnityEngine;

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
            Debug.Log("ゲームを終了");
            Application.Quit();
        }
    }
}
