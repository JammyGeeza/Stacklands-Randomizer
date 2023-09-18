﻿using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using StacklandsRandomizerNS.ItemReceiver;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace StacklandsRandomizerNS
{
    public class StacklandsRandomizer : Mod
    {
        private readonly ItemReceived _itemReceived = new();

        public static readonly object _lock = new();
        public static readonly Queue<Action> _mainThreadActions = new();

        public static List<string> unlockedPacks = new List<string>();
        public static ArchipelagoSession session;
        public void Awake() {
            ConfigEntry<string> port = Config.GetEntry<string>("Server Port", "archipelago.gg:");
            ConfigEntry<string> name = Config.GetEntry<string>("Slot Name", "Player");
            ConfigEntry<string> password = Config.GetEntry<string>("Password", "");

            port.UI.OnUI = (ConfigEntryBase entry) =>
            {
                var btn = Instantiate(PrefabManager.instance.ButtonPrefab, ModOptionsScreen.instance.ButtonsParent);
                btn.transform.localScale = Vector3.one;
                btn.transform.localPosition = Vector3.zero;
                btn.transform.localRotation = Quaternion.identity;

                btn.TextMeshPro.text = "Connect";
                btn.TooltipText = "Connect to the Archipelago server";
                btn.Clicked += () =>
                {
                    session = ArchipelagoSessionFactory.CreateSession(new Uri("ws://" + port.Value));

                    session.Items.ItemReceived += _itemReceived.OnItemReceived;

                    Connect(session, name.Value, password.Value.Length > 0 ? password.Value : null);
                };
            };

            Harmony = new Harmony("com.github.stacklandsrandomizer");
            Harmony.PatchAll();

            ChangeValues();

            InterceptQuestComplete.Logger = Logger;
            RevealAllQuests.Logger = Logger;
        }

        private static void Connect(ArchipelagoSession session, string slotName, string? password = null)
        {
            LoginResult result = null;

            try
            {
                Debug.Log("Trying to connect");

                result = session.TryConnectAndLogin("Stacklands", slotName, ItemsHandlingFlags.AllItems, password: password);

                Debug.Log(result);
            }
            catch (Exception e)
            {
                result = new LoginFailure(e.GetBaseException().Message);
            }

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"";
                foreach (string error in failure.Errors)
                {
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes)
                {
                    errorMessage += $"\n    {error}";
                }

                Debug.Log(errorMessage);

                return;
            }

            var loginSuccess = (LoginSuccessful)result;

            Debug.Log(loginSuccess);

            foreach (var location in session.Locations.AllLocations)
            {
                Debug.Log(location);
            }
        }

        private static void Disconnect() {
            if (session != null) {
                session.Socket.DisconnectAsync();
                session = null;
            }
        }

        public static async Task SendLocation(string locationID) {
            Debug.Log("Sending location " + locationID);
            await session.Locations.CompleteLocationChecksAsync(session.Locations.GetLocationIdFromName("Stacklands", locationID));
        }

        public static void UnlockPack(string packName) {
            unlockedPacks.Add(packName);
        }

        public static void ChangeValues() {
            AllQuests.PauseGame = new Quest("pause_game")
            {
                OnSpecialAction = ((string action) => action == "pause_game"),
                QuestGroup = QuestGroup.Starter
            };
        }

        public static void CreateCard(string cardName) {
            WorldManager.instance.CreateCard(new Vector2(0.0f, 0.0f), cardName, false, false, true);
        }

        public static string PickBasicRandomCard() {
            List<string> cards = new List<string>() {"berrybush","rock","tree"};
            return cards[UnityEngine.Random.Range(0, cards.Count)];
        }

        public void Update() {
            if (InputController.instance.GetKeyDown(Key.Backslash)) {
                Debug.Log("Unlocking packs");
                UnlockPack("Humble Beginnings");
                CreateCard("berrybush");
            }

            lock(_lock) {
                if (WorldManager.instance.CurrentGameState != WorldManager.GameState.InMenu) {
                    while (_mainThreadActions.Count > 0) {
                        _mainThreadActions.Dequeue().Invoke();
                    }
                }
            }
        }
    
        [HarmonyPatch(typeof(WorldManager), "QuestCompleted")]
        public class InterceptQuestComplete {
            public static ModLogger Logger;
            public static async void Prefix(Quest quest) {
                await SendLocation(quest.Description);
                Logger.Log("QuestComplete!");
            }
        }

        [HarmonyPatch(typeof(QuestManager), "JustUnlockedPack")]
        public class StopGameFromUnlockingPacks {
            static void Postfix(ref BoosterpackData __result) {
                __result = null;
            }
        }

        [HarmonyPatch(typeof(QuestManager), "BoosterIsUnlocked")]
        public class LockPacks {
            
            static void Postfix(BoosterpackData p, bool allowDebug, ref bool __result) {
                __result = unlockedPacks.Contains(p.Name);
            }
        }

        [HarmonyPatch(typeof(QuestManager), "QuestIsVisible")]
        public class ShowAllQuests {
            
            static void Postfix(ref bool __result) {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(GameScreen), "UpdateQuestLog")]
        public class RevealAllQuests {
            public static ModLogger Logger;
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                List<CodeInstruction> code = new List<CodeInstruction>(instructions);

                for (int i = 0; i < code.Count; i++) {
                    if (code[i].ToString().Contains("brfalse") && code[i].ToString().Contains("Label5")) {
                        code[i].opcode = OpCodes.Brtrue;
                    }
                }

                return code.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(WorldManager), "GetRandomCard")]
        public class RemoveBlueprints {
            static void Prefix(ref List<CardChance> chances, ref bool removeCard) {
                foreach (CardChance item in chances) {
                    Debug.Log(item.Id);
                    if (item.Id.Contains("blueprint")) {
                        item.Id = PickBasicRandomCard();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CardBag), "GetCardsInBag", new Type[] { typeof(GameDataLoader) })]
        public class RemoveBlueprintsFromBags {
            static void Postfix(ref GameDataLoader loader, ref List<string> __result) {
                List<string> toReplace = new();
                foreach (string card in __result) {
                    if (card.Contains("blueprint_")) {
                        toReplace.Add(card);
                    }
                }
                foreach (string card in toReplace) {
                    __result.Insert(__result.IndexOf(card), PickBasicRandomCard());
                    __result.Remove(card);
                }
            }
        }

        [HarmonyPatch(typeof(CardBag), "GetRawCardChanges")]
        public class RemoveBlueprintsFromSetBags {
            static void Postfix(ref List<CardChance> __result) {
                List<CardChance> toReplace = new();
                foreach (CardChance card in __result) {
                    if (card.Id.Contains("blueprint_")) {
                        toReplace.Add(card);
                    }
                }
                foreach (CardChance card in toReplace) {
                    int index = __result.IndexOf(card);
                    __result.Remove(card);
                    card.Id = PickBasicRandomCard();
                    __result.Insert(index, card);
                }
            }
        }

        // Removes the Combat Intro Boosterpack
        [HarmonyPatch(typeof(Boosterpack), "Clicked")]
        public class RemoveCombatIntroPack{
            static void Prefix() {
                if (!WorldManager.instance.CurrentSave.FoundBoosterIds.Contains("combat_intro")) {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add("combat_intro");
                }
            }
        }

        [HarmonyPatch(typeof(GameCard), "StartBlueprintTimer")]
        public class RemoveTimerForUndiscoveredBlueprints {
            static bool Prefix(ref string blueprintId) {
                if (!WorldManager.instance.CurrentSave.FoundCardIds.Contains(blueprintId)) {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(MainMenu), "Awake")]
        public class DisconnectOnQuit {
            static void Prefix(ref MainMenu __instance) {
                __instance.QuitButton.Clicked += delegate() {
                    Disconnect();
                };
            }
        }
    }
}