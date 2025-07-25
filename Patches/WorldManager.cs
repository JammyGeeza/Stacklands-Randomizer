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
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.Awake Postfix!");
            StacklandsRandomizer.instance.ModLogger.Log($"Determined save ID: {SaveManager.instance.CurrentSave.SaveId}...");

            // Set current save to Archipelago save
            SaveManager.instance.CurrentSave = CommonPatchMethods.FindOrCreateArchipelagoSave(SaveManager.instance);
        }

        /// <summary>
        /// When a save is cleared, re-sync all received items from server.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.ClearSaveAndRestart))]
        [HarmonyPostfix]
        public static void OnClearSaveAndRestart_SyncItemsWithServer(WorldManager __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.ClearSaveAndRestart)} Postfix!");

            // Re-sync all items with server
            StacklandsRandomizer.instance.SyncAllReceivedItems();
        }

        /// <summary>
        /// When getting a board with the <see cref="Location"/> enum, fake an extra board to prevent default booster pack behaviour.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.GetBoardWithLocation))]
        [HarmonyPostfix]
        public static void OnGetBoardWithLocation_PreventStandardPackBehaviour(ref Location loc, ref GameBoard __result)
        {
            // If the board being searched for is the 'Archipelago' fictional board
            if (loc == EnumExtensionHandler.ArchipelagoLocationEnum)
            {
                // Prevent it from behaving the same way as other booster packs
                __result = new GameBoard()
                {
                    Id = "archipelago",
                    BoardOptions = new BoardOptions()
                    {
                        NewVillagerSpawnsFromPack = false, // Prevent it spawning villagers
                    }
                };
            }
        }

        /// <summary>
        /// When a card is created, remove blueprints from equipable items to prevent mobs from dropping them too early.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.CreateCard), [typeof(Vector3), typeof(CardData), typeof(bool), typeof(bool), typeof(bool), typeof(bool)])]
        [HarmonyPrefix]
        public static void OnCreateCard_Intercept(WorldManager __instance, ref CardData cardDataPrefab)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.CreateCard)} Prefix!");
            StacklandsRandomizer.instance.ModLogger.Log($"Creating Card with ID: {cardDataPrefab.Id}");

            if (cardDataPrefab is Equipable equipable && equipable.blueprint != null)
            {
                equipable.blueprint = null;
            }
        }

        /// <summary>
        /// When receiving a random card, replace blueprints with a basic card.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.GetRandomCard))]
        [HarmonyPrefix]
        public static void OnGetRandomCard_ReplaceBlueprint(ref List<CardChance> chances, ref bool removeCard)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.GetRandomCard)} Prefix!");

            foreach (CardChance chance in chances)
            {
                // Check if card should be blocked and if so, replace with a random basic card
                if (CommonPatchMethods.ShouldCardBeBlocked(chance.Id))
                {
                    chance.Id = CommonPatchMethods.GetRandomBasicCard();
                }
            }
        }

        [HarmonyPatch(nameof(WorldManager.CardCapIncrease))]
        [HarmonyPostfix]
        public static void OnCardCapIncrease_CheckForBoardExpansion(WorldManager __instance, ref GameBoard board, ref int __result)
        {
            if (StacklandsRandomizer.instance.Options.BoardExpansionMode is BoardExpansionMode.Items)
            {
                foreach (GameCard card in __instance.AllCards)
                {
                    if (card.MyBoard == board)
                    {
                        if (card.CardData.Id == ModCards.board_expansion)
                        {
                            // Add expansion amount
                            __result += StacklandsRandomizer.instance.Options.BoardExpansionAmount;
                        }
                        else if (card.CardData.Id == Cards.shed)
                        {
                            // Remove bonus for shed
                            __result -= 4;
                        }
                        else if (card.CardData.Id == Cards.warehouse)
                        {
                            // Remove bonus for warehouse
                            __result -= 14;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// When checking if a card has been found, intercept specific card checks
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.HasFoundCard))]
        [HarmonyPostfix]
        public static void OnHasFoundCard_Intercept(WorldManager __instance, ref string cardId, ref bool __result)
        {
            // If dark forest enabled in checks, pretend that Idea: Stable Portal has been found to prevent it automatically spawning when returning from The Dark Forest
            if (cardId == Cards.blueprint_stable_portal && StacklandsRandomizer.instance.Options.DarkForestEnabled)
            {
                __result = true;
            }

            // Other intercepts here, if needed
        }

        /// <summary>
        /// When the pause menu is showing, override the 'IsPlaying' property (if required) to ensure time continues.
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.IsPlaying), MethodType.Getter)]
        [HarmonyPostfix]
        public static void OnIsPlaying(WorldManager __instance, ref bool __result)
        {
            // Check if pausing is disabled...
            if (!StacklandsRandomizer.instance.Options.PauseTimeEnabled)
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
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.KillVillagerCoroutine)} Postfix!");

            // Check if object is definitely a villager
            if (combatable is BaseVillager baseVillager)
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Villager '{baseVillager.Name}' has died!");

                // If this death was not triggered by a deathlink, send deathlink
                if (!StacklandsRandomizer.instance.HandlingDeathLink)
                {
                    StacklandsRandomizer.instance.SendDeathlink(combatable.Name, $"A {combatable.Name} has ceased to be.");
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
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.LoadSaveRound)} Prefix!");

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
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.Play)} Postfix!");

            // Add 'combat intro' to found booster packs to prevent it from spawning
            if (!__instance.CurrentSave.FoundBoosterIds.Contains("combat_intro"))
            {
                __instance.CurrentSave.FoundBoosterIds.Add("combat_intro");
            }

            // Send all currently completed locations
            await StacklandsRandomizer.instance.SendAllCompletedLocations();

            // Re-sync all received items
            StacklandsRandomizer.instance.SyncAllReceivedItems();

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
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.QuestCompleted)} Prefix!");
            StacklandsRandomizer.instance.ModLogger.Log($"Quest completed: {quest.Id}.");

            // Queue sending completed quest as location check
            await AsyncQueue.Enqueue(() => StacklandsRandomizer.instance.SendCompletedLocation(quest, true));
        }

        /// <summary>
        /// When a new game is started, retrieve .
        /// </summary>
        [HarmonyPatch(nameof(WorldManager.StartNewRound))]
        [HarmonyPostfix]
        public static void OnStartNewRound_SendAllCompletedLocations(ref WorldManager __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(WorldManager)}.{nameof(WorldManager.StartNewRound)} Prefix!");

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
            if (!StacklandsRandomizer.instance.Options.PauseTimeEnabled)
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
            if (!StacklandsRandomizer.instance.Options.PauseTimeEnabled)
            {
                // Set values back to their original state to prevent accidentally overwriting these values in config
                AccessibilityScreen.AutoPauseWhenUsingController = _autoPauseControllerValue;
                AccessibilityScreen.AutoPauseWhenUsingKeyboardMouse = _autoPauseKeyboardValue;
            }
        }
    }
}
