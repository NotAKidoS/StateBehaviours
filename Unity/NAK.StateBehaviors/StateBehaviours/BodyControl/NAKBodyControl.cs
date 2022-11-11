using System;
using System.Collections.Generic;
using UnityEngine;

namespace NAK.StateBehaviors.BodyControl
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
            /**
            if ((int)TrackingAll > 0)
                BodySystem.TrackingEnabled = (TrackingAll == ControlMode.Enabled);

            if ((int)TrackingLeftArm > 0)
                BodySystem.TrackingLeftArmEnabled = (TrackingLeftArm == ControlMode.Enabled);

            if ((int)TrackingRightArm > 0)
                BodySystem.TrackingRightArmEnabled = (TrackingRightArm == ControlMode.Enabled);

            if ((int)TrackingLeftLeg > 0)
                BodySystem.TrackingLeftLegEnabled = (TrackingLeftLeg == ControlMode.Enabled);

            if ((int)TrackingRightLeg > 0)
                BodySystem.TrackingRightLegEnabled = (TrackingRightLeg == ControlMode.Enabled);

            if ((int)TrackingLocomotion > 0)
                BodySystem.TrackingLocomotionEnabled = (TrackingLocomotion == ControlMode.Enabled);
            **/
        }
    }
}