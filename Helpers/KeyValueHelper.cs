using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class KeyValueHelper
    {
        private static readonly string GOAL_QUESTS_KEY = "CompletedGoalQuests";

        /// <summary>
        /// Get a value from the round's extra key values by its key name.
        /// </summary>
        /// <param name="key">The key name of the value to retrieve.</param>
        /// <returns>The value for the key - returns 0 if not found.</returns>
        public static int GetExtraKeyValue(string key)
        {
            return WorldManager.instance.RoundExtraKeyValues.GetWithKey(key) is SerializedKeyValuePair { } skvp
                ? Convert.ToInt32(skvp.Value)
                : 0;
        }

        /// <summary>
        /// Set a value for a key in the round's extra key values.
        /// </summary>
        /// <param name="key">The key for the value.</param>
        /// <param name="value">The value number.</param>
        /// <returns>The newly set value - returns 0 if not set.</returns>
        public static int SetExtraKeyValue(string key, int value)
        {
            try
            {
                WorldManager.instance.RoundExtraKeyValues.SetOrAdd(key, value.ToString());
                return value;
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"ERROR storing key value '{key}':{value}. Reason(s): {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Get the current completed Goal Quest IDs.
        /// </summary>
        /// <returns>A populated list of stored, completed goal quest IDs for the current round.</returns>
        public static List<string> GetCompletedGoalQuests()
        {
            try
            {
                return (WorldManager.instance.SaveExtraKeyValues.GetWithKey(GOAL_QUESTS_KEY)?.Value ?? string.Empty)
                    .Split(',')
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Append a Quest ID to the completed goal quests key-value.
        /// </summary>
        /// <param name="questId">The ID of the completed quest.</param>
        public static void AppendCompletedGoalQuest(string questId)
        {
            List<string> completed = GetCompletedGoalQuests();

            if (!completed.Contains(questId))
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Adding goal quest '{questId}' to completed goal quests");

                completed.Add(questId);
                WorldManager.instance.SaveExtraKeyValues.SetOrAdd(GOAL_QUESTS_KEY, string.Join(",", completed));
            }

            completed = GetCompletedGoalQuests();
            StacklandsRandomizer.instance.ModLogger.Log($"Completed quests: {string.Join(',', completed)}");
        }
    }
}
