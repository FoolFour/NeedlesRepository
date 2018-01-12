using UnityEngine;

public class SetTriggerOnStateEnter : StateMachineBehaviour
{
    [SerializeField]
    string triggerName;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetTrigger(triggerName);
    }
}
