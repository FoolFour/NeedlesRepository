using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OpeningController : MonoBehaviour
{

    public Image m_blackBoard;
    public Image m_teamLogo;
    public Transform m_ufo;
    public Image m_gameLogo;
    public GameObject m_pressImage;
    public AudioSource m_RoketAudio;
    public VideoStarter m_VideoStarte;

    Coroutine eventCoroutine;
    bool Eventing = true;

    public void Start()
    {
        eventCoroutine = StartCoroutine(Opening());
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Eventing)
            {
                StopCoroutine(eventCoroutine);
                {
                    var c = m_teamLogo.color;
                    c.a = 0;
                    m_teamLogo.color = c;
                }

                {
                    var c = m_blackBoard.color;
                    c.a = 0;
                    m_blackBoard.color = c;
                }

                {
                    m_ufo.position = new Vector3(5, 3, 0.5f);
                    m_ufo.rotation = Quaternion.Euler(-30, 0, 30);
                    Camera.main.transform.rotation = Quaternion.identity;
                    m_RoketAudio.volume = 0.2f;
                }

                {
                    var c = m_gameLogo.color;
                    c.a = 1;
                    m_gameLogo.color = c;
                }

                m_pressImage.SetActive(true);
                Eventing = false;
                m_VideoStarte.enabled = true;
            }
        }
    }

    IEnumerator Opening()
    {
        yield return new WaitForSeconds(2f);
        //チームロゴの処理
        {
            while (m_teamLogo.color.a > 0)
            {
                var c = m_teamLogo.color;
                c.a -= 0.05f;
                m_teamLogo.color = c;
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(0.5f);
        //ブラックボードの処理
        {
            while (m_blackBoard.color.a > 0)
            {
                var c = m_blackBoard.color;
                c.a -= 0.1f;
                m_blackBoard.color = c;
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(2f);
        Eventing = true;
        //母船を動かす処理
        {
            float t = 0;
            var from = m_ufo.position;
            var to = new Vector3(5, 3, 0.5f);
            while (t <= 1)
            {
                m_ufo.position = Vector3.Lerp(from, to, t);
                t += 0.003f;
                yield return new WaitForEndOfFrame();
            }
            m_RoketAudio.volume = 0.2f;
        }
        //母船を回転させる処理
        {
            float t = 0;
            var Rfrom = m_ufo.rotation;
            var Rto = Quaternion.Euler(-30, 0, 30);
            while (t <= 1)
            {
                m_ufo.rotation = Quaternion.Slerp(Rfrom, Rto, t);
                t += 0.1f;
                yield return new WaitForEndOfFrame();
            }
        }

        //カメラを動かす処理
        {
            float t = 0;
            var from = Camera.main.transform.rotation;
            var to = Quaternion.identity;
            while (t <= 1)
            {
                Camera.main.transform.rotation = Quaternion.Slerp(from, to, t);
                t += 0.003f;
                yield return new WaitForEndOfFrame();
            }
        }

        //タイトルロゴを登場させる
        {
            while (m_gameLogo.color.a < 1)
            {
                var c = m_gameLogo.color;
                c.a += 0.1f;
                m_gameLogo.color = c;
                yield return new WaitForEndOfFrame();
            }
        }

        m_pressImage.SetActive(true);
        m_VideoStarte.enabled = true;
        Eventing = false;
    }
}
