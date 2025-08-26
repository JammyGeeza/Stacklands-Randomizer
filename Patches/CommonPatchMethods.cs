using Stacklands_Randomizer_Mod.Constants;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Methods shared between patch classes.
    /// </summary>
    public static class CommonPatchMethods
    {
        /// <summary>
        /// List of basic cards on Mainland.
        /// </summary>
        public static readonly List<string> BASIC_CARDS = 
        [
            Cards.berrybush,
            Cards.berry,
            Cards.flint,
            Cards.gold,
            Cards.poop,
            Cards.rock,
            Cards.soil,
            Cards.tree,
            Cards.wood
        ];

        /// <summary>
        /// List of basic cards on The Island.
        /// </summary>
        public static readonly List<string> ISLAND_BASIC_CARDS =
        [
            Cards.banana_tree,
            Cards.banana,
            Cards.cotton_plant,
            Cards.cotton,
            Cards.flint,
            Cards.poop,
            Cards.sand,
            Cards.shell,
            Cards.wood
        ];

        /// <summary>
        /// List of all mobs that can be found on Mainland.
        /// </summary>
        public static readonly List<string> MAINLAND_MOBS = 
        [

        ];

        /// <summary>
        /// List of all mobs that can be found only in Dark Forest.
        /// </summary>
        public static readonly List<string> FOREST_MOBS = 
        [

        ];

        /// <summary>
        /// List of all mobs that can be found on The Island.
        /// </summary>
        public static readonly List<string> ISLAND_MOBS = 
        [

        ];

        public static readonly List<string> MAINLAND_PACKS = 
        [
            "basic",
            "idea",
            "farming",
            "cooking",
            "idea2",
            "equipment",
            "locations",
            "structures"
        ];

        public static readonly List<string> ISLAND_PACKS = [
            "island2",
            "island_ideas_1",
            "island_cooking",
            "island_ideas_2",
            "island_advanced",
            "island_locations"
        ];

        private static readonly string PREFIX_SAVE = "ap_";

        private static string SaveId => $"{PREFIX_SAVE}{StacklandsRandomizer.instance.Seed}";

        /// <summary>
        /// Check if a list contains all items in another list.
        /// </summary>
        /// <returns><see cref="true"/> if List A contains all of List B, <see cref="false"/> if not.</returns>
        public static bool ContainsAll<T>(this List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }

        /// <summary>
        /// Get the amount of times a booster pack has been bought in this run.
        /// </summary>
        /// <param name="boosterId">The ID of the booster pack.</param>
        /// <returns>Total amount of times a pack has been bought in this run.</returns>
        public static int GetTimesBoosterPackBought(string boosterId)
        {
            return WorldManager.instance.BoughtBoosterIds.Count(b => b == boosterId);
        }

        /// <summary>
        /// Get the total amount of times cards have been sold in this run.
        /// </summary>
        /// <returns>Total amount of times cards have been sold in this run.</returns>
        public static int GetTimesCardsSold()
        {
            return KeyValueHelper.GetExtraKeyValue(Terms.CardsSold);
        }

        /// <summary>
        /// Get the amount of times any booster packs for mainland have been bought in this run.
        /// </summary>
        /// <returns>Total amount of times any booster packs for Mainland have been bought in this run..</returns>
        public static int GetTimesIslandBoosterPacksBought()
        {
            return ISLAND_PACKS.Sum(GetTimesBoosterPackBought);
        }

        /// <summary>
        /// Get the amount of times any booster packs for mainland have been bought in this run.
        /// </summary>
        /// <returns>Total amount of times any booster packs for Mainland have been bought in this run..</returns>
        public static int GetTimesMainlandBoosterPacksBought()
        {
            return MAINLAND_PACKS.Sum(GetTimesBoosterPackBought);
        }

        /// <summary>
        /// Find an existing or create a new archipelago save.
        /// </summary>
        /// <param name="instance">The instance of <see cref="SaveManager"/> to refer to.</param>
        /// <returns>A found or newly created <see cref="SaveGame"/>.</returns>
        public static SaveGame FindOrCreateArchipelagoSave(SaveManager instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Attempting to retrieve Archipelago save...");

            // Check if archipelago save exists
            SaveGame save = SaveManager.LoadSaveFromFile(SaveId);
            if (save is null)
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Archipelago save not found. Creating a new one...");

                // Create a new save and copy its settings
                save = SaveGame.LoadFromString("", SaveId);
                save.DisabledMods = instance.CurrentSave.DisabledMods;
                save.ExtraKeyValues = instance.CurrentSave.ExtraKeyValues;

                // Save it
                instance.Save(save);
            }

            StacklandsRandomizer.instance.ModLogger.Log($"Re-routing to save '{SaveId}'...");

            // Return archipelago save
            return save;

        }

        /// <summary>
        /// Generate the ID of a random, basic card.
        /// </summary>
        /// <returns>A randomly generated card ID of a basic card.</returns>
        public static string GetRandomBasicCard()
        {
            return BASIC_CARDS.ElementAt(UnityEngine.Random.Range(0, BASIC_CARDS.Count));
        }

        /// <summary>
        /// Check whether a card should be blocked from spawning.
        /// </summary>
        /// <param name="cardId">The ID of the card to check.</param>
        /// <returns><see cref="true"/> if it should be blocked, <see cref="false"/> if it shouldn't.</returns>
        public static bool ShouldCardBeBlocked(string cardId)
        {
            // Block card if it exists as a mapped idea and has not yet been discovered
            return ItemMapping.Map.Exists(m => m.ItemType is ItemType.Idea && m.ItemId == cardId)
                && !ItemHelper.IsIdeaDiscovered(cardId);
        }
    }
}
