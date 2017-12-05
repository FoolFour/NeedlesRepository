using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerDead_effect : MonoBehaviour {

    //プレイヤーの中のモデル
    public ParticleSystem PlayerModel;
    //プレイヤーの右腕のモデル
    public ParticleSystem Player_Arm_Model_R;
    //プレイヤーの左腕のモデル
    public ParticleSystem Player_Arm_Model_L;
    //プレイヤーの右手のモデル
    public ParticleSystem Player_Hand_Model_R;
    //プレイヤーの左手のモデル
    public ParticleSystem Player_Hand_Model_L;
    //プレイヤーの本体の下のモデル
    public ParticleSystem PlayerBody_Down;
    //プレイヤーの本体の上のモデル
    public ParticleSystem PlayerBody_Up;
    //爆発のケムリ
    public ParticleSystem Explos;

    //デバッグ用
    public bool debugflag;

    // Use this for initialization
    void Start () {
        debugflag = false;
        ParticleStop();
    }
	
	// Update is called once per frame
	void Update () {
        if (debugflag == true)
        {
            ParticleStart();
            if (debugflag == true)
            {
                debugflag = false;
            }
        }
        else
        {
            ParticleStop();
        }
    }
    //Particleの開始
    public void ParticleStart()
    {
        PlayerModel.Play();
        Player_Arm_Model_R.Play();
        Player_Arm_Model_L.Play();
        Player_Hand_Model_R.Play();
        Player_Hand_Model_L.Play();
        PlayerBody_Down.Play();
        PlayerBody_Up.Play();
        Explos.Play();
    }

    //Particleの停止
    public void ParticleStop()
    {
        PlayerModel.Stop();
        Player_Arm_Model_R.Stop();
        Player_Arm_Model_L.Stop();
        Player_Hand_Model_R.Stop();
        Player_Hand_Model_L.Stop();
        PlayerBody_Down.Stop();
        PlayerBody_Up.Stop();
        Explos.Stop();
    }
}
