using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayerAttack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.
    // The is used to set the isAttacking bool to false to ensure the animation state is changed as soon as the animation is over.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
    }
}
