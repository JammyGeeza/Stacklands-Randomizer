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
        public static bool Prefix(ref string blueprintId)
        {
            Debug.Log($"{nameof(GameCard)}.{nameof(GameCard.StartBlueprintTimer)} Prefix!");
            Debug.Log($"Blueprint Timer ID: {blueprintId}");

            // Allow timers for IDs that don't start with "blueprint_" (such as graveyard which is "ideas_base")
            return !blueprintId.StartsWith("blueprint_") || ItemHandler.IsIdeaDiscovered(blueprintId);
        }
    }
}
