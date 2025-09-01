using HarmonyLib;
using Stacklands_Randomizer_Mod.Constants;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="Mob"/> class.
    /// </summary>
    [HarmonyPatch(typeof(Mob))]
    public class Mob_Patches
    {
        /// <summary>
        /// When a Mob dies, log its death.
        /// </summary>
        [HarmonyPatch("Die")]
        [HarmonyPostfix]
        public static void OnDie_LogMobDeath(ref Mob __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(Mob)}.Die postfix!");

            // Increment amount of times this mob has been killed
            int mobKilledCount = CommonPatchMethods.GetTimesMobKilled(__instance.Id);
            KeyValueHelper.SetExtraKeyValue($"mob_{__instance.Id}", mobKilledCount + 1);
        }
    }
}
