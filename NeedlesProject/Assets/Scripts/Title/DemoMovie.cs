using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DemoMovie : MonoBehaviour {

    private VideoPlayer m_Vp;
    private GameObject m_Texture;

    bool isPlaying = false;

	// Use this for initialization
	void Start ()
    {
        m_Vp = transform.Find("Video").GetComponent<VideoPlayer>();
        m_Texture = transform.Find("Texture").gameObject;

        m_Vp.Play();
        isPlaying = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.anyKeyDown || !isPlaying)
        {
            Sound.PlayBgm("Title");
            Sound.ChangeBgmVolume(1.0f);
            SceneManager.LoadScene("title");
        }
        isPlaying = m_Vp.isPlaying;
	}
}
