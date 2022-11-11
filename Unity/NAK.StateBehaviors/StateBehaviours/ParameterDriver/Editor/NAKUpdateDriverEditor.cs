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

namespace NAK.StateBehaviours.ParameterDriver
{
    [CustomEditor(typeof(NAKUpdateDriver))]
    public class NAKUpdateDriverEditor : Editor
    {
        private static class Styles
        {
            public static readonly GUIStyle
                box = GUI.skin.GetStyle("box"),
                selectionRect = GUI.skin.GetStyle("selectionRect"),
                helpBox = GUI.skin.GetStyle("helpbox");
        }

        public NAKUpdateDriver Driver;
        int selected = -1;

        //for parameter dropdown
        private Dictionary<string, AnimatorControllerParameterType> parameterList = new Dictionary<string, AnimatorControllerParameterType>();

        //google has all the answers huh
        static UnityEditor.Animations.AnimatorController GetCurrentController()
        {
            UnityEditor.Animations.AnimatorController controller = null;
            var tool = EditorWindow.focusedWindow;
            var toolType = tool.GetType();
            var controllerProperty = toolType.GetProperty("animatorController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if(controllerProperty != null)
            {
                controller = controllerProperty.GetValue(tool, null) as UnityEditor.Animations.AnimatorController;
            }
            else
            {
                Debug.Log("EditorWindow.focusedWindow " + tool + " does not contain animatorController", tool);
            }
            return controller;
        }

        public void OnEnable()
        {
            FindParameters();
        }

        void FindParameters()
        {
            parameterList.Clear();
            var controller = GetCurrentController();
            if(controller == null) return;
            foreach(var parameter in controller.parameters) //imagine not having parameterCount -_-
            {
                //dont log any built in CVR parameters
                //if (coreParameters.Contains(parameter.name)) continue;

                //add to indicies dictionary
                if (!parameterList.ContainsKey(parameter.name))
                    parameterList.Add(parameter.name, parameter.type);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            Driver = (NAKUpdateDriver)target;

            //initialize default lists, cause unity doesnt
            if (Driver.driverType == null)
            {
                //PlayTime, NormalizedTime, Lerp
                Driver.driverType = new List<NAKUpdateDriver.DriverTypes>();
                Driver.settingName = new List<string>();
                Driver.settingValue = new List<float>();
                Driver.sourceName = new List<string>();
                Driver.sourceValue = new List<float>();
            }

            //DrawDefaultInspector();

            //Add driver button, creates new entry in all lists
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
                AddDriver();
            EditorGUI.BeginDisabledGroup(selected == -1);
                if (GUILayout.Button("Up"))
                    MoveDriverUp();
                if (GUILayout.Button("Down"))
                    MoveDriverDown();
                if (GUILayout.Button("Delete"))
                    RemoveDriver();
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            //if list is null
            if (Driver.driverType == null) return;

            for (int i = 0; i < Driver.driverType.Count; i++)
            {
                DrawDriverSettings(i);
            }

            if(EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(this);
        }

        internal void AddDriver()
        {
            //PlayTime, NormalizedTime, Lerp
            Driver.driverType.Add(0);
            Driver.settingName.Add("");
            Driver.settingValue.Add(0);
            //lerp
            Driver.sourceName.Add("");
            Driver.sourceValue.Add(0);
            selected = Driver.driverType.Count-1;
        }

        internal void RemoveDriver()
        {
            //PlayTime, NormalizedTime, Lerp
            Driver.driverType.RemoveAt(selected);
            Driver.settingName.RemoveAt(selected);
            Driver.settingValue.RemoveAt(selected);
            //lerp
            Driver.sourceName.RemoveAt(selected);
            Driver.sourceValue.RemoveAt(selected);
            selected -= 1;
        }

        internal void MoveDriverUp()
        {
            if (selected == 0) return;
            selected -= 1;
            //PlayTime, NormalizedTime, Lerp
            Driver.driverType.Reverse(selected, 2);
            Driver.settingName.Reverse(selected, 2);
            Driver.settingValue.Reverse(selected, 2);
            //lerp
            Driver.sourceName.Reverse(selected, 2);
            Driver.sourceValue.Reverse(selected, 2);
        }

        internal void MoveDriverDown()
        {
            if (selected == Driver.driverType.Count-1) return;
            //PlayTime, NormalizedTime, Lerp
            Driver.driverType.Reverse(selected, 2);
            Driver.settingName.Reverse(selected, 2);
            Driver.settingValue.Reverse(selected, 2);
            //lerp
            Driver.sourceName.Reverse(selected, 2);
            Driver.sourceValue.Reverse(selected, 2);
            selected += 1;
        }

        //expensive
        internal string DrawParameterDropdown(string name)
        {
            var listParams = parameterList.Keys.ToList();
            var arrayParams = listParams.ToArray();
            int num = listParams.IndexOf(name);
            num = EditorGUILayout.Popup(num, arrayParams);
            if (num != -1)
                name = listParams[num];
            return name;
        }

        internal void DrawDriverSettings(int i)
        {
            //selected visual
            var style = Styles.box;
            if (selected == i)
                style = Styles.selectionRect;
            //entire container
            Rect selectable = (Rect)EditorGUILayout.BeginVertical(style);

            //Update & Type selection
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type:", GUILayout.Width(35));
            Driver.driverType[i] = (NAKUpdateDriver.DriverTypes)EditorGUILayout.EnumPopup((NAKUpdateDriver.DriverTypes)Driver.driverType[i]);
            GUILayout.EndHorizontal();

            switch (Driver.driverType[i])
            {
                //Set
                case NAKUpdateDriver.DriverTypes.PlayTime:
                    //setting name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Setting Name:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                    GUILayout.EndHorizontal();
                    break;
                case NAKUpdateDriver.DriverTypes.NormalizedTime:
                    //setting name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Setting Name:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                    GUILayout.EndHorizontal();
                    break;
   
                case NAKUpdateDriver.DriverTypes.Lerp:
                    //source name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Lerp Source:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.sourceName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.sourceName[i]) );
                    GUILayout.EndHorizontal();
                    //setting name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Output:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                    GUILayout.EndHorizontal();
                    //setting value
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Lerp Speed:", GUILayout.Width(100));
                    Driver.settingValue[i] = EditorGUILayout.FloatField(Driver.settingValue[i]);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    break;
                default:
                    break;
            }
            
            //check if this element was clicked
            if (Event.current.type == EventType.MouseUp && selectable.Contains(Event.current.mousePosition) )
            {
                selected = (selected == i) ? -1 : i;
                Repaint();
            }

            EditorGUILayout.EndVertical();
        }
    }


}

#endif