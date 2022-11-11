using System;
using System.Collections.Generic;
using UnityEngine;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviours.ParameterDriver
{
    public class NAKUpdateDriver : NAKStateBehaviour
    {

        //theres only few things that seem "ok" for OnUpdate...
        public List<DriverTypes> driverType;
        //base
        public List<string> settingName;
        public List<float> settingValue;
        //lerp
        public List<string> sourceName;
        public List<float> sourceValue;

        //driver types for OnUpdate
        public enum DriverTypes
        {
            PlayTime = 0,
            NormalizedTime,
            Lerp,
        }

        public void Awake()
        {
            //punishment
            if (settingName.Count == 0)
                Destroy(this);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunUpdateEntries(animator, stateInfo);
        }

        private void RunUpdateEntries(Animator animator, AnimatorStateInfo stateInfo)
        {
            for (int i = 0; i < settingName.Count; i++)
            {
                if (settingName[i] == "") continue;
                switch (driverType[i])
                {
                    case DriverTypes.PlayTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], stateInfo.normalizedTime);
                        break;
                    case DriverTypes.NormalizedTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], stateInfo.normalizedTime % 1);
                        break;
                    case DriverTypes.Lerp:
                        float smooth = getLerpedParameter(animator, settingName[i], sourceName[i], sourceValue[i]);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], smooth);
                        break;
                    default:
                        break;
                }
            }
        }

        //*ima be honest i have no fucking idea how to do lerp right so im just winging it*
        internal float getLerpedParameter(Animator animator, string settingName, string sourceName, float sourceValue)
        {
            float settingVal = animator.GetFloat(settingName);
            float sourceVal = animator.GetFloat(sourceName);
            if (Math.Abs(sourceVal - settingVal) < 0.01f)
            {
                return sourceVal;
            }
            return Mathf.Lerp(settingVal, sourceVal, sourceValue * Time.deltaTime);
        }
    }
}