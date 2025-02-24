using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="MainMenu"/> class.
    /// </summary>
    [HarmonyPatch(typeof(MainMenu))]
    public class MainMenu_Patches
    {
        /// <summary>
        /// Add an event handler to the quit button to disconnect session.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static void OnAwake_Setup(ref MainMenu __instance)
        {
            Debug.Log($"{nameof(MainMenu)}.Awake prefix!");

            // Add an event handler to disconnect the session when quitting the game
            __instance.QuitButton.Clicked += () =>
            {
                StacklandsRandomizer.instance.Disconnect();
            };
        }

        /// <summary>
        /// Add an event handler to the quit button to disconnect session.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void OnAwake_DisplayConnectionStatus(ref MainMenu __instance)
        {
            Debug.Log($"{nameof(MainMenu)}.Awake postfix!");

            // Display if we are connected
            if (StacklandsRandomizer.instance.IsConnected)
            {
                __instance.UpdateText.text = "Connected to Archipelago";
            }
            else
            {
                __instance.UpdateText.text = "Not Connected to Archipelago";
            }
        }
    }
}
