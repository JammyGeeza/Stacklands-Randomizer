using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="GameCard"/> class.
    /// </summary>
    [HarmonyPatch(typeof(GameCard))]
    public class GameCard_Patches
    {
        /// <summary>
        /// Disable the timer for creating items if the blueprint is not yet discovered.
        /// </summary>
        [HarmonyPatch(nameof(GameCard.StartBlueprintTimer))]
        [HarmonyPrefix]
        public static bool OnStartBlueprintTimer_BlockWhereNecessary(ref string blueprintId)
        {
            Debug.Log($"{nameof(GameCard)}.{nameof(GameCard.StartBlueprintTimer)} Prefix!");
            Debug.Log($"Blueprint Timer ID: {blueprintId}");

            // Block timers for cards if idea not yet discovered
            return !CommonPatchMethods.ShouldCardBeBlocked(blueprintId);
        }
    }
}
