using HarmonyLib;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="Boosterpack"/> class.
    /// </summary>
    [HarmonyPatch(typeof(Boosterpack))]
    public class Boosterpack_Patches
    {
        /// <summary>
        /// Prevent resource boosters from spawning villagers.
        /// </summary>
        [HarmonyPatch(nameof(Boosterpack.Clicked))]
        [HarmonyPrefix]
        public static void OnClicked_PrefixIntercept(Boosterpack __instance)
        {
            // If a pack has been bought only once on a board, prevent resource booster from only spawning structures.

            // Intercept if resource booster pack
            if (__instance.BoosterId == ModBoosterPacks.resource_booster)
            {
                // Temporarily change to intro pack to prevent villager spawning
                __instance.PackData.IsIntroPack = true;
            }
        }

        /// <summary>
        /// Prevent resource boosters from spawning villagers.
        /// </summary>
        [HarmonyPatch(nameof(Boosterpack.Clicked))]
        [HarmonyPostfix]
        public static void OnClicked_PostfixIntercept(Boosterpack __instance)
        {
            // Intercept if resource booster pack
            if (__instance.BoosterId == ModBoosterPacks.resource_booster)
            {
                // Set back to not intro pack
                __instance.PackData.IsIntroPack = false;
            }
        }
    }
}
