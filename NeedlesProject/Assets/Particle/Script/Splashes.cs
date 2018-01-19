using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Splashes : MonoBehaviour
{
    //AudioSource
    private AudioSource audioSource_;
    //入水音
    public AudioClip SE_Waterdrop;
    //入水Effect
    public ParticleSystem Splashes_Effect;
    //ゲームオブジェ
    public GameObject Splashes_obj;
    //
    public bool p_respawn_flag;    

    // Use this for initialization
    void Start()
    {
        //AudioSourceがなければ追加
        audioSource_ = (AudioSource)gameObject.GetComponent<AudioSource>();
        if (audioSource_ == null)  audioSource_ = (AudioSource)gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        p_respawn_flag =GameObject.Find("Player(Clone)").GetComponent<Player>().respawn;
    }

    public void OnTriggerEnter(Collider other)
    {
            //リスポーンしてるなら音を出さない
            if (p_respawn_flag == false&& other.gameObject.tag == "Player") {
                Debug.Log(other.gameObject.tag);
                //SE再生
                GetComponent<AudioSource>().PlayOneShot(SE_Waterdrop);
                //位置所得
                Transform hitPos = other.gameObject.transform;
                //particle生成
                GameObject splashes = Instantiate(Splashes_obj, hitPos) as GameObject;
                //再生終わったら消す
                Destroy(splashes, 5f);            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SE再生
            GetComponent<AudioSource>().PlayOneShot(SE_Waterdrop);
            //位置所得
            Transform hitPos = other.gameObject.transform;
            //particle生成
            GameObject splashes = Instantiate(Splashes_obj, hitPos) as GameObject;
            //再生終わったら消す
            Destroy(splashes, 5f);
        }
    }
}