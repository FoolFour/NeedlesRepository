using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SoundPlay : MonoBehaviour
{
    //SE変数
    public AudioClip SE_Bubble_Break;
    public AudioClip SE_Bubble_Long;
    public AudioClip SE_Bubble_Heavy_Short;
    public AudioClip SE_Bubble_Middle_Short;
 
    int rand100;
    int rand250;  
    int rand500;

    //ランダム
    float rand;

    //泡を使う場合のフラグ
    public bool bubbleflag;
    public bool waterdorop;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (bubbleflag == true)
        {
            rand = Random.Range(1, 500);

            if ((int)rand % 100 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Middle_Short);
            }
            else if ((int)rand % 250 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Heavy_Short);
            }           
            else if ((int)rand % 500 == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(SE_Bubble_Break);
            }
           
        }
    }
}