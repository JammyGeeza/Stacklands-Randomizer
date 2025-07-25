using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class KeyValueHelper
    {
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
    }
}
