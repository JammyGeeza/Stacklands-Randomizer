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
            Debug.Log($"{nameof(QuestManager)}.{nameof(QuestManager.ActionComplete)} Postfix!");
            Debug.Log($"CardData: {card.Name}");
            Debug.Log($"Action: {action}");
        }

        /// <summary>
        /// Unlock a booster pack if it has been unlocked from an archipelago location.
        /// (This runs every frame)
        /// </summary>
        [HarmonyPatch(nameof(QuestManager.BoosterIsUnlocked))]
        [HarmonyPostfix]
        public static void OnBoosterIsUnlocked_UnlockIfReceived(BoosterpackData p, bool allowDebug, ref bool __result)
        {
            __result = ItemHandler.IsBoosterPackDiscovered(p.BoosterId);
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
        [HarmonyPostfix]
        public static void OnSpecialActionComplete_Intercept(string action, CardData card = null)
        {
            if (action != "pause_game") // <- Prevents it constantly printing on pause
            {
                Debug.Log($"{nameof(QuestManager)}.{nameof(QuestManager.SpecialActionComplete)} Postfix!");
                Debug.Log($"CardData: {card?.Name}");
                Debug.Log($"Special Action: {action}");
            }
        }
    }
}
