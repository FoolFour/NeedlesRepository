using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyBlock : BlockBase
{
    public GameObject m_deadEffectPrefab;
    public override void StickEnter(GameObject arm)
    {
        var go = (GameObject)Instantiate(m_deadEffectPrefab, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        GetComponent<RemoveComponent>().SwitchActive(false);
        GetComponent<HomingEnemy>().m_state = HomingEnemy.State.Dead;
        base.StickEnter(arm);
    }
}
