using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod.Patches
{
    /// <summary>
    /// Patches for the <see cref="Demon"/> class.
    /// </summary>
    [HarmonyPatch(typeof(Demon))]
    public class Demon_Patches
    {
        /// <summary>
        /// Trigger the special action completed when a Demon or Demon Lord dies, as currently it does not which prevents the location check from completing.
        /// </summary>
        [HarmonyPatch(nameof(Demon.Die))]
        [HarmonyPostfix]
        public static void OnDie_TriggerSpecialAction(Demon __instance)
        {
            QuestManager.instance.SpecialActionComplete($"{__instance.Id}_killed", __instance);
        }
    }
}
