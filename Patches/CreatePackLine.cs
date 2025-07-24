using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="CardBag"/> class.
    /// </summary>
    [HarmonyPatch(typeof(CreatePackLine))]
    public class CreatePackLine_Patches
    {
        /// <summary>
        /// Add custom booster boxes
        /// </summary>
        [HarmonyPatch(nameof(CreatePackLine.CreateBoosterBoxes))]
        [HarmonyPrefix]
        public static void OnCreateBoosterBoxes_AddCustomBoosters(CreatePackLine __instance, ref List<string> boosters, ref BoardCurrency currency)
        {
            // TODO: Only do this if enabled in YAML options
            // TODO: Set cost on creation based on how many times previously purchased

            switch (currency)
            {
                case BoardCurrency.Gold:
                    {
                        // Add AP Check booster pack to the list
                        boosters.Add(ModBoosterPacks.check_booster);

                        // Set as unlocked
                        WorldManager.instance.CurrentSave.FoundBoosterIds.Add(ModBoosterPacks.check_booster);
                    }
                    break;
            }
        }
    }
}
