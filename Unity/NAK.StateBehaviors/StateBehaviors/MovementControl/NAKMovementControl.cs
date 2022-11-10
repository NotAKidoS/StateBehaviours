using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviors.MovementControl
{
    public class NAKMovementControl : NAKStateBehaviour
    {
        //movement locks
        public float movementX;
        public float movementY;
        public AnimationCurve curveX = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve curveY = AnimationCurve.Linear(0, 1, 1, 1);

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat("MovementX", curveX.Evaluate(stateInfo.normalizedTime));
            animator.SetFloat("MovementY", curveY.Evaluate(stateInfo.normalizedTime));
        }
    }
}