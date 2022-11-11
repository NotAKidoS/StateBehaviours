#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;
using System.Linq;
using UnityEditor.SceneManagement;
using System.Reflection;
using AnimatorControllerParameter = UnityEngine.AnimatorControllerParameter;
using AnimatorControllerParameterType = UnityEngine.AnimatorControllerParameterType;

//TODO: Implement chance value selection for random bool & trigger types

namespace NAK.StateBehaviors.MovementControl
{


    [CustomEditor(typeof(NAKMovementControl))]
    public class NAKMovementControlEditor : Editor
    {
        public NAKMovementControl MovementControl;

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            MovementControl = (NAKMovementControl)target;

            //display curve editor
            MovementControl.curveX = EditorGUILayout.CurveField("Animation on X", MovementControl.curveX);
            MovementControl.curveY = EditorGUILayout.CurveField("Animation on Y", MovementControl.curveY);

            if(EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(this);
        }
    }
}

#endif