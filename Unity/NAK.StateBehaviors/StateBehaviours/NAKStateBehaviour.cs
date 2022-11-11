using System;
using UnityEngine;

namespace NAK.StateBehaviours
{
    //for lookup of all my custom behaviors
    //will allow me to strip from remote players easier
    public class NAKStateBehaviour : StateMachineBehaviour
    {
        public enum UpdateModes
        {
            OnEnter,
            OnExit,
        }
    }
}