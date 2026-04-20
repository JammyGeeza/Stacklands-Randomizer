using HarmonyLib;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="BuyBoosterBox"/> class.
    /// </summary>
    [HarmonyPatch(typeof(BuyBoosterBox))]
    public class BuyBoosterBox_Patches
    {
        /// <summary>
        /// Intercept mod boxes from receiving cards.
        /// </summary>
        [HarmonyPatch(nameof(BuyBoosterBox.CanHaveCard))]
        [HarmonyPrefix]
        public static bool OnCanHaveCard_PreventCards(BuyBoosterBox __instance, ref bool __result)
        {
            // If spendsanity is on and this booster box is the spendsanity box
            if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off && __instance.BoosterId == ModBoosterPacks.spendsanity)
            {
                // Reject all cards if maximum purchase count reached
                __result =
                    CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) < StacklandsRandomizer.instance.Options.SpendsanityCount;

                // Prevent source method from being called
                return false;
            }

            // Continue as normal
            return true;
        }

        /// <summary>
        /// Prevent locked booster packs from displaying 'Complete X quests to unlock' when hovered.
        /// </summary>
        [HarmonyPatch("GetTooltipText")]
        [HarmonyPrefix]
        public static bool OnGetTooltipText_Postfix(BuyBoosterBox __instance, ref string __result)
        {
            if (__instance.Booster.IsUnlocked)
            {
                string value = Icons.Gold;
                if (__instance.BoardCurrency == BoardCurrency.Shell)
                {
                    value = Icons.Shell;
                }
                else if (__instance.BoardCurrency == BoardCurrency.Dollar)
                {
                    value = Icons.Dollar;
                }

                __result = SokLoc.Translate("label_drag_coins_to_buy_pack", LocParam.Create("goldicon", value), LocParam.Create("cost", __instance.GetCost().ToString())) + "\n\n" + __instance.Booster.GetSummary();
            }
            else
            {
                // Get booster pack item name
                string itemName = ItemMapping.Map.FirstOrDefault(itm => itm.ItemId == __instance.BoosterId).Name;
                __result = SokLoc.Translate("label_receive_item_for_pack", LocParam.Create("item", itemName));
            }

            return false;
        }

        /// <summary>
        /// Intercept mod booster boxes from dispensing packs.
        /// </summary>
        [HarmonyPatch("CreateBoosterPack")]
        [HarmonyPrefix]
        public static bool OnCreateBoosterPack_PreventBoosterPack(BuyBoosterBox __instance)
        {
            // If booster box is for the AP Check booster pack
            if (__instance.BoosterId == ModBoosterPacks.spendsanity)
            {
                // Add as a bought booster
                WorldManager.instance.BoughtBoosterIds.Add(ModBoosterPacks.spendsanity);

                // Get total bought count for this pack
                int boughtCount = WorldManager.instance.BoughtBoosterIds.Count(b => b == ModBoosterPacks.spendsanity);

                // Fire special action to trigger check
                QuestManager.instance.SpecialActionComplete($"buy_{ModBoosterPacks.spendsanity}_pack");

                // Check if maximum purchase count not yet reached
                if (boughtCount < StacklandsRandomizer.instance.Options.SpendsanityCount)
                {
                    switch (StacklandsRandomizer.instance.Options.Spendsanity)
                    {
                        case Spendsanity.Incremental:
                            {
                                // Incrementally increase cost if configured to do so
                                __instance.Cost += StacklandsRandomizer.instance.Options.SpendsanityCost;
                            }
                            break;
                    }
                }

                // Prevent source method from being invoked
                return false;
            }

            // Continue as normal
            return true;
        }
    }
}
