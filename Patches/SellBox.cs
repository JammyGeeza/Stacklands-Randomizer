using HarmonyLib;
using Stacklands_Randomizer_Mod.Constants;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="SellBox"/> class.
    /// </summary>
    [HarmonyPatch(typeof(SellBox))]
    public class SellBox_Patches
    {
        /// <summary>
        /// Add custom booster boxes before creation.
        /// </summary>
        [HarmonyPatch(nameof(SellBox.CardDropped))]
        [HarmonyPostfix]
        public static void OnCardDropped_LogSellCard(SellBox __instance, ref GameCard card)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(SellBox)}.{nameof(SellBox.CardDropped)} Postfix!");

            // Get current amount of cards sold and increment value
            int currentSold = KeyValueHelper.GetExtraKeyValue(Terms.CardsSold);
            KeyValueHelper.SetExtraKeyValue(Terms.CardsSold, currentSold + 1);
        }
    }
}
