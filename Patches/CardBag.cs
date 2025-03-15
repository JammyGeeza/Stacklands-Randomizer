using HarmonyLib;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="CardBag"/> class.
    /// </summary>
    [HarmonyPatch(typeof(CardBag))]
    public class CardBag_Patches
    {
        /// <summary>
        /// Replace blueprints in card bags with basic cards.
        /// </summary>
        [HarmonyPatch(nameof(CardBag.GetCardsInBag), [typeof(GameDataLoader)])]
        [HarmonyPostfix]
        public static void OnGetCardsInBag_ReplaceBlueprints(ref GameDataLoader loader, ref List<string> __result)
        {
            // Remove all cards that need to be blocked
            __result.RemoveAll(c => CommonPatchMethods.ShouldCardBeBlocked(c));

            // Add all basic cards
            foreach (string card in CommonPatchMethods.BASIC_CARDS)
            {
                if (!__result.Contains(card))
                {
                    __result.Add(card);
                }
            }
        }

        /// <summary>
        /// Replace blueprints with basic cards.
        /// </summary>
        [HarmonyPatch("GetRawCardChanges")]
        [HarmonyPostfix]
        public static void OnGetRawCardChanges_ReplaceBlueprints(ref List<CardChance> __result)
        {
            List<CardChance> toReplace = __result
                .Where(c => CommonPatchMethods.ShouldCardBeBlocked(c.Id))
                .ToList();

            foreach (CardChance chance in toReplace)
            {
                // Swap blueprint ID with a basic card ID
                __result[__result.IndexOf(chance)].Id = CommonPatchMethods.GetRandomBasicCard();
            }
        }
    }
}
