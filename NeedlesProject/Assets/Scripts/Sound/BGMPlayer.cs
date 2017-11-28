using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour {

    public string m_bgmKey;

	// Use this for initialization
	void Start ()
    {
        Sound.PlayBgm(m_bgmKey);	
	}
}
