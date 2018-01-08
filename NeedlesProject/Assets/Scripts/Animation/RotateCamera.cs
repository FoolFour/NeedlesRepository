using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : StateMachineBehaviour
{
    [SerializeField]
    Vector3 rotate;

    [SerializeField]
    float time;

    [SerializeField]
    AnimationCurve animationCurve;

    float timer;

    Quaternion startRotate;
    Quaternion endRotate;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startRotate = Camera.main.transform.rotation;
        endRotate = Quaternion.Euler(rotate);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        float amount = animationCurve.Evaluate( Mathf.Min(timer / time, 1.0f));
        Camera.main.transform.rotation = Quaternion.Slerp(startRotate, endRotate, amount);
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
