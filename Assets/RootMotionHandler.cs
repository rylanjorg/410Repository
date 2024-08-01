using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionHandler : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        float speed = animator.GetFloat("Speed");
        animator.applyRootMotion = true;
        if(speed < 3.0f) 
        {
            //animator.applyRootMotion = true;
            //animator.SetLayerWeight(layerIndex, 1); 
        }
        else if(speed > 3.0f) 
        {
            //animator.applyRootMotion = false;
            //animator.SetLayerWeight(layerIndex, 0); // Disable the animation
        }
        
         
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.applyRootMotion = false;
       //animator.SetLayerWeight(layerIndex, 1); // Fully enable the animation
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
