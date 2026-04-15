using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="GameCard"/> class.
    /// </summary>
    [HarmonyPatch(typeof(University))]
    public class University_Patches
    {
        /// <summary>
        /// Prevent the University structure from giving inventions.
        /// </summary>
        [HarmonyPatch(nameof(University.GiveInvention))]
        [HarmonyPrefix]
        public static bool OnGiveInvention_Prefix(University __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(University)}.{nameof(University.GiveInvention)} Prefix!");

            // Spawn a random basic card
            CardData cardData = WorldManager.instance.CreateCard(__instance.MyGameCard.Position, CommonPatchMethods.GetRandomBasicCard(), checkAddToStack: false, faceUp: false);
            cardData.MyGameCard.SendIt();

            // Reset coin count
            __instance.CoinCount = 0;

            // Prevent original method
            return false;
        }
    }
}
