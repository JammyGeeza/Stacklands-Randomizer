using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class KeyValueHelper
    {
        /// <summary>
        /// Get a value from SaveExtraValueKeys by its key name.
        /// </summary>
        /// <param name="key">The key name of the value to retrieve.</param>
        /// <returns>The value for the key or -1 if failure.</returns>
        public static int GetExtraKeyValue(string key)
        {
            return WorldManager.instance.SaveExtraKeyValues.GetWithKey(key) is SerializedKeyValuePair { } skvp
                ? Convert.ToInt32(skvp.Value)
                : -1;
        }

        /// <summary>
        /// Set a value for a key in SaveExtraValueKeys.
        /// </summary>
        /// <param name="key">The key for the value.</param>
        /// <param name="value">The value number.</param>
        /// <returns>The newly set value or -1 on failure.</returns>
        public static int SetExtraKeyValue(string key, int value)
        {
            try
            {
                WorldManager.instance.SaveExtraKeyValues.SetOrAdd(key, value.ToString());

                // Save
                SaveManager.instance.Save(WorldManager.instance.CurrentSave);

                return value;
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"ERROR storing key value '{key}':{value}. Reason(s): {ex.Message}");
                return -1;
            }
        }
    }
}
