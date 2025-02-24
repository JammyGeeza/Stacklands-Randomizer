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
        /// Replace blueprints with basic cards.
        /// </summary>
        [HarmonyPatch(nameof(CardBag.GetCardsInBag), [typeof(GameDataLoader)])]
        [HarmonyPostfix]
        public static void OnGetCardsInBag_ReplaceBlueprints(ref GameDataLoader loader, ref List<string> __result)
        {
            List<string> toReplace = __result
                .Where(c => c.Contains($"blueprint_"))
                .ToList();

            foreach (string card in toReplace)
            {
                // Swap blueprint ID with a basic card ID
                __result[__result.IndexOf(card)] = StacklandsRandomizer.instance.GetRandomBasicCard();
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
                .Where(c => c.Id.Contains($"blueprint_"))
                .ToList();

            foreach (CardChance chance in toReplace)
            {
                // Swap blueprint ID with a basic card ID
                __result[__result.IndexOf(chance)].Id = StacklandsRandomizer.instance.GetRandomBasicCard();
            }
        }
    }
}
