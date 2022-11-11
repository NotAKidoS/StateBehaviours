
using ABI_RC.Core.Player;
using MelonLoader;
using UnityEngine;

namespace NAK.StateBehaviors;

public class StateBehaviorsMod : MelonMod
{
    public override void OnApplicationStart()
    {
        MelonLoader.MelonCoroutines.Start(WaitForLocalPlayer());
    }

    System.Collections.IEnumerator WaitForLocalPlayer()
    {
        while (PlayerSetup.Instance == null)
            yield return null;

        PlayerSetup.Instance.gameObject.AddComponent<LocalAnimatorManager>();
    }
}