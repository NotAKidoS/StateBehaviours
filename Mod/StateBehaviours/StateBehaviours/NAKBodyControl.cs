using ABI_RC.Systems.IK;
using ABI_RC.Systems.IK.SubSystems;
using UnityEngine;
using RootMotion.FinalIK;
using ABI_RC.Core.Player;

namespace NAK.StateBehaviours.BodyControl
{
    public class NAKBodyControl : NAKStateBehaviour
    {
        public UpdateModes updateMode;
        public ControlMode TrackingAll;
        public ControlMode TrackingHead;
        public ControlMode TrackingLeftArm;
        public ControlMode TrackingRightArm;
        public ControlMode TrackingLeftLeg;
        public ControlMode TrackingRightLeg;
        public ControlMode TrackingLocomotion;

        public enum ControlMode
        {
            Ignore,
            Enabled,
            Disabled,
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateMode == UpdateModes.OnEnter)
                ToggleIKSystem();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (updateMode == UpdateModes.OnExit)
                ToggleIKSystem();
        }

        internal void ToggleIKSystem()
        {

            if ((int)TrackingLocomotion > 0)
                BodySystem.TrackingLocomotionEnabled = (TrackingLocomotion == ControlMode.Enabled);

            if ((int)TrackingAll > 0)
            {
                BodySystem.TrackingEnabled = (TrackingAll == ControlMode.Enabled);
                ToggleHeadIK(BodySystem.TrackingEnabled);
                if (BodySystem.TrackingEnabled) return;
            }

            if ((int)TrackingHead > 0)
            {
                ToggleHeadIK(TrackingHead == ControlMode.Enabled);
            }

            //the game doesnt check if leftArm and rightArm targets are null- but they do for legs :<
            if ((int)TrackingLeftArm > 0)
            {
                if (IKSystem.vrik.solver.leftArm.target != null)
                    BodySystem.TrackingLeftArmEnabled = (TrackingLeftArm == ControlMode.Enabled);
            }

            if ((int)TrackingRightArm > 0)
            {
                if (IKSystem.vrik.solver.rightArm.target != null)
                    BodySystem.TrackingRightArmEnabled = (TrackingRightArm == ControlMode.Enabled);
            }

            if ((int)TrackingLeftLeg > 0)
                BodySystem.TrackingLeftLegEnabled = (TrackingLeftLeg == ControlMode.Enabled);

            if ((int)TrackingRightLeg > 0)
                BodySystem.TrackingRightLegEnabled = (TrackingRightLeg == ControlMode.Enabled);
        }

        internal void ToggleHeadIK(bool flag)
        {
            //VRIK head tracking
            if (IKSystem.vrik.solver.spine.headTarget != null)
                IKSystem.vrik.solver.spine.headClampWeight = (flag ? 0f : 1f);
            //Desktop head tracking
            LookAtIK lookIK = PlayerSetup.Instance._avatar.GetComponent<LookAtIK>();
            if (lookIK != null)
                lookIK.solver.IKPositionWeight = (flag ? 1f : 0f);
        }
    }
}