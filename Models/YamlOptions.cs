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
        /// Gets or sets the boards that are included in this run.
        /// </summary>
        public Boards Boards { get; private set; }

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
        public Goal Goal { get; private set; }

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
        /// Gets or sets the amount of cards to be sold in a 'Sell Cards Trap' for this run.
        /// </summary>
        public int SellCardTrapAmount { get; private set; }

        /// <summary>
        /// Gets or sets the starting inventory for this run.
        /// </summary>
        public Dictionary<string, int> StartInventory { get; private set; }

        public YamlOptions(Dictionary<string, object> slotData)
        {
            //ActiveBoards = slotData.TryGetValue(TAG_ACTIVE_BOARDS, out object activeBoards)
            //    ? JsonConvert.DeserializeObject<string[]>(activeBoards.ToString()) ?? [ Board.Mainland ]
            //    : [Board.Mainland]; // Default to Mainland if not found

            //Debug.Log($"Active Board(s) for this run: {ActiveBoards}");

            BoardExpansionAmount = slotData.TryGetValue(TAG_BOARD_EXPANSION_AMOUNT, out object expansionAmount)
                ? Convert.ToInt32(expansionAmount)
                : 8; // Default to 8 if not found

            Debug.Log($"Board Expansion Amount for this run: {BoardExpansionAmount}");

            BoardExpansionMode = slotData.TryGetValue(TAG_BOARD_EXPANSION_MODE, out object expansionMode)
                ? (BoardExpansionMode)Convert.ToInt32(expansionMode)
                : BoardExpansionMode.Ideas; // Default to 'Ideas' if not found 
            
            Debug.Log($"Board Expansion Mode for this run: {BoardExpansionMode}");

            Boards = slotData.TryGetValue(TAG_BOARDS, out object boards)
                ? (Boards)Convert.ToInt32(boards)
                : Boards.Mainland; // Default to Mainland if not found

            Debug.Log($"Boards value for this run: {Boards}");

            //DarkForestEnabled = slotData.TryGetValue(TAG_DARKFOREST, out object darkForestEnabled)
            //    ? Convert.ToBoolean(darkForestEnabled)
            //    : false; // Default to false if not found

            //Debug.Log($"Dark Forest Enabled for this run: {DarkForestEnabled}");

            Deathlink = slotData.TryGetValue(TAG_DEATHLINK, out object deathlink)
                ? Convert.ToBoolean(deathlink)
                : false; // Default to false if not found

            Debug.Log($"Deathlink Enabled for this run: {DarkForestEnabled}");

            Goal = slotData.TryGetValue(TAG_GOAL, out object goal)
                ? GoalMapping.Map.Single(g => g.Type == (GoalType)Convert.ToInt32(goal))
                : GoalMapping.Map.Single(g => g.Type == GoalType.KillDemon); // Default to Kill Demon if not found

            Debug.Log($"Goal for this run: {Goal.Name}");

            PauseTimeEnabled = slotData.TryGetValue(TAG_PAUSE_ENABLED, out object pause)
                ? Convert.ToBoolean(pause)
                : true; // Default to true if not found

            Debug.Log($"Pausing Time Enabled for this run: {PauseTimeEnabled}");

            MobsanityEnabled = slotData.TryGetValue(TAG_MOBSANITY, out object mobsanity)
                ? Convert.ToBoolean(mobsanity)
                : false; // Default to false if not found

            Debug.Log($"Mobsanity Enabled for this run: {MobsanityEnabled}");

            // Set moon length for this run
            MoonLength = slotData.TryGetValue(TAG_MOON_LENGTH, out object moonLength)
                ? (MoonLength)Convert.ToInt32(moonLength)
                : MoonLength.Normal; // Default to Normal if not found

            Debug.Log($"Moon Length for this run: {MoonLength}");

            SellCardTrapAmount = slotData.TryGetValue(TAG_SELL_CARD_AMOUNT, out object sellCardAmount)
                ? Convert.ToInt32(sellCardAmount)
                : 3; // Default to 3 if not found

            Debug.Log($"Sell Card Trap Amount for this run: {SellCardTrapAmount}");

            StartInventory = slotData.TryGetValue(TAG_STARTING_INVENTORY, out object inv)
                ? JsonConvert.DeserializeObject<Dictionary<string, int>>(inv.ToString()) ?? new()
                : new(); // Default to empty if not found

            //// If boards does not have 'Forest' flag but goal includes Kill the Wicked Witch, add flag
            //if (!Boards.HasFlag(Boards.Forest) && Goal.Type == GoalType.KillWickedWitch)
            //{
            //    Boards |= Boards.Forest;
            //}
        }
    }
}
