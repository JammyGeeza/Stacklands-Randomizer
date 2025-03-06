using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="SaveManager"/> class.
    /// </summary>
    [HarmonyPatch(typeof(SaveManager))]
    public class SaveManager_Patches
    {
        private static readonly string SAVE_ID = "archipelago";

        /// <summary>
        ///  When determining current save, override with an archipelago save.
        /// </summary>
        [HarmonyPatch("DetermineCurrentSave")]
        [HarmonyPostfix]
        public static void OnDetemineCurrentSave_ReplaceWithArchipelagoSave(SaveManager __instance, ref SaveGame __result)
        {
            Debug.Log($"{nameof(SaveManager)}.DetermineCurrentSave Postfix!");
            Debug.Log($"Determined save ID: {__result.SaveId}");

            // Intercept result with an Archipelago save
            __result = CommonPatchMethods.FindOrCreateArchipelagoSave(__instance);
        }
    }
}
