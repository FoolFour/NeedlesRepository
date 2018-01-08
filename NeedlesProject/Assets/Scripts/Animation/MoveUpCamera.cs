using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCamera : StateMachineBehaviour
{
    [SerializeField]
    float moveSpeed = 1;

    float time;
    float startY;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startY = Camera.main.transform.position.y;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime * moveSpeed;
        float y = (time - 1) * (time - 1) + -1;

        Vector3 pos = Camera.main.transform.position;
        pos.y = startY + y;
        Camera.main.transform.position = pos;
    }
}
