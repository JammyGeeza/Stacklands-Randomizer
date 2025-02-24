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
        /// Disable the blueprint timer for currently undiscovered blueprints.
        /// </summary>
        [HarmonyPatch(nameof(GameCard.StartBlueprintTimer))]
        [HarmonyPrefix]
        public static bool Prefix(ref string blueprintId)
        {
            Debug.Log($"{nameof(GameCard)}.{nameof(GameCard.StartBlueprintTimer)} Prefix!");

            return WorldManager.instance.CurrentSave.FoundCardIds.Contains(blueprintId);
        }
    }
}
