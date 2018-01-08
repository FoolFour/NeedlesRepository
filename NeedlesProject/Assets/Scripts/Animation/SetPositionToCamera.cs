using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionToCamera : StateMachineBehaviour {

    [Header("カメラからの相対値")]
    [SerializeField]
    Vector3 position;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos += position;
        animator.transform.position = cameraPos;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos += position;
        animator.transform.position = cameraPos;
    }
}
