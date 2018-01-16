using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamaera : StateMachineBehaviour 
{
    float startXPos;
    float startZPos;

    static readonly float endZPos = -15;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startXPos = Camera.main.transform.position.x;
        startZPos = Camera.main.transform.position.z;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 pos = Camera.main.transform.position;
        pos.x = Mathf.SmoothStep(startXPos, animator.transform.position.x, stateInfo.normalizedTime);
        pos.z = Mathf.SmoothStep(startZPos, endZPos, stateInfo.normalizedTime);
        Camera.main.transform.position = pos;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
