using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABI_RC.Core.Player;

//This monobehavior is here for testing in Unity. 
//Replicated the basics of ChilloutVR's AnimatorManager that I'll need to patch if I can make this a mod.
//Might update to support testing multiple avatars at once in Unity... but im lazy.

//only addition is animatorParameterTypes dictionary

//POTENTIAL FLAW
//Anything that calls SetAnimatorParameterFromFloat() will set the local players parameters!


public class LocalAnimatorManager : MonoBehaviour
{
    public static LocalAnimatorManager Instance;

    private Dictionary<string, AnimatorControllerParameterType> animatorParameterTypes = new Dictionary<string, AnimatorControllerParameterType>();
    void Start()
    {
        Instance = this;
    }

    //stolen
    public void AnalyzeController(Animator _animator)
    {
        this.animatorParameterTypes.Clear();
        for (int i = 0; i<_animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = _animator.parameters[i];
            //add to indicies dictionary
            if (!this.animatorParameterTypes.ContainsKey(parameter.name))
                this.animatorParameterTypes.Add(parameter.name, parameter.type);
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
                PlayerSetup.Instance.animatorManager.SetAnimatorParameterFloat(name, value);
                break;
            case AnimatorControllerParameterType.Int:
                PlayerSetup.Instance.animatorManager.SetAnimatorParameterInt(name, (int)value);
                break;
            case AnimatorControllerParameterType.Bool:
                PlayerSetup.Instance.animatorManager.SetAnimatorParameterBool(name, value > 0.5f);
                break;
            case AnimatorControllerParameterType.Trigger:
                if (value>0)
                    PlayerSetup.Instance.animatorManager.SetAnimatorParameterTrigger(name);
                break;
        }
    }
}
