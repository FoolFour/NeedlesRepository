using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

public class Goal : BlockBase
{
    [SerializeField]
    private SceneChanger sceneChanger;

    private void Reset()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
    }

    public override void StickEnter(GameObject arm)
    {
        sceneChanger.SceneChange("Result", LoadSceneMode.Additive);
        StickPointOut();
        Destroy(GetComponent<BoxCollider>());
    }

    public override void StickExit()
    {
        base.StickExit();
    }
}
