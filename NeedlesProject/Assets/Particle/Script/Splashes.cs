using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Splashes : MonoBehaviour {
    
    //AudioSource
    private AudioSource audioSource_;
    //入水音
    public AudioClip SE_Waterdrop;
    //入水Effect
    public ParticleSystem Splashes_Effect;
    //プレイヤーが入った位置
    public Vector3 Enterposition;
    //生成位置
    //public GameObject Splashes_Effect_pos;

    // Use this for initialization
    void Start () {
        //AudioSourceがなければ追加
        audioSource_ = (AudioSource)gameObject.GetComponent<AudioSource>();
        if (audioSource_ == null)
        {
            audioSource_ = (AudioSource)gameObject.AddComponent<AudioSource>();
        }
}

    // Update is called once per frame
    void Update () {	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().PlayOneShot(SE_Waterdrop);
            Enterposition = other.gameObject.transform.position;

            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);

            ParticleSystem Effect_pos = Instantiate(Splashes_Effect,hitPos,transform.localRotation);
            Destroy(Effect_pos, 5f);
        }
    }
}
