using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductionSwitch : MonoBehaviour
{
    //プラスとマイナスのブロック
    public GameObject plusblock;
    public GameObject minusblock;
    //動くオブジェクト
    public GameObject DoorR;
    public GameObject DoorL;
    //移動のスピード   
    public float movespeed = 0.1f;
    //移動した距離の保存
    public float movePoint = 0.0f;
    //移動する距離の限界値
    public float moveMax;
    //電導スイッチのフラグ
    [SerializeField]
    private bool plusflag;
    [SerializeField]
    private bool minusflag;

    public enum DoorState
    {
        MOVE,
        STOP,
        IDOL
    }
    public DoorState Doorstate;

    void Start()
    {
        moveMax = 10;
    }

    void Update()
    {
        //フラグ確認
        plusflag = plusblock.GetComponent<ConductionBlocks>().blockSwitch;
        minusflag = minusblock.GetComponent<ConductionBlocks>().blockSwitch;

        //両方のブロックに当たっているなら
        if ((plusflag == true && minusflag == true) && Doorstate != DoorState.STOP)
        {
            //扉が開く
            Doorstate = DoorState.MOVE;
            movePoint += movespeed * Time.deltaTime;
            //
            DoorR.transform.position += new Vector3(movespeed, 0f, 0f) * Time.deltaTime;
            DoorL.transform.position -= new Vector3(movespeed, 0f, 0f) * Time.deltaTime;

            if (movePoint >= moveMax)
            {
                Doorstate = DoorState.STOP;
            }
        }

        //片方離れている場合
        if (Doorstate == DoorState.MOVE || Doorstate == DoorState.STOP)
        {
            if ((plusflag == false || minusflag == false))
            {
                movePoint -= movespeed * Time.deltaTime;
                //扉が閉まるする
                Doorstate = DoorState.MOVE;
                DoorR.transform.position -= new Vector3(movespeed, 0f, 0f) * Time.deltaTime;
                DoorL.transform.position += new Vector3(movespeed, 0f, 0f) * Time.deltaTime;
            }
        }

        //初期位置に戻ったなら動かないようにする
        if (movePoint < 0.0f)
        {
            Doorstate = DoorState.IDOL;
        }
    }
}