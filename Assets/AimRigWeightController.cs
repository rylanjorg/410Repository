using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class AimRigWeightController : StateMachineBehaviour
{
    [Range(0,1)] public float enterWeight;
    [Range(0,1)] public float exitWeight;
    [SerializeField] Rig rig;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Rig rig = GameObject.Find("PlayerRootContainer").GetComponent<PlayerIK>().rig;
        //float aimWeight = animator.GetFloat("AimWeight");
        //rig.weight = aimWeight;
        if(rig == null) 
        {
            Debug.LogError("Rig is null");
        }
        else
        {
            Debug.LogError("Rig is not null");
            rig.weight = enterWeight;
        }
        
        /*
        if(aimWeight > 0.0f) 
        {
            
            //animator.SetLayerWeight(layerIndex, 1); // Enable the animation
        }
        else if(aimWeight == 0.0f) 
        {
            //animator.SetLayerWeight(layerIndex, 0); // Disable the animation
        }    */
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Rig rig = GameObject.Find("PlayerRootContainer").GetComponent<PlayerIK>().rig;
        //float aimWeight = animator.GetFloat("AimWeight");
        //rig.weight = aimWeight;
        if(rig == null) 
        {
            Debug.LogError("Rig is null");
        }
        else
        {
            Debug.LogError("Rig is not null");
           rig.weight = exitWeight;
        }
        
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

