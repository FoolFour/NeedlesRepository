using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoin : BlockBase,IRespawnMessage
{
    [SerializeField, Tooltip("プレイヤー本体と当たる")]
    public bool CheckPlayer;
    [SerializeField, Tooltip("アームと当たる")]
    public bool CheckArm;

    private RemoveComponent m_RCom;
    private bool isDead = false;

    
    public GameObject Coineffect_obj;   //
    public Transform death_pos;         //

    public void Start()
    {
        m_RCom = GetComponent<RemoveComponent>();
        isDead = false;
        death_pos = this.gameObject.transform;  //
    }

    public override void StickEnter(GameObject arm)
    {
        if (CheckArm)
        {
            //particle生成
            GameObject Coineffect =
                Instantiate(Coineffect_obj, death_pos) as GameObject;//
                                                                     //再生終わったら消す
            Destroy(Coineffect, 5f);//

            m_RCom.SwitchActive(false);
            Sound.PlaySe("CoinGet");
            isDead = true;
        }
        base.StickEnter(arm);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CheckPlayer)
        {
            if (other.CompareTag("Player"))
            {
                //particle生成
                GameObject Coineffect =
                    Instantiate(Coineffect_obj, death_pos) as GameObject;//
                //再生終わったら消す
                Destroy(Coineffect, 5f);//

                isDead = true;
                m_RCom.SwitchActive(false);
                Sound.PlaySe("CoinGet");
            }
        }
        if (CheckArm)
        {
            if (other.CompareTag("PlayerArm"))
            {
                //particle生成
                GameObject Coineffect = 
                    Instantiate(Coineffect_obj, death_pos) as GameObject;//
                //再生終わったら消す
                Destroy(Coineffect, 5f);//

                isDead = true;
                m_RCom.SwitchActive(false);
                Sound.PlaySe("CoinGet");
            }
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void RespawnInit()
    {

        m_RCom.SwitchActive(true);
        isDead = false;
    }
}
