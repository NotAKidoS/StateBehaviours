using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviors.ParameterDriver
{
    public class NAKParameterDriver : NAKStateBehaviour
    {
        //lists of custom classes in mod dll dont deserialize properly, so uh... 
        public List<NAKParameterDriverEntry> stateEnter_Entries = new List<NAKParameterDriverEntry>();
        public List<NAKParameterDriverEntry> stateExit_Entries = new List<NAKParameterDriverEntry>();
        public List<NAKParameterDriverEntryUpdate> stateUpdate_Entries = new List<NAKParameterDriverEntryUpdate>();

        //i plan to dump the above data here, as list of custom class doesnt deserialize right from mod dll
        //or maybe i should just use these directly...???
        public NAKParamData_Simple stateEnter_data;
        public NAKParamData_Simple stateExit_data;
        public NAKParamData_Simple stateUpdate_data;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunEntries(animator, stateEnter_Entries);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunEntries(animator, stateExit_Entries);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunUpdateEntries(animator, stateUpdate_Entries, stateInfo);
        }

        private void RunEntries(Animator animator, List<NAKParameterDriverEntry> entries)
        {
            if (entries.Count() == 0) return;

            for (int i = 0; i < entries.Count(); i++)
            {
                var entry = entries[i];
                switch (entry.driverType)
                {
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case NAKParameterDriverEntry.DriverType.Set:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, entry.settingValue);
                        break;
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case NAKParameterDriverEntry.DriverType.Add:
                        float addedValue = getAddedParameterAsFloat(animator, entry.settingName, entry.settingValue);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, entry.settingValue);
                        break;
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case NAKParameterDriverEntry.DriverType.Random:
                        float randomValue = Random.Range(entry.valMin, entry.valMax);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, randomValue);
                        break;
                    //supports float, int, bool
                    case NAKParameterDriverEntry.DriverType.Copy:
                        float newValue = getAdjustedParameterAsFloat(animator, entry.sourceName, entry.convertRange, entry.srcMin, entry.srcMax, entry.dstMin, entry.dstMax);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, newValue);
                        break;
                    default:
                        break;
                }
            }
        }

        private void RunUpdateEntries(Animator animator, List<NAKParameterDriverEntryUpdate> entries, AnimatorStateInfo stateInfo)
        {
            if (entries.Count == 0) return;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                switch (entry.driverType)
                {
                    case NAKParameterDriverEntryUpdate.DriverType.PlayTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, stateInfo.normalizedTime);
                        break;
                    case NAKParameterDriverEntryUpdate.DriverType.NormalizedTime:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, stateInfo.normalizedTime % 1);
                        break;
                    case NAKParameterDriverEntryUpdate.DriverType.Lerp:
                        float smooth = getLerpedParameter(animator, entry.settingName, entry.lerpSource, entry.lerpSpeed);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(entry.settingName, smooth);
                        break;
                    default:
                        break;
                }
            }
        }

        internal float getAddedParameterAsFloat(Animator animator, string sourceName, float addValue)
        {
            float curValue = 0;
            AnimatorControllerParameterType type = LocalAnimatorManager.Instance.GetAnimatorParameterType(sourceName);
            switch (type)
            {
            case AnimatorControllerParameterType.Float:
                curValue = animator.GetFloat(sourceName);
                break;
            case AnimatorControllerParameterType.Int:
                curValue = (float)animator.GetInteger(sourceName);
                break;
            case AnimatorControllerParameterType.Bool:
                curValue = animator.GetBool(sourceName) ? 1.0f : 0.0f;
                break;
            case AnimatorControllerParameterType.Trigger:
                break;
            }
            return curValue+addValue;
        }

        //returns the sourceName parameter after its been modified as a float
        //taken from https://github.com/lyuma/Av3Emulator - MIT License
        internal float getAdjustedParameterAsFloat(Animator animator, string sourceName, bool convertRange=false, float srcMin=0.0f, float srcMax=0.0f, float dstMin=0.0f, float dstMax=0.0f) 
        {
            float newValue = 0;
            AnimatorControllerParameterType type = LocalAnimatorManager.Instance.GetAnimatorParameterType(sourceName);
            switch (type)
            {
            case AnimatorControllerParameterType.Float:
                newValue = animator.GetFloat(sourceName);
                break;
            case AnimatorControllerParameterType.Int:
                newValue = (float)animator.GetInteger(sourceName);
                break;
            case AnimatorControllerParameterType.Bool:
                newValue = animator.GetBool(sourceName) ? 1.0f : 0.0f;
                break;
            case AnimatorControllerParameterType.Trigger:
                break;
            }
            if (convertRange) {
                if (dstMax != dstMin) {
                    newValue = Mathf.Lerp(dstMin, dstMax, Mathf.Clamp01(Mathf.InverseLerp(srcMin, srcMax, newValue)));
                } else {
                    newValue = dstMin;
                }
            }
            return newValue;
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

    [Serializable]
    public class NAKParameterDriverEntry
    {
        //set & add
        public DriverType driverType = DriverType.Set;
        //public DriverMode driverMode = DriverMode.OnEnter;
        public string settingName;
        public float settingValue;

        //random
        public float chance;
        public float valMin;
        public float valMax;
        
        //copy
        public string sourceName;
        //copy + convert range
        public bool convertRange;
        public float srcMin;
        public float srcMax;
        public float dstMin;
        public float dstMax;

        //driver types for OnEnter or OnExit
        public enum DriverType
        {
            Set = 0,
            Add,
            Random,
            Copy,
        }
    }

    [Serializable]
    public class NAKParameterDriverEntryUpdate
    {
        //theres only few things that seem "ok" for OnUpdate...
        public DriverType driverType = DriverType.PlayTime;
        public string settingName;

        //lerp
        public string lerpSource;
        public float lerpSpeed;

        //driver types for OnUpdate
        public enum DriverType
        {
            PlayTime = 0,
            NormalizedTime,
            Lerp,
        }
    }
}