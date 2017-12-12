using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyBlock : BlockBase
{
    public override void StickEnter(GameObject arm)
    {
        GetComponent<RemoveComponent>().SwitchActive(false);
        GetComponent<HomingEnemy>().m_state = HomingEnemy.State.Dead;
        base.StickEnter(arm);
    }
}
