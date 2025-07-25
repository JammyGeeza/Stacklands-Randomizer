using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="CardBag"/> class.
    /// </summary>
    [HarmonyPatch(typeof(CreatePackLine))]
    public class CreatePackLine_Patches
    {
        /// <summary>
        /// Add custom booster boxes before creation.
        /// </summary>
        [HarmonyPatch(nameof(CreatePackLine.CreateBoosterBoxes))]
        [HarmonyPrefix]
        public static void OnCreateBoosterBoxes_AddCustomBoosters(CreatePackLine __instance, ref List<string> boosters, ref BoardCurrency currency)
        {
            // Insert booster pack if speedsanity enabled.
            if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off)
            {
                // If creating for mainland, insert spendsanity booster box
                if (currency == BoardCurrency.Gold && boosters.Contains("idea") && !boosters.Contains(ModBoosterPacks.spendsanity))
                {
                    boosters.Add(ModBoosterPacks.spendsanity);
                }

                // Set as unlocked if not already
                if (!WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(ModBoosterPacks.spendsanity))
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(ModBoosterPacks.spendsanity);
                }
            }
        }

        /// <summary>
        /// Adjust custom booster boxes after creation.
        /// </summary>
        [HarmonyPatch(nameof(CreatePackLine.CreateBoosterBoxes))]
        [HarmonyPostfix]
        public static void OnCreateBoosterBoxes_AdjustCustomBoosters(CreatePackLine __instance)
        {
            // TODO: Set cost on creation based on how many times previously purchased
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(CreatePackLine)}.{nameof(CreatePackLine.CreateBoosterBoxes)} Postfix!");

            foreach (BuyBoosterBox bb in WorldManager.instance.AllBoosterBoxes)
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Booster box: {bb.BoosterId}");
            }

            //// Attempt to get spendsanity booster
            //if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off)
            //{
            //    BuyBoosterBox spendsanityBooster = WorldManager.instance.AllBoosterBoxes.FirstOrDefault(bb => bb.BoosterId == ModBoosterPacks.spendsanity);

            //    // Get last stored amount for booster
            //    int lastStoredAmount = CommonPatchMethods.GetLastBoosterPackStoredamount(ModBoosterPacks.spendsanity);

            //    StacklandsRandomizer.instance.ModLogger.Log($"Last known stored amount of '{ModBoosterPacks.spendsanity}': {lastStoredAmount}");

            //    // Set last stored amount in booster box
            //    spendsanityBooster.StoredCostAmount = lastStoredAmount > 0
            //        ? lastStoredAmount
            //        : 0;

                //    // Get last known cost of the booster box
                //    int lastCost = CommonPatchMethods.GetLastBoosterPackCost(ModBoosterPacks.spendsanity);

                //    StacklandsRandomizer.instance.ModLogger.Log($"Last known cost of '{ModBoosterPacks.spendsanity}': {lastCost}");
                //    StacklandsRandomizer.instance.ModLogger.Log($"Last known stored amount of '{ModBoosterPacks.spendsanity}': {lastStoredAmount}");

                //    //switch (StacklandsRandomizer.instance.Options.Spendsanity)
                //    //{
                //    //    case Spendsanity.Fixed:
                //    //        {
                //    //            StacklandsRandomizer.instance.ModLogger.Log($"Setting to fixed cost.");

                //    //            // Set fixed cost to last known cost or configured cost if zero or less
                //    //            spendsanityBooster.Cost = lastCost > 0 
                //    //                ? lastCost 
                //    //                : StacklandsRandomizer.instance.Options.SpendsanityCost;
                //    //        }
                //    //        break;

                //    //    case Spendsanity.Incremental:
                //    //        {
                //    //            // Check if last cost is known
                //    //            if (lastCost > 0)
                //    //            {
                //    //                StacklandsRandomizer.instance.ModLogger.Log($"Setting to last cost.");

                //    //                // Set last known cost
                //    //                spendsanityBooster.Booster.Cost = lastCost;
                //    //            }
                //    //            else
                //    //            {
                //    //                int boughtCount = CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity);
                //    //                int multiplier = Math.Max(1, boughtCount);

                //    //                StacklandsRandomizer.instance.ModLogger.Log($"Setting to Incremental cost.");

                //    //                // Set calculated incremental cost
                //    //                spendsanityBooster.Booster.Cost = StacklandsRandomizer.instance.Options.SpendsanityCost * multiplier;
                //    //            }
                //    //        }
                //    //        break;
                //    //}

                //    // Set the last known cost (or set to configured cost if zero or less
                //    spendsanityBooster.Cost = lastCost > 0
                //        ? lastCost
                //        : StacklandsRandomizer.instance.Options.SpendsanityCost;

                //    // Set the last known stored amount
                //    spendsanityBooster.StoredCostAmount = lastStoredAmount > 0
                //        ? lastStoredAmount
                //        : 0;
                //}
            //}
        }
    }
}
