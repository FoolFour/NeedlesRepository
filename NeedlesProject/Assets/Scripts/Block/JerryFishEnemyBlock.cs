using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryFishEnemyBlock : BlockBase {

    public override void StickEnter(GameObject arm)
    {
        if (GetComponent<JerryFishEnemy>().m_state == JerryFishEnemy.State.Normal)
        {
            GetComponent<RemoveComponent>().SwitchActive(false);
            GetComponent<JerryFishEnemy>().m_state = JerryFishEnemy.State.Dead;
        }
        else if(GetComponent<JerryFishEnemy>().m_state == JerryFishEnemy.State.Shock)
        {
            var power = arm.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            arm.GetComponent<NeedleArm>().PlayerStan(power);
        }
    }
}
