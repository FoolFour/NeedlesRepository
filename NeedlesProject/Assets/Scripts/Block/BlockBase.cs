﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour {

    //自分がhitするブロックかどうか
    public bool isHitBlock;

    /// <summary>
    /// 刺さったときの判定
    /// </summary>
    /// <param name="arm">どちらかの腕</param>
    public virtual void StickEnter(GameObject arm)
    {

    }

    /// <summary>
    /// 抜ける時の判定
    /// </summary>
    public virtual void StickExit()
    {

    }
}
