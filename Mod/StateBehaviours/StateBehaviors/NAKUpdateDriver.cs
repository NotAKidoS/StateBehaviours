using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviors.ParameterDriver
{
    public class NAKUpdateDriver : NAKStateBehaviour
    {

        //theres only few things that seem "ok" for OnUpdate...
        public List<DriverType> driverType;
        public List<string> settingName;
        //lerp
        public List<string> lerpSource;
        public List<float> lerpSpeed;

        //driver types for OnUpdate
        public enum DriverType
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
                    case DriverType.PlayTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], stateInfo.normalizedTime);
                        break;
                    case DriverType.NormalizedTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], stateInfo.normalizedTime % 1);
                        break;
                    case DriverType.Lerp:
                        float smooth = getLerpedParameter(animator, settingName[i], lerpSource[i], lerpSpeed[i]);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], smooth);
                        break;
                    default:
                        break;
                }
            }
        }

        //*ima be honest i have no fucking idea how to do lerp right so im just winging it*
        internal float getLerpedParameter(Animator animator, string settingName, string sourceName, float lerpSpeed)
        {
            float settingVal = animator.GetFloat(settingName);
            float sourceVal = animator.GetFloat(sourceName);
            if (Math.Abs(sourceVal-settingVal) < 0.01f)
            {
                return sourceVal;
            }
            return Mathf.Lerp(settingVal, sourceVal, lerpSpeed*Time.deltaTime );
        }
    }
}