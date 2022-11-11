
using ABI_RC.Core.Player;
using MelonLoader;
using UnityEngine;

namespace NAK.StateBehaviours;

public class StateBehaviorsMod : MelonMod
{
    public override void OnInitializeMelon()
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