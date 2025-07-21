using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="InputController"/> class.
    /// </summary>
    [HarmonyPatch(typeof(InputController))]
    public class InputController_Patches
    {

        /// <summary>
        /// If the Time3 binding is triggered, intercept the value.
        /// </summary>
        [HarmonyPatch(nameof(InputController.Time3_Triggered))]
        [HarmonyPostfix]
        public static void OnTime3Triggered_InterceptPause(ref bool __result)
        {
            // Intercept result if set to true and pausing has been disabled
            __result = __result && StacklandsRandomizer.instance.IsPauseEnabled;
        }

        /// <summary>
        /// If a the time pause binding is triggered, intercept the value.
        /// </summary>
        [HarmonyPatch(nameof(InputController.TimePauseTriggered))]
        [HarmonyPostfix]
        public static void OnTimePauseTriggered_InterceptPause(ref bool __result)
        {
            // Intercept result if set to true and pausing has been disabled
            __result = __result && StacklandsRandomizer.instance.IsPauseEnabled;
        }
    }
}
