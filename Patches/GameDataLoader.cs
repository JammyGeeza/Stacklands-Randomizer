using HarmonyLib;
using Stacklands_Randomizer_Mod.Constants;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="GameDataLoader"/> class.
    /// </summary>
    [HarmonyPatch(typeof(GameDataLoader))]
    public class GameDataLoader_Patches
    {
        /// <summary>
        /// When booster data is being retrieved, adjust data where necessary.
        /// </summary>
        [HarmonyPatch(nameof(GameDataLoader.GetBoosterData))]
        [HarmonyPostfix]
        public static void OnGetBoosterData_AdjustBoosterData(GameDataLoader __instance, ref string boosterId, ref BoosterpackData __result)
        {
            // If booster ID is the spendsanity pack
            if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off && boosterId == ModBoosterPacks.spendsanity)
            {
                // Set cost based on configuration
                __result.Cost = StacklandsRandomizer.instance.Options.Spendsanity switch
                {
                    Spendsanity.Fixed => StacklandsRandomizer.instance.Options.SpendsanityCost,
                    Spendsanity.Incremental => StacklandsRandomizer.instance.Options.SpendsanityCost * (CommonPatchMethods.GetTimesBoosterPackBought(boosterId) + 1)
                };
            }
        }
    }
}
