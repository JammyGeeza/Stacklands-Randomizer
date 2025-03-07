using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Methods shared between patch classes.
    /// </summary>
    public static class CommonPatchMethods
    {
        // Private Member(s)
        private static readonly List<string> BASIC_CARDS = [
            Cards.apple,
            Cards.apple_tree,
            Cards.berrybush,
            Cards.berry,
            Cards.bone,
            Cards.carrot,
            Cards.gold,
            Cards.goop,
            Cards.iron_deposit,
            Cards.poop,
            Cards.rock,
            Cards.tree
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

            // Block if card is from Mainland, is an Idea and has not yet been discovered.
            return cardData.CardUpdateType switch
            {
                // If from Mainland, block if it is an Idea and has not yet been discovered
                CardUpdateType.Main => cardData.MyCardType is CardType.Ideas && !ItemHandler.IsIdeaDiscovered(cardId),

                // If from Island, block if it is an Idea and the current goal does not require the island
                CardUpdateType.Island => cardData.MyCardType is CardType.Ideas && StacklandsRandomizer.instance.CurrentGoal.Type is not GoalType.KillDemonLord,

                _ => false // Block all other card update types
            };
        }
    }
}
