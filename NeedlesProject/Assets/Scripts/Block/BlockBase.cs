﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour {

    //自分がhitするブロックかどうか
    public bool isHitBlock;

    /// <summary>
    /// 刺さったときの判定
    /// </summary>
    /// <param name="HitPoint"></param>
    protected virtual void StickEnter()
    {

    }

    /// <summary>
    /// 抜ける時の判定
    /// </summary>
    protected virtual void StickExit()
    {

    }
}