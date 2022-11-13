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
    [CustomEditor(typeof(NAKSimpleDriver))]
    public class NAKSimpleDriverEditor : Editor
    {
        private static class Styles
        {
            public static readonly GUIStyle
                box = GUI.skin.GetStyle("box"),
                selectionRect = GUI.skin.GetStyle("selectionRect"),
                helpBox = GUI.skin.GetStyle("helpbox");
        }

        public NAKSimpleDriver Driver;
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
            Driver = (NAKSimpleDriver)target;

            //initialize default lists, cause unity doesnt
            if (Driver.updateMode == null)
            {
                //OnEnter or OnExit
                Driver.updateMode = new List<NAKSimpleDriver.UpdateModes>();
                //set, add, random, copy
                Driver.driverType = new List<NAKSimpleDriver.DriverTypes>();
                Driver.settingName = new List<string>();
                Driver.settingValue = new List<float>();
                //random
                Driver.valMin = new List<float>();
                Driver.valMax = new List<float>();
                //copy
                Driver.sourceName = new List<string>();
                //copy + convert range
                Driver.convertRange = new List<bool>();
                Driver.srcMin = new List<float>();
                Driver.srcMax = new List<float>();
                Driver.dstMin = new List<float>();
                Driver.dstMax = new List<float>();
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

            //Debug.Log("Count is " + Driver.driverType.Count);

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
            //OnEnter or OnExit
            Driver.updateMode.Add(0);
            //set, add, random, copy
            Driver.driverType.Add(0);
            Driver.settingName.Add("");
            Driver.settingValue.Add(0);
            //random
            Driver.valMin.Add(0);
            Driver.valMax.Add(0);
            //copy
            Driver.sourceName.Add("");
            //copy + convert range
            Driver.convertRange.Add(false);
            Driver.srcMin.Add(0);
            Driver.srcMax.Add(0);
            Driver.dstMin.Add(0);
            Driver.dstMax.Add(0);
            selected = Driver.driverType.Count-1;
        }

        internal void RemoveDriver()
        {
            //OnEnter or OnExit
            Driver.updateMode.RemoveAt(selected);
            //set, add, random, copy
            Driver.driverType.RemoveAt(selected);
            Driver.settingName.RemoveAt(selected);
            Driver.settingValue.RemoveAt(selected);
            //random
            Driver.valMin.RemoveAt(selected);
            Driver.valMax.RemoveAt(selected);
            //copy
            Driver.sourceName.RemoveAt(selected);
            //copy + convert range
            Driver.convertRange.RemoveAt(selected);
            Driver.srcMin.RemoveAt(selected);
            Driver.srcMax.RemoveAt(selected);
            Driver.dstMin.RemoveAt(selected);
            Driver.dstMax.RemoveAt(selected);
            selected -= 1;
        }

        internal void MoveDriverUp()
        {
            if (selected == 0) return;
            selected -= 1;
            //OnEnter or OnExit
            Driver.updateMode.Reverse(selected, 2);
            //set, add, random, copy
            Driver.driverType.Reverse(selected, 2);
            Driver.settingName.Reverse(selected, 2);
            Driver.settingValue.Reverse(selected, 2);
            //random
            Driver.valMin.Reverse(selected, 2);
            Driver.valMax.Reverse(selected, 2);
            //copy
            Driver.sourceName.Reverse(selected, 2);
            //copy + convert range
            Driver.convertRange.Reverse(selected, 2);
            Driver.srcMin.Reverse(selected, 2);
            Driver.srcMax.Reverse(selected, 2);
            Driver.dstMin.Reverse(selected, 2);
            Driver.dstMax.Reverse(selected, 2);
        }

        internal void MoveDriverDown()
        {
            if (selected == Driver.driverType.Count-1) return;
            //OnEnter or OnExit
            Driver.updateMode.Reverse(selected, 2);
            //set, add, random, copy
            Driver.driverType.Reverse(selected, 2);
            Driver.settingName.Reverse(selected, 2);
            Driver.settingValue.Reverse(selected, 2);
            //random
            Driver.valMin.Reverse(selected, 2);
            Driver.valMax.Reverse(selected, 2);
            //copy
            Driver.sourceName.Reverse(selected, 2);
            //copy + convert range
            Driver.convertRange.Reverse(selected, 2);
            Driver.srcMin.Reverse(selected, 2);
            Driver.srcMax.Reverse(selected, 2);
            Driver.dstMin.Reverse(selected, 2);
            Driver.dstMax.Reverse(selected, 2);
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
            EditorGUILayout.LabelField("Update On:", GUILayout.Width(65));
            Driver.updateMode[i] = (NAKSimpleDriver.UpdateModes)EditorGUILayout.EnumPopup((NAKSimpleDriver.UpdateModes)Driver.updateMode[i]);
            EditorGUILayout.LabelField("Type:", GUILayout.Width(35));
            Driver.driverType[i] = (NAKSimpleDriver.DriverTypes)EditorGUILayout.EnumPopup((NAKSimpleDriver.DriverTypes)Driver.driverType[i]);
            GUILayout.EndHorizontal();

            switch (Driver.driverType[i])
            {
                //Set
                case NAKSimpleDriver.DriverTypes.Set:
                    //setting name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Setting Name:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                    GUILayout.EndHorizontal();
                    //setting value
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Setting Value:", GUILayout.Width(100));
                    Driver.settingValue[i] = EditorGUILayout.FloatField(Driver.settingValue[i]);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);


                    break;
                case NAKSimpleDriver.DriverTypes.Add:
                    //setting name
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Setting Name:", GUILayout.Width(100));
                    //Dropdown & Textfield
                    Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                    GUILayout.EndHorizontal();
                    //setting value
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Added Value:", GUILayout.Width(100));
                    Driver.settingValue[i] = EditorGUILayout.FloatField(Driver.settingValue[i]);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    break;
                case NAKSimpleDriver.DriverTypes.Random:
                        //setting name
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Setting Name:", GUILayout.Width(100));
                        //Dropdown & Textfield
                        Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                        GUILayout.EndHorizontal();

                        //check type
                        AnimatorControllerParameterType type;
                        var hasType = parameterList.TryGetValue( Driver.settingName[i], out type );
                        
                        //TODO: better handle this...

                        //just in case
                        if (!hasType)
                        {
                            //minimum & maximum choices (float, int)
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Minimum:", GUILayout.Width(65));
                            Driver.valMin[i] = EditorGUILayout.FloatField(Driver.valMin[i]);
                            EditorGUILayout.LabelField("Maximum:", GUILayout.Width(65));
                            Driver.valMax[i] = EditorGUILayout.FloatField(Driver.valMax[i]);
                            GUILayout.EndHorizontal();
                            //chance value for bool, trigger
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Chance:", GUILayout.Width(65));
                            Driver.settingValue[i] = EditorGUILayout.Slider(Driver.settingValue[i], 0, 1);
                            GUILayout.EndHorizontal();
                            break;
                        }

                        if (type == AnimatorControllerParameterType.Int || type == AnimatorControllerParameterType.Float)
                        {
                            //minimum & maximum choices (float, int)
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Minimum:", GUILayout.Width(65));
                            Driver.valMin[i] = EditorGUILayout.FloatField(Driver.valMin[i]);
                            EditorGUILayout.LabelField("Maximum:", GUILayout.Width(65));
                            Driver.valMax[i] = EditorGUILayout.FloatField(Driver.valMax[i]);
                            GUILayout.EndHorizontal();
                            break;
                        }
                        //chance value for bool, trigger
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Chance:", GUILayout.Width(65));
                        Driver.settingValue[i] = EditorGUILayout.Slider(Driver.settingValue[i], 0, 1);
                        GUILayout.EndHorizontal();
                    break;
                case NAKSimpleDriver.DriverTypes.Copy:
                        //source name
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Copy From:", GUILayout.Width(100));
                        //Dropdown & Textfield
                        Driver.sourceName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.sourceName[i]) );
                        GUILayout.EndHorizontal();
                        //setting name
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Copy To:", GUILayout.Width(100));
                        //Dropdown & Textfield
                        Driver.settingName[i] = EditorGUILayout.TextField( DrawParameterDropdown(Driver.settingName[i]) );
                        GUILayout.EndHorizontal();

                        //convert range option
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Convert Range:", GUILayout.Width(100));
                        Driver.convertRange[i] = EditorGUILayout.Toggle(Driver.convertRange[i]);
                        GUILayout.EndHorizontal();

                        //convert range settings
                        if (Driver.convertRange[i])
                        {
                            GUILayout.Space(10);
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Covert From Range:");
                            GUILayout.EndHorizontal();
                            //Source Minimum&Maximum
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Minimum:", GUILayout.Width(60));
                            Driver.srcMin[i] = EditorGUILayout.FloatField(Driver.srcMin[i]);
                            EditorGUILayout.LabelField("Maximum:", GUILayout.Width(60));
                            Driver.srcMax[i] = EditorGUILayout.FloatField(Driver.srcMax[i]);
                            GUILayout.EndHorizontal();

                            GUILayout.Space(10);
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Covert To Range:");
                            GUILayout.EndHorizontal();
                            //Destination Minimum&Maximum
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Minimum:", GUILayout.Width(60));
                            Driver.dstMin[i] = EditorGUILayout.FloatField(Driver.dstMin[i]);
                            EditorGUILayout.LabelField("Maximum:", GUILayout.Width(60));
                            Driver.dstMax[i] = EditorGUILayout.FloatField(Driver.dstMax[i]);
                            GUILayout.EndHorizontal();
                        }

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