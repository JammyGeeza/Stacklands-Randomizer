using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Methods shared between patch classes.
    /// </summary>
    public static class CommonMethods
    {
        private static readonly string SAVE_PREFIX = "ap_";
        private static string SaveId => $"{SAVE_PREFIX}{StacklandsRandomizer.instance.Seed}";

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
    }
}
