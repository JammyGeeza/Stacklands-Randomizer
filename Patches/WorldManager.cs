using HarmonyLib;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="WorldManager"/> class.
    /// </summary>
    [HarmonyPatch(typeof(WorldManager))]
    public class WorldManager_Patches
    {
        private static bool _autoPauseControllerValue = false;
        private static bool _autoPauseKeyboardValue = false;
        private static bool _isNewRun = false;

        /// <summary>
        /// When worldmanager initializing, set current save to an archipelago one.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static void OnAwake_Setup(WorldManager __instance)
        {
            Debug.Log($"{nameof(WorldManager)}.Awake Postfix!");
            Debug.Log($"Determined save ID: {SaveManager.instance.CurrentSave.SaveId}...");

            // Set current save to Archipelago save
            SaveManager.instance.CurrentSave = CommonPatchMethods.FindOrCreateArchipelagoSave(SaveManager.instance);
        }

        /// <summary>
        /// When a save is cleared, re-sync all received items from server.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(WorldManager.ClearSaveAndRestart))]
        [HarmonyPostfix]
        public static void OnClearSaveAndRestart_SyncItemsWithServer(WorldManager __instance)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.ClearSaveAndRestart)} Postfix!");

            // Re-sync all items with server
            StacklandsRandomizer.instance.SyncAllReceivedItems(true);
        }

        /// <summary>
        /// When receiving a random card, replace blueprints with a basic card.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.GetRandomCard))]
        [HarmonyPrefix]
        public static void OnGetRandomCard_ReplaceBlueprint(ref List<CardChance> chances, ref bool removeCard)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.GetRandomCard)} Prefix!");

            foreach (CardChance chance in chances)
            {
                if (CommonPatchMethods.ShouldCardBeBlocked(chance.Id))
                {
                    chance.Id = CommonPatchMethods.GetRandomBasicCard();
                }
            }
        }

        /// <summary>
        /// When the pause menu is showing, override the 'IsPlaying' property (if required) to ensure time continues.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.IsPlaying), MethodType.Getter)]
        [HarmonyPostfix]
        public static void OnIsPlaying(WorldManager __instance, ref bool __result)
        {
            // Check if pausing is disabled...
            if (!StacklandsRandomizer.instance.IsPauseEnabled)
            {
                // If the game is currently paused...
                if (__instance.CurrentGameState is WorldManager.GameState.Paused)
                {
                    // Pretend it isn't
                    __result = true;
                }
            }
        }

        /// <summary>
        /// When a villager is killed, trigger a DeathLink (if this was not already caused by a DeathLink trigger in the first place)
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.KillVillagerCoroutine))]
        [HarmonyPostfix]
        public static void OnKillVillagerCoroutine_SendDeathLink(WorldManager __instance, ref Combatable combatable)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.KillVillagerCoroutine)} Postfix!");

            // Check if object is definitely a villager
            if (combatable is BaseVillager baseVillager)
            {
                Debug.Log($"Villager '{baseVillager.Name}' has died!");

                // If this death was not triggered by a deathlink, send deathlink
                if (!StacklandsRandomizer.instance.HandlingDeathLink)
                {
                    StacklandsRandomizer.instance.SendDeathlink($"their {combatable.Name} dying.");
                }

                // Set block back to false
                StacklandsRandomizer.instance.HandlingDeathLink = false;
            }
        }

        /// <summary>
        /// When a game is loaded, send all currently completed locations and retrieve all locations.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.LoadSaveRound))]
        [HarmonyPostfix]
        public static void OnLoadSaveRound_SendAllCompletedLocations(WorldManager __instance)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.LoadSaveRound)} Prefix!");

            // Set 'is new game' to false
            _isNewRun = false;
        }

        /// <summary>
        /// When a game begins, send all completed locations to server and retrieve items for all completed locations
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(WorldManager.Play))]
        [HarmonyPostfix]
        public static async void OnPlay_SyncLocations(WorldManager __instance)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.Play)} Postfix!");

            // Send all currently completed locations
            await StacklandsRandomizer.instance.SendAllCompletedLocations();

            // If new run started, re-spawn all received items.
            // If continuing a run, only spawn un-received items.
            StacklandsRandomizer.instance.SyncAllReceivedItems(_isNewRun);

            // Reset value
            _isNewRun = false;
        }

        /// <summary>
        /// When a quest is completed, send the location check completion to the archipelago server.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.QuestCompleted))]
        [HarmonyPrefix]
        public static async void OnQuestCompleted_SendLocation(Quest quest)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.QuestCompleted)} Prefix!");
            Debug.Log($"Quest completed: {quest.Id}.");

            await AsyncQueue.Enqueue(() => StacklandsRandomizer.instance.SendCompletedLocation(quest, true));
        }

        /// <summary>
        /// When a new game is started, retrieve .
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.StartNewRound))]
        [HarmonyPostfix]
        public static void OnStartNewRound_SendAllCompletedLocations(ref WorldManager __instance)
        {
            Debug.Log($"{nameof(WorldManager)}.{nameof(WorldManager.StartNewRound)} Prefix!");

            // Set 'is new game' to true
            _isNewRun = true;
        }

        /// <summary>
        /// Before update, override the auto-pause values.
        /// </summary>
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void OnUpdatePre_OverrideAutoPause(ref WorldManager __instance)
        {
            // Check if pausing is disabled
            if (!StacklandsRandomizer.instance.IsPauseEnabled)
            {
                // Store the current set values
                _autoPauseControllerValue = AccessibilityScreen.AutoPauseWhenUsingController;
                _autoPauseKeyboardValue = AccessibilityScreen.AutoPauseWhenUsingKeyboardMouse;

                // Set the values to false to prevent dragging causing a pause
                AccessibilityScreen.AutoPauseWhenUsingController = false;
                AccessibilityScreen.AutoPauseWhenUsingKeyboardMouse = false;
            }
        }

        /// <summary>
        /// After update, set the auto-pause values back to their original state.
        /// </summary>
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void OnUpdatePost_OverrideAutoPause(ref WorldManager __instance)
        {
            if (!StacklandsRandomizer.instance.IsPauseEnabled)
            {
                // Set values back to their original state to prevent accidentally overwriting these values in config
                AccessibilityScreen.AutoPauseWhenUsingController = _autoPauseControllerValue;
                AccessibilityScreen.AutoPauseWhenUsingKeyboardMouse = _autoPauseKeyboardValue;
            }
        }
    }
}
