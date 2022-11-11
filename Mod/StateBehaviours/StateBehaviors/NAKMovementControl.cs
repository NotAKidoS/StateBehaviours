using ABI_RC.Core.Savior;
using ABI_RC.Systems.MovementSystem;
using UnityEngine;

namespace NAK.StateBehaviors.MovementControl
{
    public class NAKMovementControl : NAKStateBehaviour
    {
        //movement locks
        public bool lockMovement;
        public AnimationCurve movementX = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve movementY = AnimationCurve.Linear(0, 1, 1, 1);

        private Vector2 initialMovement;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            initialMovement = CVRInputManager.Instance.movementVector;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (lockMovement)
            {
                CVRInputManager.Instance.movementVector = initialMovement;
                return;
            }
            Vector2 newMovement = new Vector2(movementX.Evaluate(stateInfo.normalizedTime), movementY.Evaluate(stateInfo.normalizedTime));
            CVRInputManager.Instance.movementVector = newMovement;
        }
    }
}