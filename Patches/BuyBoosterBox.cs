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
            if (__instance.BoosterId == ModBoosterPacks.check_booster)
            {
                // Fire special action to trigger check
                QuestManager.instance.SpecialActionComplete($"buy_{__instance.BoosterId}_pack");

                // TODO: Implement YAML options for this
                // TODO: Increment cost by amount in YAML option and store the current cost (may not be required due to next TODO)
                // TODO: Increment total times purchased this session (or find out if this is already stored somewhere)
                // TODO: If max check amount reached, prevent further purchases

                // Increase cost
                __instance.Cost += 5;

                // Prevent original method actions
                return false;
            }

            return true;
        }
    }
}
