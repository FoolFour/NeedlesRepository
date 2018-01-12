using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

public class GoalAnimation : MonoBehaviour
{
    [SerializeField]
    int hideBackgroundFrame = 4;

    Animator          animator;
    //AnimatorStateInfo info;

    ImageSyncHider imageSyncHider;

    public void StartAnimation()
    {
        if(animator != null) { return; }

        //プレイヤー追跡用のカメラを削除
        GameCamera.Camera camera = Camera.main.GetComponent<GameCamera.Camera>();
        DestroyImmediate(camera);
        DestroyImmediate(Camera.main.transform.GetChild(0).gameObject);

        //アニメーションの再生
        animator = GetComponent<Animator>();
        animator.SetBool("Goal", true);

        imageSyncHider = FindObjectOfType<ImageSyncHider>();
    }

    //Animationからの呼び出し
    public void HideBackground()
    {
        GetComponent<Goal>().SceneChange();

        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        if(imageSyncHider == null) { yield break; }
        Debug.Log(imageSyncHider);

        for(int i = 1; i <= hideBackgroundFrame; i++)
        {
            imageSyncHider.ApplyImageFillAmount(1.0f - ((float)i / hideBackgroundFrame));
            yield return null;
        }

        imageSyncHider.ApplyImageFillAmount(0);
    }
}
