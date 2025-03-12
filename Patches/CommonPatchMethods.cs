﻿using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Methods shared between patch classes.
    /// </summary>
    public static class CommonPatchMethods
    {
        // Private Member(s)
        private static readonly List<string> BASIC_CARDS = [
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

        private static readonly string PREFIX_SAVE = "ap_";

        private static string SaveId => $"{PREFIX_SAVE}{StacklandsRandomizer.instance.Seed}";

        /// <summary>
        /// Find an existing or create a new archipelago save.
        /// </summary>
        /// <param name="instance">The instance of <see cref="SaveManager"/> to refer to.</param>
        /// <returns>A found or newly created <see cref="SaveGame"/>.</returns>
        public static SaveGame FindOrCreateArchipelagoSave(SaveManager instance)
        {
            Debug.Log($"Attempting to retrieve Archipelago save...");

            // Check if archipelago save exists
            SaveGame save = SaveManager.LoadSaveFromFile(SaveId);
            if (save is null)
            {
                Debug.Log($"Archipelago save not found. Creating a new one...");

                // Create a new save and copy its settings
                save = SaveGame.LoadFromString("", SaveId);
                save.DisabledMods = instance.CurrentSave.DisabledMods;
                save.ExtraKeyValues = instance.CurrentSave.ExtraKeyValues;

                // Save it
                instance.Save(save);
            }

            Debug.Log($"Re-routing to save '{SaveId}'...");

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
            // Get card data
            CardData cardData = WorldManager.instance.GetCardPrefab(cardId, false);

            // Block card if it exists as a mapped idea and has not yet been discovered
            return ItemMapping.Map.Exists(m => m.ItemType is ItemType.Idea && m.ItemId == cardId)
                && !ItemHandler.IsIdeaDiscovered(cardId);
        }

        /// <summary>
        /// Check whether a booster pack should be blocked from spawning.
        /// </summary>
        /// <param name="boosterId">The ID of the booster pack to check.</param>
        /// <returns><see cref="true"/> if it should be blocked, <see cref="false"/> if it shouldn't.</returns>
        public static bool ShouldBoosterPackBeBlocked(string boosterId)
        {
            return boosterId == "new_weaponry";
        }
    }
}
