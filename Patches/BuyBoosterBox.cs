using HarmonyLib;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="BuyBoosterBox"/> class.
    /// </summary>
    [HarmonyPatch(typeof(BuyBoosterBox))]
    public class BuyBoosterBox_Patches
    {
        /// <summary>
        /// Intercept AP Check Booster Box from dispensing packs.
        /// </summary>
        [HarmonyPatch("CreateBoosterPack")]
        [HarmonyPrefix]
        public static bool OnCreateBoosterPack_PreventBoosterPack(BuyBoosterBox __instance)
        {
            // If booster box is for the AP Check booster pack
            if (__instance.BoosterId == ModBoosterPacks.spendsanity)
            {
                // Increment bought count
                int boughtCount = CommonPatchMethods.IncrementTimesBoosterPackBought(__instance.BoosterId);

                // Fire special action to trigger check
                QuestManager.instance.SpecialActionComplete($"buy_{ModBoosterPacks.spendsanity}_pack");

                // Check if maximum reached
                if (boughtCount < StacklandsRandomizer.instance.Options.SpendsanityCount)
                {
                    // Increase cost if incremental spendsanity mode
                    if (StacklandsRandomizer.instance.Options.Spendsanity is Spendsanity.Incremental)
                    {
                        __instance.Cost += StacklandsRandomizer.instance.Options.SpendsanityCost;
                    }
                }
                else
                {
                    __instance.enabled = false;
                }

                // Prevent original method actions
                return false;
            }

            return true;
        }
    }
}
