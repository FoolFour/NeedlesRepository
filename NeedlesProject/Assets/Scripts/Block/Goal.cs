using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private SceneChanger sceneChanger;

    private void Reset()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            sceneChanger.SceneChange("Result", LoadSceneMode.Additive);
            Destroy(this);
        }
    }
}
