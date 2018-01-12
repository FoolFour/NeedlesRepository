using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SoundPlay : MonoBehaviour {

    //SE変数
    public AudioClip SE_Bubble_Break;
    public AudioClip SE_Bubble_Long;
    public AudioClip SE_Bubble_Heavy_Short;   
    public AudioClip SE_Bubble_Middle_Short;
    public AudioClip SE_Waterdrop;

    //ランダム
    public int rand;

    //泡を使う場合のフラグ
    public bool bubbleflag;
    public bool waterdorop;


    // Use this for initialization
    void Start () {
        if (waterdorop == true)
        {
            GetComponent<AudioSource>().PlayOneShot(SE_Waterdrop);
            //waterdorop = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        rand = Random.Range(1, 100);

        if (bubbleflag == true)
        {
            if (rand % 20 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Break);
            }

            if (rand % 15 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Heavy_Short);
            }

            if (rand % 10 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Middle_Short);
            }
        }
    }
}