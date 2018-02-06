using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCanvas : MonoBehaviour
{

    VideoPlayer m_VideoPlayer;
    RectTransform m_RectTransform;

    // Use this for initialization
    void Start()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();
        m_RectTransform = GetComponent<RectTransform>();
        StartCoroutine(VideoStart());
    }

    IEnumerator VideoStart()
    {
        m_VideoPlayer.Play();
        yield return new WaitForSeconds(1.0f);
        {
            //videoを動かす
            {
                float t = 0;
                var posfrom = m_RectTransform.localPosition;
                var posto = new Vector3(-400, 200, 0);

                var scalefrom = m_RectTransform.localScale;
                var scaleto = new Vector3(0.6f, 0.6f, 1.0f);
                while (t <= 1)
                {
                    GamePad.isButtonLock = true;
                    t += 0.05f;
                    m_RectTransform.localPosition = Vector3.Lerp(posfrom, posto, Mathf.SmoothStep(0, 1, t));
                    m_RectTransform.localScale = Vector3.Lerp(scalefrom, scaleto, Mathf.SmoothStep(0, 1, t));
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        yield return new WaitForSeconds(1.0f);
        GamePad.isButtonLock = false;
    }
}
