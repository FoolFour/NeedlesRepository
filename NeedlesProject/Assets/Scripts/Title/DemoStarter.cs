using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoStarter : MonoBehaviour {

    private float m_Time = 0;
    public float DEMO_START_SECOND = 60;
	// Update is called once per frame
	void Update ()
    {
        m_Time += Time.deltaTime;
        if(Input.anyKeyDown)
        {
            m_Time = 0;
        }

        if(m_Time > DEMO_START_SECOND)
        {
            Sound.StopBgm();
            SceneManager.LoadScene("DemoScene");
        }
	}
}
