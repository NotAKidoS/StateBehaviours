using ABI_RC.Core;
using HarmonyLib;
using MelonLoader;
using System.Collections;
using UnityEngine;
using NAK.StateBehaviours.ParameterDriver;

namespace NAK.StateBehaviours;

[HarmonyPatch]
internal class HarmonyPatches
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CVRAnimatorManager), "AnalyzeController")]
    static void OnAnalyzeController_Prefix(ref Animator ____animator)
    {
        //AnalyzeController runs for everyone... thats why UI shows wrong emote strings?
        if (____animator.gameObject.layer == 8)
        {
            MelonLogger.Msg("Animator " + ____animator + " is local!");
            LocalAnimatorManager.Instance.AnalyzeController(____animator);
            
        }
        else
        {
            MelonLogger.Msg("Removing NAKStateBehaviour from remote avatar");

            NAKStateBehaviour[] array = ____animator.GetBehaviours<NAKStateBehaviour>();

            MelonLogger.Msg(array);

            foreach (NAKStateBehaviour behaviour in array)
            {
                MelonLogger.Msg("Found " + behaviour);
                if (behaviour != null)
                {
                    MelonLogger.Msg("Removed " + behaviour);
                    UnityEngine.Object.DestroyImmediate(behaviour);
                }
            }
        }

        MelonLogger.Msg("Removing all null NAKStateBehaviours left behind after avatar initialization.");

        //shit fix for cvr related conflict- theres a bunch of null clones on avatar load ???
        foreach (NAKStateBehaviour behaviour in Resources.FindObjectsOfTypeAll(typeof(NAKStateBehaviour)) as NAKStateBehaviour[])
        {
            if (behaviour.name == "")
            {
                MelonLogger.Msg("Removed " + behaviour);
                UnityEngine.Object.Destroy(behaviour);
            }
        }
    }
}