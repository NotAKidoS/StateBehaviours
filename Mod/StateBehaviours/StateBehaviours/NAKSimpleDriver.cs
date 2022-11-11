using UnityEngine;
using Random = UnityEngine.Random;

//TODO: add error catching or verification of broken drivers/parameters

namespace NAK.StateBehaviours.ParameterDriver
{
    public class NAKSimpleDriver : NAKStateBehaviour
    {
        //lists of custom classes in mod dll dont deserialize properly, so uh...

        //OnEnter or OnExit
        public List<UpdateModes> updateMode;
        //set & add
        public List<DriverTypes> driverType;
        //public DriverMode driverMode = DriverMode.OnEnter;
        public List<string> settingName;
        public List<float> settingValue;
        //random
        public List<float> valMin;
        public List<float> valMax;
        //copy
        public List<string> sourceName;
        //copy + convert range
        public List<bool> convertRange;
        public List<float> srcMin;
        public List<float> srcMax;
        public List<float> dstMin;
        public List<float> dstMax;

        public enum DriverTypes
        {
            Set = 0,
            Add,
            Random,
            Copy,
        }

        public void Awake()
        {
            //punishment
            if (settingName.Count == 0)
                Destroy(this);

            //I could probably optimize stuff if i parsed the lists and assigned class subclass with different action
            //that way i dont need to check what driver type, parameter type, ect each run
            //and optimize OnEnter and OnExit check

            //... but not now- and not until i figure out why CVR will instantiate like 3 of these each???
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunEntries(animator, true);
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunEntries(animator, false);
        }

        private void RunEntries(Animator animator, bool onEnter)
        {
            for (int i = 0; i < settingName.Count; i++)
            {
                if (settingName[i] == "") continue;
                if ((updateMode[i] == UpdateModes.OnEnter) != onEnter) continue;
                switch (driverType[i])
                {
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case DriverTypes.Set:
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], settingValue[i]);
                        break;
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case DriverTypes.Add:
                        float addedValue = getAddedParameterAsFloat(animator, settingName[i], settingValue[i]);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], settingValue[i]);
                        break;
                    //supports float, int, bool, trigger (trigger doesnt reset)
                    case DriverTypes.Random:
                        float randomValue = getRandomParameterAsFloat(animator, settingName[i], settingValue[i], valMin[i], valMax[i]);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], randomValue);
                        break;
                    //supports float, int, bool
                    case DriverTypes.Copy:
                        float newValue = getAdjustedParameterAsFloat(animator, sourceName[i], convertRange[i], srcMin[i], srcMax[i], dstMin[i], dstMax[i]);
                        LocalAnimatorManager.Instance.SetAnimatorParameterFromFloat(settingName[i], newValue);
                        break;
                    default:
                        break;
                }
            }
        }

        internal float getAddedParameterAsFloat(Animator animator, string sourceName, float addValue)
        {
            float curValue = addValue;
            AnimatorControllerParameterType type = LocalAnimatorManager.Instance.GetAnimatorParameterType(sourceName);
            switch (type)
            {
                case AnimatorControllerParameterType.Float:
                    curValue += animator.GetFloat(sourceName);
                    break;
                case AnimatorControllerParameterType.Int:
                    curValue += (float)animator.GetInteger(sourceName);
                    break;
                case AnimatorControllerParameterType.Bool:
                    curValue += animator.GetBool(sourceName) ? 1.0f : 0.0f;
                    break;
                case AnimatorControllerParameterType.Trigger:
                    //there is no way to get a triggers current value
                    break;
            }
            return curValue;
        }

        internal float getRandomParameterAsFloat(Animator animator, string sourceName, float chance, float min, float max)
        {
            AnimatorControllerParameterType type = LocalAnimatorManager.Instance.GetAnimatorParameterType(sourceName);
            float randValue = 0f;
            switch (type)
            {
                case AnimatorControllerParameterType.Float:
                    randValue = Random.Range(min, max);
                    break;
                case AnimatorControllerParameterType.Int:
                    randValue = Random.Range(min, max);
                    break;
                case AnimatorControllerParameterType.Bool:
                    randValue = (Random.value > chance) ? 1f : 0f;
                    break;
                case AnimatorControllerParameterType.Trigger:
                    randValue = (Random.value > chance) ? 1f : 0f;
                    break;
            }
            return randValue;
        }

        //returns the sourceName parameter after its been modified as a float
        //taken from https://github.com/lyuma/Av3Emulator - MIT License
        internal float getAdjustedParameterAsFloat(Animator animator, string sourceName, bool convertRange = false, float srcMin = 0.0f, float srcMax = 0.0f, float dstMin = 0.0f, float dstMax = 0.0f)
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
            if (convertRange)
            {
                if (dstMax != dstMin)
                {
                    newValue = Mathf.Lerp(dstMin, dstMax, Mathf.Clamp01(Mathf.InverseLerp(srcMin, srcMax, newValue)));
                }
                else
                {
                    newValue = dstMin;
                }
            }
            return newValue;
        }
    }
}