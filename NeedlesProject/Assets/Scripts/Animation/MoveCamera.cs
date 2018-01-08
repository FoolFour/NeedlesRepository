using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : StateMachineBehaviour
{
    [SerializeField]
    Vector3 moveTo;

    [SerializeField]
    float time;

    [SerializeField]
    AnimationCurve animationCurve;

    float timer;

    Vector3 startPosition;
    Vector3 endPosition;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startPosition = Camera.main.transform.position;
        endPosition   = startPosition + moveTo;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        float amount = animationCurve.Evaluate( Mathf.Min(timer / time, 1.0f));
        Camera.main.transform.position = Vector3.Lerp(startPosition, endPosition, amount);
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
