using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod.Patches
{
    /// <summary>
    /// Patches for the <see cref="Harvestable"/> class.
    /// </summary>
    [HarmonyPatch(typeof(Harvestable))]
    public class Harvestable_Patches
    {
        /// <summary>
        /// Replace un-found cards from harvestables.
        /// </summary>
        [HarmonyPatch(nameof(Harvestable.GetCardToGive))]
        [HarmonyPostfix]
        public static void OnGetCardsInBag_ReplaceWhereNecessary(Harvestable __instance, ref ICardId __result)
        {
            Debug.Log($"{nameof(Harvestable)}.{nameof(Harvestable.GetCardToGive)} Postfix!");
            Debug.Log($"Card ID {__result.Id} selected.");

            // If card should be blocked, replace it with a random basic card.
            if (CommonPatchMethods.ShouldCardBeBlocked(__result.Id))
            {
                __result = new CardId(CommonPatchMethods.GetRandomBasicCard());
            }
        }
    }
}
