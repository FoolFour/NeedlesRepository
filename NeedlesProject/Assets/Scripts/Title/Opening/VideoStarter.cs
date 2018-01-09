using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoStarter : MonoBehaviour
{

    public VideoPlayer m_VideoPlayer;
    public GameObject m_VideoPanel;
    public GameObject m_sceneChanger;
    public Image m_TitleImage;
    public Text m_TitleText;

    Coroutine eventCoroutine;
    bool Eventing = false;

    public void Update()
    {
        if (Input.anyKeyDown && !Eventing)
        {
            Sound.PlaySe("TitleOnButton1");
            Eventing = true;
            m_TitleImage.enabled = false;
            m_TitleText.enabled = false;
            eventCoroutine = StartCoroutine(VideoStart());
        }
    }

    IEnumerator VideoStart()
    {
        //カメラを動かす処理
        {
            float t = 0;
            var from = Camera.main.transform.rotation;
            var to = Quaternion.AngleAxis(-180,Vector3.up);
            while (t <= 1)
            {
                Camera.main.transform.rotation = Quaternion.Slerp(from, to, t);
                t += 0.003f;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.5f);

        {
            float t = 0;
            var from = m_VideoPanel.transform.localScale;
            var to = new Vector3(7.7f,4.3f,0.0f);
            while (t <= 1)
            {
                m_VideoPanel.transform.localScale = Vector3.Lerp (from, to, t);
                t += 0.3f;
                yield return new WaitForEndOfFrame();
            }
        }

        Sound.PlaySe("ScreenUp");
        yield return new WaitForSeconds(0.5f);

        m_VideoPlayer.Play();

        yield return new WaitForSeconds(1.0f);

        m_sceneChanger.SetActive(true);

        while (m_VideoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        m_sceneChanger.GetComponent<PushButtonToScenechange>().SceneChange();

    }
}
