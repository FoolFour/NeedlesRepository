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
    //リスポーンしてるかのフラグ
    public bool p_respawn_flag;
    // GravityChangeArea
    public GameObject GCA_obj;

    void Start()
    {
        //AudioSourceがなければ追加
        audioSource_ = (AudioSource)gameObject.GetComponent<AudioSource>();
        if (audioSource_ == null) audioSource_ = (AudioSource)gameObject.AddComponent<AudioSource>();
        GCA_obj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        p_respawn_flag = GameObject.Find("Player(Clone)").GetComponent<Player>().respawn;
    }

    public void OnTriggerEnter(Collider other)
    {
        //リスポーンしてるなら音を出さない
        if (p_respawn_flag == false && other.gameObject.tag == "Player")
        {
            //SE再生
            GetComponent<AudioSource>().PlayOneShot(SE_Waterdrop);
            //位置所得
            Transform hitPos = other.gameObject.transform;
            //particle生成
            Splashes_obj = Instantiate(Splashes_obj, hitPos) as GameObject;
            //GravityChangeAreaの子に入れる
            Splashes_obj.transform.parent = GCA_obj.transform;
            //再生終わったら消す
            Destroy(Splashes_obj, 5f);
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
           　Splashes_obj = Instantiate(Splashes_obj, hitPos) as GameObject;
            //GravityChangeAreaの子に入れる
            Splashes_obj.transform.parent = GCA_obj.transform;
            //再生終わったら消す
            Destroy(Splashes_obj, 5f);
        }
    }
}