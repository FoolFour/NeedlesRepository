using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoin : BlockBase
{
    [SerializeField, Tooltip("プレイヤー本体と当たる")]
    public bool CheckPlayer;
    [SerializeField, Tooltip("アームと当たる")]
    public bool CheckArm;

    public override void StickEnter(GameObject arm)
    {
        if (CheckArm)
        {
            Destroy(gameObject);
        }
        base.StickEnter(arm);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CheckPlayer)
        {
            if (other.CompareTag("Player")) Destroy(gameObject);
        }
        if (CheckArm)
        {
            if (other.CompareTag("PlayerArm")) Destroy(gameObject);
        }
    }
}
