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
        public static void OnDie_LogMobDeath(Mob __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(Mob)}.Die postfix!");

            // Increment amount of times this mob has been killed (and isn't a trap!)
            if (!__instance.Name.StartsWith("Trap "))
            {
                int mobKilledCount = CommonPatchMethods.GetTimesMobKilled(__instance.Id);
                KeyValueHelper.SetExtraKeyValue($"mob_{__instance.Id}", mobKilledCount + 1);
            }
        }

        [HarmonyPatch(nameof(Mob.TryDropItems))]
        [HarmonyPrefix]
        public static bool OnTryDropItems_Prevent(Mob __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(Mob)}.{nameof(Mob.TryDropItems)} Prefix!");

            // If combatable's override name starts with 'Trap '...
            if (__instance.Name.StartsWith("Trap "))
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Preventing drops for {__instance.Name}...");

                // Prevent original method from firing
                return false;
            }

            // Continue as normal
            return true;
        }
    }
}
