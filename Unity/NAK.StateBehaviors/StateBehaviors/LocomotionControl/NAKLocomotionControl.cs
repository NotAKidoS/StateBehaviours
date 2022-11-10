using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviors.LocomotionControl
{
    public class NAKLocomotionControl : NAKStateBehaviour
    {
        //movement locks
        public bool shouldMove;
        public bool shouldRotate;

        //movement input
        public bool enableFlight;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }
    }
}