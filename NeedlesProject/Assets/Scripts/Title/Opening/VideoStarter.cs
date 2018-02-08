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

    public void Start()
    {
        var color = m_VideoPanel.GetComponent<MeshRenderer>().material.color;
        color.a = 0;
        m_VideoPanel.GetComponent<MeshRenderer>().material.color = color;
    }

    public void ShowVideo()
    {
        if (!Eventing)
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
        {
            float t = 0;
            var from = Camera.main.transform.rotation;
            var to = Quaternion.AngleAxis(-180, Vector3.up);
            while (t < 1)
            {
                Camera.main.transform.rotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0, 1, t));
                t += 0.01f;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.5f);

        m_VideoPlayer.Play();
        {
            float t = 0;
            while (t <= 1)
            {
                t += 0.03f;
                var color = m_VideoPanel.GetComponent<MeshRenderer>().material.color;
                color.a = t;
                m_VideoPanel.GetComponent<MeshRenderer>().material.color = color;
                yield return new WaitForEndOfFrame();
            }
        }

        //Sound.PlaySe("ScreenUp");

        yield return new WaitForSeconds(1.0f);

        m_sceneChanger.SetActive(true);

        while (m_VideoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        m_sceneChanger.GetComponent<PushButtonToScenechange>().SceneChange();

    }
}
