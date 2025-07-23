using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public class YamlOptions
    {
        private static readonly string TAG_BOARD_EXPANSION_MODE = "board_expansion_mode";
        private static readonly string TAG_BOARD_EXPANSION_AMOUNT = "board_expansion_amount";
        private static readonly string TAG_BOARDS = "boards";
        private static readonly string TAG_DARKFOREST = "dark_forest";
        private static readonly string TAG_DEATHLINK = "death_link";
        private static readonly string TAG_GOAL = "goal";
        private static readonly string TAG_MOBSANITY = "mobsanity";
        private static readonly string TAG_MOON_LENGTH = "moon_length";
        private static readonly string TAG_PAUSE_ENABLED = "pausing";
        private static readonly string TAG_SELL_CARD_AMOUNT = "sell_card_trap_amount";
        private static readonly string TAG_STARTING_INVENTORY = "start_inventory";
        private static readonly string TAG_VERSION = "version";

        ///// <summary>
        ///// Gets or sets the active boards for this run.
        ///// </summary>
        //public string[] ActiveBoards { get; private set; } = [];

        /// <summary>
        /// Gets the amount of cards a board expansion will increase the card limit by for this run.
        /// </summary>
        public int BoardExpansionAmount { get; private set; }

        /// <summary>
        /// Gets or sets the board expansion mode for this run.
        /// </summary>
        public BoardExpansionMode BoardExpansionMode { get; private set; }

        /// <summary>
        /// Gets or sets whether The Dark Forest is enabled for this run.
        /// </summary>
        public bool DarkForestEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether deathlink is enabled for this run.
        /// </summary>
        public bool Deathlink { get; private set; }

        /// <summary>
        /// Gets or sets the goal for this run.
        /// </summary>
        public GoalFlags Goal {  get; private set; }

        /// <summary>
        /// Gets or sets the list of goal quests for this run.
        /// </summary>
        public List<Quest> GoalQuests { get; private set; } = new() { };

        /// <summary>
        /// Gets or sets whether pausing time is enabled for this run.
        /// </summary>
        public bool PauseTimeEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether Mobsanity is enabled for this run.
        /// </summary>
        public bool MobsanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the moon length for this run.
        /// </summary>
        public MoonLength MoonLength { get; private set; }

        /// <summary>
        /// Gets or sets the quests that are included in this run.
        /// </summary>
        public QuestCheckFlags QuestChecks { get; private set; }

        /// <summary>
        /// Gets or sets the amount of cards to be sold in a 'Sell Cards Trap' for this run.
        /// </summary>
        public int SellCardTrapAmount { get; private set; }

        /// <summary>
        /// Gets or sets the starting inventory for this run.
        /// </summary>
        public Dictionary<string, int> StartInventory { get; private set; }

        /// <summary>
        /// Gets or sets the version of the .apworld file.
        /// </summary>
        public string Version { get; private set; }

        public YamlOptions(Dictionary<string, object> slotData)
        {
            BoardExpansionAmount = slotData.TryGetValue(TAG_BOARD_EXPANSION_AMOUNT, out object expansionAmount)
                ? Convert.ToInt32(expansionAmount)
                : 8; // Default to 8 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Board Expansion Amount for this run: {BoardExpansionAmount}");

            BoardExpansionMode = slotData.TryGetValue(TAG_BOARD_EXPANSION_MODE, out object expansionMode)
                ? (BoardExpansionMode)Convert.ToInt32(expansionMode)
                : BoardExpansionMode.Ideas; // Default to 'Ideas' if not found 
            
            StacklandsRandomizer.instance.ModLogger.Log($"Board Expansion Mode for this run: {BoardExpansionMode}");

            Deathlink = slotData.TryGetValue(TAG_DEATHLINK, out object deathlink)
                ? Convert.ToBoolean(deathlink)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Deathlink Enabled for this run: {DarkForestEnabled}");

            Goal = slotData.TryGetValue(TAG_GOAL, out object goal)
                ? (GoalFlags)Convert.ToInt32(goal)
                : GoalFlags.None; // Default to None if not found.

            StacklandsRandomizer.instance.ModLogger.Log($"Goal value for this run: {Goal}");

            // Add goal quests to list
            if (Goal.HasFlag(GoalFlags.Kill_the_Demon))
            {
                GoalQuests.Add(AllQuests.KillDemon);
            }

            if (Goal.HasFlag(GoalFlags.Kill_the_Wicked_Witch))
            {
                GoalQuests.Add(AllQuests.FightWickedWitch);
            }

            StacklandsRandomizer.instance.ModLogger.Log($"Goal Quests for this run: {string.Join(", ", GoalQuests.Select(gq => gq.Id))}");

            PauseTimeEnabled = slotData.TryGetValue(TAG_PAUSE_ENABLED, out object pause)
                ? Convert.ToBoolean(pause)
                : true; // Default to true if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Pausing Time Enabled for this run: {PauseTimeEnabled}");

            MobsanityEnabled = slotData.TryGetValue(TAG_MOBSANITY, out object mobsanity)
                ? Convert.ToBoolean(mobsanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Mobsanity Enabled for this run: {MobsanityEnabled}");

            // Set moon length for this run
            MoonLength = slotData.TryGetValue(TAG_MOON_LENGTH, out object moonLength)
                ? (MoonLength)Convert.ToInt32(moonLength)
                : MoonLength.Normal; // Default to Normal if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Moon Length for this run: {MoonLength}");

            QuestChecks = slotData.TryGetValue(TAG_BOARDS, out object boards)
                ? (QuestCheckFlags)Convert.ToInt32(boards)
                : QuestCheckFlags.Mainland; // Default to Mainland if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Quest Checks value for this run: {QuestChecks}");

            SellCardTrapAmount = slotData.TryGetValue(TAG_SELL_CARD_AMOUNT, out object sellCardAmount)
                ? Convert.ToInt32(sellCardAmount)
                : 3; // Default to 3 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Sell Card Trap Amount for this run: {SellCardTrapAmount}");

            StartInventory = slotData.TryGetValue(TAG_STARTING_INVENTORY, out object inv)
                ? JsonConvert.DeserializeObject<Dictionary<string, int>>(inv.ToString()) ?? new()
                : new(); // Default to empty if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Starting Inventory for this run: {string.Join(", ", StartInventory.Select(si => si.Key))}");

            Version = slotData.TryGetValue(TAG_VERSION, out object version)
                ? Convert.ToString(version)
                : string.Empty;

            StacklandsRandomizer.instance.ModLogger.Log($".apworld version: {Version}");
        }
    }
}
