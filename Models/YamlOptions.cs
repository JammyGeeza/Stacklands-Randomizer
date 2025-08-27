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
        private static readonly string TAG_EQUIPMENTSANITY = "equipmentsanity";
        private static readonly string TAG_FOODSANITY = "foodsanity";
        private static readonly string TAG_GOAL = "goal";
        private static readonly string TAG_GOAL_BOARDS = "goal_boards";
        private static readonly string TAG_LOCATIONSANITY = "locationsanity";
        private static readonly string TAG_MOBSANITY = "mobsanity";
        private static readonly string TAG_MOBSANITY_BALANCING = "mobsanity_balancing";
        private static readonly string TAG_MOON_LENGTH = "moon_length";
        //private static readonly string TAG_PACKSANITY = "packsanity";
        private static readonly string TAG_PAUSE_ENABLED = "pausing";
        private static readonly string TAG_SELL_CARD_AMOUNT = "sell_card_trap_amount";
        private static readonly string TAG_SPENDSANITY = "spendsanity";
        private static readonly string TAG_SPENDSANITY_COST = "spendsanity_cost";
        private static readonly string TAG_SPENDSANITY_COUNT = "spendsanity_count";
        private static readonly string TAG_STARTING_INVENTORY = "start_inventory";
        private static readonly string TAG_STRUCTURESANITY = "structuresanity";
        private static readonly string TAG_VERSION = "version";

        ///// <summary>
        ///// Gets or sets the active boards for this run.
        ///// </summary>
        //public string[] ActiveBoards { get; private set; } = [];

        /// <summary>
        /// Gets the active boards for this run.
        /// </summary>
        public BoardFlags Boards { get; private set; }

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
        /// Gets or sets whether Equipmentsanity is enabled for this run.
        /// </summary>
        public bool EquipmentsanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether Foodsanity is enabled for this run.
        /// </summary>
        public bool FoodsanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the goal for this run.
        /// </summary>
        public GoalFlags Goal {  get; private set; }

        /// <summary>
        /// Gets or sets the boards the goal applies to for this run.
        /// </summary>
        public BoardFlags GoalBoards { get; private set; }

        /// <summary>
        /// Gets or sets the list of goal quests for this run.
        /// </summary>
        public List<Quest> GoalQuests { get; private set; } = new() { };

        /// <summary>
        /// Gets or sets whether Locationsanity is enabled for this run.
        /// </summary>
        public bool LocationsanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether pausing time is enabled for this run.
        /// </summary>
        public bool PauseTimeEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether Mobsanity is enabled for this run.
        /// </summary>
        public bool MobsanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets whether Mobsanity balancing is enabled for this run.
        /// </summary>
        public bool MobsanityBalancingEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the moon length for this run.
        /// </summary>
        public MoonLength MoonLength { get; private set; }

        ///// <summary>
        ///// Gets or sets the quests that are included in this run.
        ///// </summary>
        //public QuestCheckFlags QuestChecks { get; private set; }

        /// <summary>
        /// Gets or sets the amount of cards to be sold in a 'Sell Cards Trap' for this run.
        /// </summary>
        public int SellCardTrapAmount { get; private set; }

        /// <summary>
        /// Gets or sets the Spendsanity mode.
        /// </summary>
        public Spendsanity Spendsanity { get; private set; }

        /// <summary>
        /// Gets or sets the spendsanity cost.
        /// </summary>
        public int SpendsanityCost { get; private set; }

        /// <summary>
        /// Gets or sets the amount of spendsanity checks.
        /// </summary>
        public int SpendsanityCount { get; private set; }

        /// <summary>
        /// Gets or sets the starting inventory for this run.
        /// </summary>
        public Dictionary<string, int> StartInventory { get; private set; }

        /// <summary>
        /// Gets or sets whether Structuresanity is enabled for this run.
        /// </summary>
        public bool StructuresanityEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the version of the .apworld file.
        /// </summary>
        public string Version { get; private set; }

        public YamlOptions(Dictionary<string, object> slotData)
        {
            Boards = slotData.TryGetValue(TAG_BOARDS, out object boards)
                ? (BoardFlags)Convert.ToInt32(boards)
                : BoardFlags.Mainland;

            StacklandsRandomizer.instance.ModLogger.Log($"Boards for this run: {string.Join(", ", Enum.GetValues(typeof(BoardFlags))
                .Cast<BoardFlags>()
                .Where(flag => flag != BoardFlags.None && Boards.HasFlag(flag)))}");

            BoardExpansionAmount = slotData.TryGetValue(TAG_BOARD_EXPANSION_AMOUNT, out object expansionAmount)
                ? Convert.ToInt32(expansionAmount)
                : 8; // Default to 8 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Board Expansion Amount for this run: {BoardExpansionAmount}");

            BoardExpansionMode = slotData.TryGetValue(TAG_BOARD_EXPANSION_MODE, out object expansionMode)
                ? (BoardExpansionMode)Convert.ToInt32(expansionMode)
                : BoardExpansionMode.Ideas; // Default to 'Ideas' if not found 
            
            StacklandsRandomizer.instance.ModLogger.Log($"Board Expansion Mode for this run: {BoardExpansionMode}");

            Goal = slotData.TryGetValue(TAG_GOAL, out object goal)
                ? (GoalFlags)Convert.ToInt32(goal)
                : GoalFlags.None; // Default to None if not found.

            StacklandsRandomizer.instance.ModLogger.Log($"Goal(s) for this run: {string.Join(", ", Enum.GetValues(typeof(GoalFlags))
                .Cast<GoalFlags>()
                .Where(flag => flag != GoalFlags.None && Goal.HasFlag(flag)))}");

            GoalBoards = slotData.TryGetValue(TAG_GOAL_BOARDS, out object goalBoards)
                ? (BoardFlags)Convert.ToInt32(goalBoards)
                : BoardFlags.None;

            StacklandsRandomizer.instance.ModLogger.Log($"Goal Board(s) for this run: {string.Join(", ", Enum.GetValues(typeof(BoardFlags))
                .Cast<BoardFlags>()
                .Where(flag => flag != BoardFlags.None && GoalBoards.HasFlag(flag)))}");

            // Set goal quests depending on selected goal
            if (Goal.HasFlag(GoalFlags.AllBosses) || Goal.HasFlag(GoalFlags.RandomBoss))
            {
                if (GoalBoards.HasFlag(BoardFlags.Mainland))
                {
                    GoalQuests.Add(AllQuests.KillDemon);
                }

                if (GoalBoards.HasFlag(BoardFlags.Forest))
                {
                    GoalQuests.Add(AllQuests.FightWickedWitch);
                }

                if (GoalBoards.HasFlag(BoardFlags.Island))
                {
                    GoalQuests.Add(AllQuests.KillDemonLord);
                }
            }

            StacklandsRandomizer.instance.ModLogger.Log($"Goal Quest(s) for this run: {string.Join(", ", GoalQuests.Select(gq => gq.Id))}");

            PauseTimeEnabled = slotData.TryGetValue(TAG_PAUSE_ENABLED, out object pause)
                ? Convert.ToBoolean(pause)
                : true; // Default to true if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Pausing Time Enabled for this run: {PauseTimeEnabled}");

            EquipmentsanityEnabled = slotData.TryGetValue(TAG_EQUIPMENTSANITY, out object equipmentsanity)
                ? Convert.ToBoolean(equipmentsanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Equipmentsanity Enabled for this run: {EquipmentsanityEnabled}");

            FoodsanityEnabled = slotData.TryGetValue(TAG_FOODSANITY, out object foodsanity)
                ? Convert.ToBoolean(foodsanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Foodsanity Enabled for this run: {FoodsanityEnabled}");

            LocationsanityEnabled = slotData.TryGetValue(TAG_LOCATIONSANITY, out object locationsanity)
                ? Convert.ToBoolean(locationsanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Locationsanity Enabled for this run: {LocationsanityEnabled}");

            MobsanityEnabled = slotData.TryGetValue(TAG_MOBSANITY, out object mobsanity)
                ? Convert.ToBoolean(mobsanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Mobsanity Enabled for this run: {MobsanityEnabled}");

            MobsanityBalancingEnabled = slotData.TryGetValue(TAG_MOBSANITY_BALANCING, out object mobsanityBalancing)
                ? Convert.ToBoolean(mobsanityBalancing)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Mobsanity Balancing Enabled for this run: {MobsanityBalancingEnabled}");

            // Set moon length for this run
            MoonLength = slotData.TryGetValue(TAG_MOON_LENGTH, out object moonLength)
                ? (MoonLength)Convert.ToInt32(moonLength)
                : MoonLength.Normal; // Default to Normal if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Moon Length for this run: {MoonLength}");

            //QuestChecks = slotData.TryGetValue(TAG_BOARDS, out object boards)
            //    ? (QuestCheckFlags)Convert.ToInt32(boards)
            //    : QuestCheckFlags.Mainland; // Default to Mainland if not found

            //StacklandsRandomizer.instance.ModLogger.Log($"Quest Checks value for this run: {QuestChecks}");

            SellCardTrapAmount = slotData.TryGetValue(TAG_SELL_CARD_AMOUNT, out object sellCardAmount)
                ? Convert.ToInt32(sellCardAmount)
                : 3; // Default to 3 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Sell Card Trap Amount for this run: {SellCardTrapAmount}");

            Spendsanity = slotData.TryGetValue(TAG_SPENDSANITY, out object spendsanity)
                ? (Spendsanity)Convert.ToInt32(spendsanity)
                : Spendsanity.Off; // Default to off if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Spendsanity Mode for this run: {Spendsanity}");

            SpendsanityCost = slotData.TryGetValue(TAG_SPENDSANITY_COST, out object spendsanityCost)
                ? Convert.ToInt32(spendsanityCost)
                : 3; // Default to 3 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Spendsanity Cost for this run: {SpendsanityCost}");

            SpendsanityCount = slotData.TryGetValue(TAG_SPENDSANITY_COUNT, out object spendsanityCount)
                ? Convert.ToInt32(spendsanityCount)
                : 0; // Default to 0 if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Spendsanity Count for this run: {SpendsanityCount}");

            StartInventory = slotData.TryGetValue(TAG_STARTING_INVENTORY, out object inv)
                ? JsonConvert.DeserializeObject<Dictionary<string, int>>(inv.ToString()) ?? new()
                : new(); // Default to empty if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Starting Inventory for this run: {string.Join(", ", StartInventory.Select(si => si.Key))}");

            StructuresanityEnabled = slotData.TryGetValue(TAG_STRUCTURESANITY, out object structuresanity)
                ? Convert.ToBoolean(structuresanity)
                : false; // Default to false if not found

            StacklandsRandomizer.instance.ModLogger.Log($"Structuresanity Enabled for this run: {StructuresanityEnabled}");

            Version = slotData.TryGetValue(TAG_VERSION, out object version)
                ? Convert.ToString(version)
                : string.Empty;

            StacklandsRandomizer.instance.ModLogger.Log($"Version: {Version}");
        }
    }
}
