using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="RunOptionsScreen"/> class.
    /// </summary>
    [HarmonyPatch(typeof(RunOptionsScreen))]
    public class RunOptionsScreen_Patches
    {
        /// <summary>
        /// Add an event handler to the quit button to disconnect session.
        /// </summary>
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void OnAwake_Setup(RunOptionsScreen __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(RunOptionsScreen)}.Awake prefix!");

            // Disable all moon selection buttons
            __instance.ShortMoon.ButtonEnabled = false;
            __instance.NormalMoon.ButtonEnabled = false;
            __instance.LongMoon.ButtonEnabled = false;

            // Set moon length from session options
            __instance.CurMoonLength = StacklandsRandomizer.instance.Options.MoonLength;

            // Disable peaceful mode selection buttons 
            __instance.PeacefulModeOn.ButtonEnabled = false;
            __instance.PeacefulModeOff.ButtonEnabled = false;

            // Set peaceful mode to off (no current support for peaceful mode)
            __instance.PeacefulMode = false;

            // Disable curse selection buttons
            __instance.DeathButton.ButtonEnabled = false;
            __instance.HappinessButton.ButtonEnabled = false;

            // Set curses to off (no current support for curses)
            __instance.EnableHappiness = false;
            __instance.EnableDeath = false;
        }
    }
}
