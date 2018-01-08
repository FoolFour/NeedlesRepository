using UnityEngine;

public class SetBoolOnStateEnter : StateMachineBehaviour
{
    [SerializeField]
    string boolName;

    [SerializeField]
    bool   condition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, condition);
    }
}
