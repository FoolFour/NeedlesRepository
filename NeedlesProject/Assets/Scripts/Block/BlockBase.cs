using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour {

    //自分がhitするブロックかどうか
    public bool isHitBlock;

    /// <summary>
    /// 触った時の判定
    /// </summary>
    /// <param name="arm">どちらかの腕</param>
    public virtual void StickEnter(GameObject arm)
    {

    }
    /// <summary>
    /// 刺さった時の判定
    /// </summary>
    /// <param name="arm">どちらかの腕</param>
    public virtual void StickHit(GameObject arm, GameObject stickpoint)
    {

    }

    /// <summary>
    /// 刺さっている時の処理
    /// </summary>
    /// <param name="arm"></param>
    public virtual void StickStay(GameObject arm,GameObject stickpoint)
    {

    }

    /// <summary>
    /// 抜ける時の判定
    /// </summary>
    public virtual void StickExit()
    {

    }

    public virtual void Init()
    {

    }

    public void StickPointOut()
    {
        var sp = transform.Find("StickPoint(Clone)");
        if (sp)
        sp.parent = null;
    }
}
