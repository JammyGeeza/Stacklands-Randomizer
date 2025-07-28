using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="QuestManager"/> class.
    /// </summary>
    [HarmonyPatch(typeof(QuestManager))]
    public class QuestManager_Patches
    {
        /// <summary>
        /// Intercept actions when they are completed.
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.ActionComplete))]
        [HarmonyPostfix]
        public static void OnActionComplete_Intercept(CardData card, string action, CardData focusCard = null)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(QuestManager)}.{nameof(QuestManager.ActionComplete)} Postfix!");
            StacklandsRandomizer.instance.ModLogger.Log($"CardData: {card.Name}");
            StacklandsRandomizer.instance.ModLogger.Log($"Action: {action}");
        }

        /// <summary>
        /// Unlock a booster pack if it has been unlocked from an archipelago location.
        /// (This runs every frame)
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.BoosterIsUnlocked))]
        [HarmonyPostfix]
        public static void OnBoosterIsUnlocked_UnlockIfReceived(BoosterpackData p, bool allowDebug, ref bool __result)
        {
            __result = ItemHelper.IsBoosterPackDiscovered(p.BoosterId);

            // If booster has been unlocked...
            if (__result)
            {
                // If not yet completed 'Unlock All Packs' quest and all mainland packs have been discovered, trigger special action for quest.
                if (!WorldManager.instance.CurrentSave.CompletedAchievementIds.Contains(AllQuests.UnlockAllPacks.Id) && WorldManager.instance.CurrentSave.FoundBoosterIds.ContainsAll(CommonPatchMethods.MAINLAND_PACKS))
                {
                    QuestManager.instance.SpecialActionComplete("unlocked_all_packs");
                }

                // All Island Packs quest will go here when Island is supported
            }
        }

        /// <summary>
        /// Add custom quests to the list of all quests.
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.GetAllQuests))]
        [HarmonyPostfix]
        public static void OnGetAllQuests_AddCustomQuests(ref List<Quest> __result)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(QuestManager)}.{nameof(QuestManager.GetAllQuests)} Postfix!");

            // If pausing is disabled, remove pausing quest
            if (!StacklandsRandomizer.instance.Options.PauseTimeEnabled)
            {
                StacklandsRandomizer.instance.ModLogger.Log("Removing pausing quest...");

                // Remove pausing quest
                __result.Remove(AllQuests.PauseGame);
            }

            // If mobsanity enabled, add mobsanity quests
            if (StacklandsRandomizer.instance.Options.MobsanityEnabled)
            {
                StacklandsRandomizer.instance.ModLogger.Log("Inserting Mobsanity quests...");

                // Add mobsanity quests (and only include Dark Forest ones if enabled)
                __result.AddRange(CustomQuestMapping.Map.Where(q => q.QuestGroup == EnumExtensionHandler.MobsanityQuestGroupEnum 
                    && (StacklandsRandomizer.instance.Options.QuestChecks.HasFlag(QuestCheckFlags.Forest) || q.QuestLocation != Location.Forest)));
            }

            // If packsanity enabled, add packsanity quests
            if (StacklandsRandomizer.instance.Options.PacksanityEnabled)
            {
                StacklandsRandomizer.instance.ModLogger.Log("Inserting Packsanity quests...");

                // Add packsanity quests
                __result.AddRange(CustomQuestMapping.Map.Where(q => q.QuestGroup == EnumExtensionHandler.PacksanityQuestGroupEnum
                    && q.QuestLocation == Location.Mainland));
            }

            // If spendsanity enabled, add spendsanity quests
            if (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off)
            {
                StacklandsRandomizer.instance.ModLogger.Log("Inserting Spendsanity quests...");

                // Add spendsanity quests (up to the amount 
                __result.AddRange(CustomQuestMapping.Map.Where(q => q.QuestGroup == EnumExtensionHandler.SpendsanityQuestGroupEnum
                    && q.QuestLocation == Location.Mainland
                    && q.RequiredCount <= StacklandsRandomizer.instance.Options.SpendsanityCount));
            }
        }

        /// <summary>
        /// Prevent the game from unlocking booster packs.
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.JustUnlockedPack))]
        [HarmonyPostfix]
        public static void OnJustUnlockedPack_PreventUnlock(ref BoosterpackData __result)
        {
            __result = null;
        }


        /// <summary>
        /// Show all quests to user in the left-hand bar.
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.QuestIsVisible))]
        [HarmonyPostfix]
        public static void OnQuestIsVisible_ShowAllQuests(ref bool __result)
        {
            __result = true;
        }

        /// <summary>
        /// Intercept special actions when they are completed.
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.SpecialActionComplete))]
        [HarmonyPrefix]
        public static bool OnSpecialActionComplete_InterceptPre(string action, CardData card = null)
        {
            if (action != "pause_game") // <- Prevents it constantly printing every frame on pause
            {
                StacklandsRandomizer.instance.ModLogger.Log($"{nameof(QuestManager)}.{nameof(QuestManager.SpecialActionComplete)} Prefix!");
                StacklandsRandomizer.instance.ModLogger.Log($"CardData: {card?.Name}");
                StacklandsRandomizer.instance.ModLogger.Log($"Special Action: {action}");

                // If 'Unlock All Booster Packs' action and not all boosters have been found, block it
                if ((action is "unlock_all_packs" or "unlocked_all_packs") && !WorldManager.instance.CurrentSave.FoundBoosterIds.ContainsAll(CommonPatchMethods.MAINLAND_PACKS))
                {
                    StacklandsRandomizer.instance.ModLogger.Log($"Intercepting '{action}' - not all mainland packs have been discovered yet.");
                    return false;
                }
            }

            return true;
        }
    }
}
