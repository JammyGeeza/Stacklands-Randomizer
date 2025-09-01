using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="CreatePackLine"/> class.
    /// </summary>
    [HarmonyPatch(typeof(CreatePackLine))]
    public class CreatePackLine_Patches
    {
        /// <summary>
        /// Add custom booster boxes before creation.
        /// </summary>
        [HarmonyPatch(nameof(CreatePackLine.CreateBoosterBoxes))]
        [HarmonyPrefix]
        public static void OnCreateBoosterBoxes_AddCustomBoosters(CreatePackLine __instance, ref List<string> boosters, ref BoardCurrency currency)
        {
            StacklandsRandomizer.instance.ModLogger.Log("CreateBoosterBoxes Prefix!");

            // Insert booster pack if speedsanity enabled.
            if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off)
            {
                // If creating for mainland, insert spendsanity booster box
                if (currency == BoardCurrency.Gold && boosters.Contains("idea") && !boosters.Contains(ModBoosterPacks.spendsanity))
                {
                    boosters.Add(ModBoosterPacks.spendsanity);
                }

                // Set as unlocked if not already
                if (!WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(ModBoosterPacks.spendsanity))
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(ModBoosterPacks.spendsanity);
                }
            }
        }
    }
}
