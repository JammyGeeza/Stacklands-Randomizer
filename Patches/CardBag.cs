using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="CardBag"/> class.
    /// </summary>
    [HarmonyPatch(typeof(CardBag))]
    public class CardBag_Patches
    {
        /// <summary>
        /// Replace blueprints in card bags with basic cards for booster box hover-over dialog.
        /// </summary>
        [HarmonyPatch(nameof(CardBag.GetCardsInBag), [typeof(GameDataLoader)])]
        [HarmonyPostfix]
        public static void OnGetCardsInBag_ReplaceBlueprints(ref GameDataLoader loader, ref List<string> __result)
        {
            // Remove all cards that need to be blocked
            __result.RemoveAll(c => CommonPatchMethods.ShouldCardBeBlocked(c));

            // Get relevant basic cards mobs depending on current board
            List<string> basicCards = WorldManager.instance.CurrentBoard.Location switch
            {
                Location.Mainland => CommonPatchMethods.MAINLAND_BASIC_CARDS,
                Location.Island => CommonPatchMethods.ISLAND_BASIC_CARDS,
                _ => new List<string>()
            };

            // Add all basic cards
            foreach (string card in basicCards)
            {
                if (!__result.Contains(card))
                {
                    __result.Add(card);
                }
            }

            // If mobsanity AND mobsanity balancing are enabled...
            if (StacklandsRandomizer.instance.Options.MobsanityEnabled && StacklandsRandomizer.instance.Options.MobsanityBalancingEnabled)
            {
                // Get relevant mobsanity mobs depending on current board
                List<string> mobsanityMobs = WorldManager.instance.CurrentBoard.Location switch
                {
                    Location.Mainland => CommonPatchMethods.MAINLAND_MOBSANITY.Keys.ToList(),
                    Location.Island => CommonPatchMethods.ISLAND_MOBSANITY.Keys.ToList(),
                    _ => new List<string>()
                };

                // If pack contains any mobsanity mobs for the current board, add all potential mobs to pack
                if (__result.Any(c => mobsanityMobs.Contains(c)))
                {
                    foreach (string mob in mobsanityMobs)
                    {
                        if (!__result.Contains(mob))
                        {
                            __result.Add(mob);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replace blueprints with basic cards.
        /// </summary>
        [HarmonyPatch("GetRawCardChanges")]
        [HarmonyPostfix]
        public static void OnGetRawCardChanges_ReplaceCards(ref SetCardBagType bag, ref List<CardChance> __result)
        {
            // If Enemies card bag and Mobsanity is enabled...
            if (StacklandsRandomizer.instance.Options.MobsanityEnabled && StacklandsRandomizer.instance.Options.MobsanityBalancingEnabled && CommonPatchMethods.IsEnemyCardBagType(bag))
            {
                // Get mobs to replace
                List<CardChance> mobsToReplace = __result
                    .Where(cc => CommonPatchMethods.GetTimesMobKilled(cc.Id) > 0)
                    .ToList();

                //StacklandsRandomizer.instance.ModLogger.LogWarning($"Found {mobsToReplace.Count()} mobs in card bag to be replaced by un-killed Mobsanity mobs.");

                // If mobs need replacing...
                if (mobsToReplace.Count > 0)
                {
                    List<string> unkilledMobs = new List<string>();

                    if (WorldManager.instance.CurrentBoard.Location == Location.Mainland)
                    {
                        // Get all mobs from Mainland that have not been killed and would typically spawn after the current month
                        unkilledMobs.AddRange(
                            CommonPatchMethods.MAINLAND_MOBSANITY.Where(kvp =>
                                CommonPatchMethods.GetTimesMobKilled(kvp.Key) == 0
                                && WorldManager.instance.CurrentMonth >= kvp.Value)
                                .Select(kvp => kvp.Key)
                                .ToList());
                    }
                    else if (WorldManager.instance.CurrentBoard.Location == Location.Forest)
                    {
                        // Get all mobs from Mainland that have not been killed and would typically spawn after the current month
                        unkilledMobs.AddRange(
                            CommonPatchMethods.FOREST_MOBSANITY.Where(kvp =>
                                CommonPatchMethods.GetTimesMobKilled(kvp.Key) == 0
                                && WorldManager.instance.CurrentRunVariables.ForestWave >= kvp.Value)
                                .Select(kvp => kvp.Key)
                                .ToList());
                    }
                    else if (WorldManager.instance.CurrentBoard.Location == Location.Island)
                    {
                        // Get all mobs from The Island that have not been killed and would typically spawn after the current month
                        unkilledMobs.AddRange(
                            CommonPatchMethods.ISLAND_MOBSANITY.Where(kvp =>
                                CommonPatchMethods.GetTimesMobKilled(kvp.Key) == 0
                                && WorldManager.instance.CurrentMonth >= kvp.Value)
                                .Select(kvp => kvp.Key)
                                .ToList());
                    }

                    //StacklandsRandomizer.instance.ModLogger.Log($"Found {unkilledMobs.Count()} un-killed, reachable mobs remaining for Mobsanity.");

                    // If any un-killed mobs remain, attempt to replace
                    if (unkilledMobs.Count > 0)
                    {
                        foreach (CardChance chance in mobsToReplace)
                        {
                            // Swap mob ID with one from the currently un-killed mobs
                            __result[__result.IndexOf(chance)].Id = unkilledMobs[UnityEngine.Random.Range(0, unkilledMobs.Count)];
                        }

                        //StacklandsRandomizer.instance.ModLogger.Log($"Replaced {mobsToReplace.Count()} mobs in bag.");
                    }
                }
            }

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
