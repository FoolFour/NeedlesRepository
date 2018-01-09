using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SoundPlay : MonoBehaviour {

    public AudioClip Bubble_Break;
    public AudioClip Bubble_Long;
    public AudioClip Bubble_Heavy_Short;   
    public AudioClip Bubble_Middle_Short;

    public int rand;

    // Use this for initialization
    void Start () {
       
     
    }
	
	// Update is called once per frame
	void Update () {
        rand = Random.Range(1, 100);

        if (rand%20==0)
        {
            GetComponent<AudioSource>().PlayOneShot(Bubble_Break);
        }

        if (rand % 15 == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(Bubble_Heavy_Short);
        }

        if (rand % 10 == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(Bubble_Middle_Short);
        }
    }
}
