using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCamera : StateMachineBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    float time;

    float timer;

    float startY;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(time < 0)
        {
            time = 0.001f;
        }
        startY = Camera.main.transform.position.y;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        float y = curve.Evaluate(timer / time);

        Vector3 pos = Camera.main.transform.position;
        pos.y = startY + y;
        Camera.main.transform.position = pos;
    }
}
