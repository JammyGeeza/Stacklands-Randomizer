using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="GameScreen"/> class.
    /// </summary>
    [HarmonyPatch(typeof(GameScreen))]
    public class GameSceeen_Patches
    {
        /// <summary>
        /// Prevent automatic 'Quest Completed' notifications, as we will be sending our own.
        /// </summary>
        [HarmonyPatch(nameof(GameScreen.AddNotification))]
        [HarmonyPrefix]
        public static bool OnAddNotification_PreventQuestCompletedNotification(ref string title, ref string text)
        {
            // Disable if title contains quest completion
            return !title.Equals(SokLoc.Translate("label_quest_completed"));
        }

        /// <summary>
        /// Add an event handler to the game speed button click to override pause.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void OnAwake_OverridePause(GameScreen __instance)
        {
            Debug.Log($"{nameof(GameScreen)}.Awake Postfix!");

            // Is pause disabled?
            if (!StacklandsRandomizer.instance.IsPauseEnabled)
            {
                // Add an additional handler to game speed button click
                __instance.GameSpeedButton.Clicked += delegate
                {
                    // If speed is due to toggle to 0f next...
                    if (WorldManager.instance.SpeedUp == 5f)
                    {
                        // Set to 0f already so that it skips time pause
                        WorldManager.instance.SpeedUp = 0f;
                    }
                };
            }
        }
    }
}
