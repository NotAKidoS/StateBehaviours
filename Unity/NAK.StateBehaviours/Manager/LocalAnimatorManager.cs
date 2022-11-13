using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This monobehavior is here for testing in Unity. 
//Replicated the basics of ChilloutVR's AnimatorManager that I'll need to patch if I can make this a mod.
//Might update to support testing multiple avatars at once in Unity... but im lazy.

//only addition is animatorParameterTypes dictionary

namespace NAK.StateBehaviours
{
    public class LocalAnimatorManager : MonoBehaviour
    {
        public static LocalAnimatorManager Instance;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            AnalyzeController();
        }

        //stolen
        public void AnalyzeController()
        {
            this.animatorParameterTypes.Clear();
            this.animatorParameterNameHashes.Clear();
            this.animatorParameterBoolList.Clear();
            this.animatorParameterFloatList.Clear();
            this.animatorParameterIntList.Clear();
            this.animatorParameterTriggerList.Clear();

            for (int i = 0; i<this._animator.parameterCount; i++)
            {
                AnimatorControllerParameter parameter = this._animator.parameters[i];

                //dont log any built in CVR parameters
                //if (coreParameters.Contains(parameter.name)) continue;

                //add to indicies dictionary
                if (!this.animatorParameterTypes.ContainsKey(parameter.name))
                    this.animatorParameterTypes.Add(parameter.name, parameter.type);

                //log nameHash for optimization n stuff
                if (!this.animatorParameterNameHashes.ContainsKey(parameter.name))
                    this.animatorParameterNameHashes.Add(parameter.name, parameter.nameHash);
                
                //sort parameters into lists
                AnimatorControllerParameterType type = parameter.type;
                switch (type)
                {
                case AnimatorControllerParameterType.Float:
                    this.animatorParameterFloatList.Add(parameter.name);
                    break;
                case AnimatorControllerParameterType.Int:
                    this.animatorParameterIntList.Add(parameter.name);
                    break;
                case AnimatorControllerParameterType.Bool:
                    this.animatorParameterBoolList.Add(parameter.name);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    this.animatorParameterTriggerList.Add(parameter.name);
                    break;
                }
            }
        }

        public AnimatorControllerParameterType GetAnimatorParameterType(string name)
        {
            //might need catch here...
            return animatorParameterTypes[name];
        }

        public void SetAnimatorParameterFromFloat(string name, float value)
        {
            AnimatorControllerParameterType type = LocalAnimatorManager.Instance.GetAnimatorParameterType(name);
            switch (type)
            {
                case AnimatorControllerParameterType.Float:
                    LocalAnimatorManager.Instance.SetAnimatorParameterFloat(name, value);
                    break;
                case AnimatorControllerParameterType.Int:
                    LocalAnimatorManager.Instance.SetAnimatorParameterInt(name, (int)value);
                    break;
                case AnimatorControllerParameterType.Bool:
                    LocalAnimatorManager.Instance.SetAnimatorParameterBool(name, value>0.5f);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (value>0.5f)
                        LocalAnimatorManager.Instance.SetAnimatorParameterTrigger(name);
                    break;
            }
        }

        public void SetAnimatorParameterFloat(string name, float value)
        {
            if (!this.animatorParameterFloatList.Contains(name) || this._animator == null) return;

            if (!this._animator.IsParameterControlledByCurve(this.animatorParameterNameHashes[name]))
            {
                this._animator.SetFloat(this.animatorParameterNameHashes[name], value);
            }
        }

        public void SetAnimatorParameterInt(string name, int value)
        {
            if (!this.animatorParameterIntList.Contains(name) || this._animator == null) return;

            if (!this._animator.IsParameterControlledByCurve(this.animatorParameterNameHashes[name]))
            {
                this._animator.SetInteger(this.animatorParameterNameHashes[name], value);
            }
        }

        public void SetAnimatorParameterBool(string name, bool value)
        {
            if (!this.animatorParameterBoolList.Contains(name) || this._animator == null) return;

            if (!this._animator.IsParameterControlledByCurve(this.animatorParameterNameHashes[name]))
            {
                this._animator.SetBool(this.animatorParameterNameHashes[name], value);
            }
        }

        public void SetAnimatorParameterTrigger(string name)
        {
            if (!this.animatorParameterTriggerList.Contains(name) || this._animator == null) return;

            if (!this._animator.IsParameterControlledByCurve(this.animatorParameterNameHashes[name]))
            {
                this._animator.SetTrigger(this.animatorParameterNameHashes[name]);
            }
        }

        public Animator _animator;
        public Dictionary<string, int> animatorParameterNameHashes = new Dictionary<string, int>();
        public List<string> animatorParameterBoolList = new List<string>();
        public List<string> animatorParameterFloatList = new List<string>();
        public List<string> animatorParameterIntList = new List<string>();
        public List<string> animatorParameterTriggerList = new List<string>();

        private Dictionary<string, AnimatorControllerParameterType> animatorParameterTypes = new Dictionary<string, AnimatorControllerParameterType>();

        //built in chilloutvr parameters
        private static HashSet<string> coreParameters = new HashSet<string>
        {
            "MovementX",
            "MovementY",
            "Grounded",
            "Emote",
            "GestureLeft",
            "GestureRight",
            "Toggle",
            "Sitting",
            "Crouching",
            "CancelEmote",
            "Prone",
            "Flying"
        };
    }
}