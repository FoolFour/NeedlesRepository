﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Sound.LoadBgm("bgm", "test");
        Sound.PlayBgm("bgm");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}