using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class CustomQuestMapping
    {
        /// <summary>
        /// List of custom quests for Mainland.
        /// </summary>
        public static readonly List<Quest> Mainland = new()
        {
            #region Additional Archipelago Quests

            // Booster Packs
            new("buy_idea_booster")                   { OnSpecialAction = (string action) => action == $"buy_idea_pack"                                                                     , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_farming_booster")                { OnSpecialAction = (string action) => action == $"buy_farming_pack"                                                                  , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_cooking_booster")                { OnSpecialAction = (string action) => action == $"buy_cooking_pack"                                                                  , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_idea2_booster")                  { OnSpecialAction = (string action) => action == $"buy_idea2_pack"                                                                    , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_equipment_booster")              { OnSpecialAction = (string action) => action == $"buy_equipment_pack"                                                                , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_locations_booster")              { OnSpecialAction = (string action) => action == $"buy_locations_pack"                                                                , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },
            new("buy_structures_booster")             { OnSpecialAction = (string action) => action == $"buy_structures_pack"                                                               , PossibleInPeacefulMode = true      , QuestGroup = QuestGroup.Starter },

            new("buy_5_mainland_packs")               { OnSpecialAction = (string action) => CommonPatchMethods.MAINLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesMainlandBoosterPacksBought() >= 5     , PossibleInPeacefulMode = true    , QuestGroup = QuestGroup.Starter   , RequiredCount = 5     , DescriptionTermOverride = "quest_buy_mainland_packs_text" },
            new("buy_10_mainland_packs")              { OnSpecialAction = (string action) => CommonPatchMethods.MAINLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesMainlandBoosterPacksBought() >= 10    , PossibleInPeacefulMode = true    , QuestGroup = QuestGroup.Starter   , RequiredCount = 10    , DescriptionTermOverride = "quest_buy_mainland_packs_text" },
            new("buy_25_mainland_packs")              { OnSpecialAction = (string action) => CommonPatchMethods.MAINLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesMainlandBoosterPacksBought() >= 25    , PossibleInPeacefulMode = true    , QuestGroup = QuestGroup.Starter   , RequiredCount = 25    , DescriptionTermOverride = "quest_buy_mainland_packs_text" },

            // Selling cards
            new("sell_5_cards")                       { OnSpecialAction = (string action) => action == "sell_card" && CommonPatchMethods.GetTimesCardsSold() >= 5                           , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Starter       , RequiredCount = 5     , DescriptionTermOverride = "quest_sell_cards_text" },
            new("sell_10_cards")                      { OnSpecialAction = (string action) => action == "sell_card" && CommonPatchMethods.GetTimesCardsSold() >= 10                          , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Starter       , RequiredCount = 10    , DescriptionTermOverride = "quest_sell_cards_text" },
            new("sell_25_cards")                      { OnSpecialAction = (string action) => action == "sell_card" && CommonPatchMethods.GetTimesCardsSold() >= 25                          , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Starter       , RequiredCount = 25    , DescriptionTermOverride = "quest_sell_cards_text" },

            // Reaching Moons
            new("reach_month_18")                     { OnSpecialAction = (string action) => (action == "month_end" && WorldManager.instance.CurrentMonth >= 18)                            , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Survival      , RequiredCount = 18    , DescriptionTermOverride = "quest_reach_month_text" },
            new("reach_month_30")                     { OnSpecialAction = (string action) => (action == "month_end" && WorldManager.instance.CurrentMonth >= 30)                            , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Survival      , RequiredCount = 36    , DescriptionTermOverride = "quest_reach_month_text" },

            // Villagers
            new("get_5_villagers")                    { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCount<BaseVillager>() == 5                                           , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.MainQuest     , RequiredCount = 5     , DescriptionTermOverride = "quest_get_villagers_text" },
            new("get_7_villagers")                    { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCount<BaseVillager>() == 7                                           , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.MainQuest     , RequiredCount = 7     , DescriptionTermOverride = "quest_get_villagers_text" },

            // Resources
            new("have_10_bricks")                     { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.brick) == 10                                    , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },
            new("have_10_flint")                      { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.flint) == 10                                    , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },
            new("have_10_iron_bars")                  { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.iron_bar) == 10                                 , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },
            new("have_10_iron_ore")                   { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.iron_ore) == 10                                 , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },
            new("have_10_planks")                     { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.plank) == 10                                    , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },
            new("have_10_sticks")                     { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.stick) == 10                                    , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Resources     , RequiredCount = 10 },

            #endregion

            #region Equipmentsanity

            new($"make_{Cards.bone_spear}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_bone_spear && action == "finish_blueprint"        , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.boomerang}")              { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_boomerang && action == "finish_blueprint"         , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.club}")                   { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_club && action == "finish_blueprint"              , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.chainmail_armor}")        { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_chainmail_armor && action == "finish_blueprint"   , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.iron_shield}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_iron_shield && action == "finish_blueprint"       , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.magic_blade}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_magic_blade && action == "finish_blueprint"       , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.magic_ring}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_magic_ring && action == "finish_blueprint"        , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.magic_staff}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_magic_staff && action == "finish_blueprint"       , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.magic_tome}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_magic_tome && action == "finish_blueprint"        , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.magic_wand}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_magic_wand && action == "finish_blueprint"        , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.slingshot}")              { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_slingshot && action == "finish_blueprint"         , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.spear}")                  { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_woodenweapons && action == "finish_blueprint"     , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.spiked_plank}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_spiked_plank && action == "finish_blueprint"      , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.sword}")                  { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_ironweapons && action == "finish_blueprint"       , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.throwing_star}")          { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_throwing_star && action == "finish_blueprint"     , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.wooden_shield}")          { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_wooden_shield && action == "finish_blueprint"     , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },

            #endregion

            #region Foodsanity

            new($"make_{Cards.fruit_salad}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_fruitsalad && action == "finish_blueprint"        , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },
            new($"make_{Cards.milkshake}")              { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_milkshake && action == "finish_blueprint"         , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },
            new($"make_{Cards.stew}")                   { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_stew && action == "finish_blueprint"              , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },

            #endregion

            #region Locationsanity

            new($"explore_{Cards.catacombs}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.catacombs && action == "complete_harvest"                   , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },
            new($"explore_{Cards.graveyard}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.graveyard && action == "complete_harvest"                   , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },
            new($"explore_{Cards.old_village}")         { OnActionComplete = (CardData card, string action) => card.Id == Cards.old_village && action == "complete_harvest"                 , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },
            new($"explore_{Cards.plains}")              { OnActionComplete = (CardData card, string action) => card.Id == Cards.plains && action == "complete_harvest"                      , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },

            #endregion

            #region Mobsanity

            new($"kill_{Cards.bear}")                   { OnSpecialAction = (string action) => action == $"{Cards.bear}_killed"                                                             , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.elf}")                    { OnSpecialAction = (string action) => action == $"{Cards.elf}_killed"                                                              , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.elf_archer}")             { OnSpecialAction = (string action) => action == $"{Cards.elf_archer}_killed"                                                       , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.enchanted_shroom}")       { OnSpecialAction = (string action) => action == $"{Cards.enchanted_shroom}_killed"                                                 , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.feral_cat}")              { OnSpecialAction = (string action) => action == $"{Cards.feral_cat}_killed"                                                        , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.frog_man}")               { OnSpecialAction = (string action) => action == $"{Cards.frog_man}_killed"                                                         , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.ghost}")                  { OnSpecialAction = (string action) => action == $"{Cards.ghost}_killed"                                                            , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.giant_rat}")              { OnSpecialAction = (string action) => action == $"{Cards.giant_rat}_killed"                                                        , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.giant_snail}")            { OnSpecialAction = (string action) => action == $"{Cards.giant_snail}_killed"                                                      , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.goblin}")                 { OnSpecialAction = (string action) => action == $"{Cards.goblin}_killed"                                                           , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.goblin_archer}")          { OnSpecialAction = (string action) => action == $"{Cards.goblin_archer}_killed"                                                    , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.goblin_shaman}")          { OnSpecialAction = (string action) => action == $"{Cards.goblin_shaman}_killed"                                                    , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.merman}")                 { OnSpecialAction = (string action) => action == $"{Cards.merman}_killed"                                                           , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.mimic}")                  { OnSpecialAction = (string action) => action == $"{Cards.mimic}_killed"                                                            , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.mosquito}")               { OnSpecialAction = (string action) => action == $"{Cards.mosquito}_killed"                                                         , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.slime}")                  { OnSpecialAction = (string action) => action == $"{Cards.slime}_killed"                                                            , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.small_slime}")            { OnSpecialAction = (string action) => action == $"{Cards.small_slime}_killed"                                                      , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.wolf}")                   { OnSpecialAction = (string action) => action == $"{Cards.wolf}_killed"                                                             , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },

            #endregion

            #region Structuresanity

            new($"build_{Cards.coin_chest}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_coinchest && action == "finish_blueprint"         , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.garden}")                { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_garden && action == "finish_blueprint"            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.hotpot}")                { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_hotpot && action == "finish_blueprint"            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.mine}")                  { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_mine && action == "finish_blueprint"              , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.market}")                { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_market && action == "finish_blueprint"            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.resource_chest}")        { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_resourcechest && action == "finish_blueprint"     , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.sawmill}")               { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_sawmill && action == "finish_blueprint"           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.smelter}")               { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_smelting && action == "finish_blueprint"          , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.stove}")                 { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_stove && action == "finish_blueprint"             , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.warehouse}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_warehouse && action == "finish_blueprint"         , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },

            #endregion
        };

        /// <summary>
        /// List of custom quests for Dark Forest.
        /// </summary>
        public static readonly List<Quest> DarkForest = new()
        {
            #region Additional Archipelago Quests

            new("fight_wave_two")                       { OnSpecialAction = (string action) => (action == "completed_forest_wave" && WorldManager.instance.CurrentRunVariables.ForestWave == 2)     , PossibleInPeacefulMode = false    , QuestGroup = QuestGroup.Forest_MainQuest  , RequiredCount = 2  , DescriptionTermOverride = "quest_fight_wave_text" },
            new("fight_wave_four")                      { OnSpecialAction = (string action) => (action == "completed_forest_wave" && WorldManager.instance.CurrentRunVariables.ForestWave == 4)     , PossibleInPeacefulMode = false    , QuestGroup = QuestGroup.Forest_MainQuest  , RequiredCount = 4  , DescriptionTermOverride = "quest_fight_wave_text" },
            new("fight_wave_eight")                     { OnSpecialAction = (string action) => (action == "completed_forest_wave" && WorldManager.instance.CurrentRunVariables.ForestWave == 8)     , PossibleInPeacefulMode = false    , QuestGroup = QuestGroup.Forest_MainQuest  , RequiredCount = 8  , DescriptionTermOverride = "quest_fight_wave_text" },

            #endregion

            #region Mobsanity

            new($"kill_{Cards.dark_elf}")               { OnSpecialAction = (string action) => action == $"{Cards.dark_elf}_killed"                                                                 , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.ent}")                    { OnSpecialAction = (string action) => action == $"{Cards.ent}_killed"                                                                      , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.ogre}")                   { OnSpecialAction = (string action) => action == $"{Cards.ogre}_killed"                                                                     , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },

            #endregion
        };

        /// <summary>
        /// List of custom quests for The Island.
        /// </summary>
        public static readonly List<Quest> Island = new()
        {
            #region Additional Archipelago Quests

            // Booster Packs
            new("buy_island2_booster")                  { OnSpecialAction = (string action) => action == $"buy_island2_pack"                                                                                                                    , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },
            new("buy_island_ideas_1_booster")           { OnSpecialAction = (string action) => action == $"buy_island_ideas_1_pack"                                                                                                             , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },
            new("buy_island_cooking_booster")           { OnSpecialAction = (string action) => action == $"buy_island_cooking_pack"                                                                                                             , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },
            new("buy_island_ideas_2_booster")           { OnSpecialAction = (string action) => action == $"buy_island_ideas_2_pack"                                                                                                             , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },
            new("buy_island_advanced_booster")          { OnSpecialAction = (string action) => action == $"buy_island_advanced_pack"                                                                                                            , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },
            new("buy_island_locations_booster")         { OnSpecialAction = (string action) => action == $"buy_island_locations_pack"                                                                                                           , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings },

            new("buy_5_island_packs")                   { OnSpecialAction = (string action) => CommonPatchMethods.ISLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesIslandBoosterPacksBought() >= 5      , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings     , RequiredCount = 5     , DescriptionTermOverride = "quest_buy_island_packs_text" },
            new("buy_10_island_packs")                  { OnSpecialAction = (string action) => CommonPatchMethods.ISLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesIslandBoosterPacksBought() >= 10     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings     , RequiredCount = 10    , DescriptionTermOverride = "quest_buy_island_packs_text" },
            new("buy_25_island_packs")                  { OnSpecialAction = (string action) => CommonPatchMethods.ISLAND_PACKS.Select(n => $"buy_{n}_pack").Contains(action) && CommonPatchMethods.GetTimesIslandBoosterPacksBought() >= 25     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings     , RequiredCount = 25    , DescriptionTermOverride = "quest_buy_island_packs_text" },

            // Selling cards
            new("have_30_shells")                       { OnCardCreate = (CardData card) => WorldManager.instance.GetShellCount(includeInChest: true) >= 30                                                                                     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings     , RequiredCount = 30    , DescriptionTermOverride = "quest_have_shells_text" },
            new("have_50_shells")                       { OnCardCreate = (CardData card) => WorldManager.instance.GetShellCount(includeInChest: true) >= 50                                                                                     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Beginnings     , RequiredCount = 50    , DescriptionTermOverride = "quest_have_shells_text" },

            // Resources
            new("have_10_cotton")                       { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.cotton) == 10                                                                                     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_fabric")                       { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.fabric) == 10                                                                                     , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_glass")                        { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.glass) == 10                                                                                      , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_gold_bars")                    { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.gold_bar) == 10                                                                                   , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_gold_ore")                     { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.gold_ore) == 10                                                                                   , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_rope")                         { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.rope) == 10                                                                                       , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_sails")                        { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.sail) == 10                                                                                       , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },
            new("have_10_sandstone")                    { OnCardCreate = (CardData card) => WorldManager.instance.GetCardCountWithChest(Cards.sandstone) == 10                                                                                  , PossibleInPeacefulMode = true     , QuestGroup = QuestGroup.Island_Misc           , RequiredCount = 10 },

            #endregion

            #region Equipmentsanity

            new($"make_{Cards.amulet_of_mountain}")     { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_amulet_of_mountain && action == "finish_blueprint"                                                    , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.blunderbuss}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_blunderbuss && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.bone_staff}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_bone_staff && action == "finish_blueprint"                                                            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.crossbow}")               { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_crossbow && action == "finish_blueprint"                                                              , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.gold_chestplate}")        { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_gold_chestplate && action == "finish_blueprint"                                                       , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },
            new($"make_{Cards.wizard_robe}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_wizard_robe && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.EquipmentsanityQuestGroupEnum },

            #endregion

            #region Foodsanity

            new($"make_{Cards.bottle_of_water}")        { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_fill_bottle && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },
            new($"make_{Cards.cooked_fish}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_cooked_fish && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },
            new($"make_{Cards.tamago_sushi}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_tamago_sushi && action == "finish_blueprint"                                                          , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.FoodsanityQuestGroupEnum },

            #endregion

            #region Locationsanity

            new($"explore_{Cards.cave}")                { OnActionComplete = (CardData card, string action) => card.Id == Cards.cave && action == "complete_harvest"                                                                            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },
            new($"explore_{Cards.jungle}")              { OnActionComplete = (CardData card, string action) => card.Id == Cards.jungle && action == "complete_harvest"                                                                          , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.LocationsanityQuestGroupEnum },

            #endregion

            #region Mobsanity

            new($"kill_{Cards.eel}")                    { OnSpecialAction = (string action) => action == $"{Cards.eel}_killed"                                                                                                                  , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.momma_crab}")             { OnSpecialAction = (string action) => action == $"{Cards.momma_crab}_killed"                                                                                                           , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.orc_wizard}")             { OnSpecialAction = (string action) => action == $"{Cards.orc_wizard}_killed"                                                                                                           , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.pirate}")                 { OnSpecialAction = (string action) => action == $"{Cards.pirate}_killed"                                                                                                               , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.seagull}")                { OnSpecialAction = (string action) => action == $"{Cards.seagull}_killed"                                                                                                              , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.shark}")                  { OnSpecialAction = (string action) => action == $"{Cards.shark}_killed"                                                                                                                , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.snake}")                  { OnSpecialAction = (string action) => action == $"{Cards.snake}_killed"                                                                                                                , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.tentacle}")               { OnSpecialAction = (string action) => action == $"{Cards.tentacle}_killed"                                                                                                             , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },
            new($"kill_{Cards.tiger}")                  { OnSpecialAction = (string action) => action == $"{Cards.tiger}_killed"                                                                                                                , PossibleInPeacefulMode = false    , QuestGroup = EnumExtensionHandler.MobsanityQuestGroupEnum },

            #endregion

            #region Structuresanity

            new($"build_{Cards.distillery}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_distillery && action == "finish_blueprint"                                                            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.frigate}")               { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_frigate && action == "finish_blueprint"                                                               , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.gold_mine}")             { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_gold_mine && action == "finish_blueprint"                                                             , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.lighthouse}")            { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_lighthouse && action == "finish_blueprint"                                                            , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.sand_quarry}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_sand_quarry && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },
            new($"build_{Cards.shell_chest}")           { OnActionComplete = (CardData card, string action) => card.Id == Cards.blueprint_shell_chest && action == "finish_blueprint"                                                           , PossibleInPeacefulMode = true     , QuestGroup = EnumExtensionHandler.StructuresanityQuestGroupEnum },

            #endregion
        };

        /// <summary>
        /// List of archived quests.
        /// </summary>
        public static readonly List<Quest> Archived = new()
        {
            #region Spendsanity Quests

            new("buy_1_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 1    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 1 },
            new("buy_2_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 2    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 2 },
            new("buy_3_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 3    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 3 },
            new("buy_4_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 4    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 4 },
            new("buy_5_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 5    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 5 },
            new("buy_6_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 6    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 6 },
            new("buy_7_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 7    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 7 },
            new("buy_8_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 8    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 8 },
            new("buy_9_ap_spendsanity_pack")    { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 9    , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 9 },
            new("buy_10_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 10   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 10},
            new("buy_11_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 11   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 11},
            new("buy_12_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 12   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 12},
            new("buy_13_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 13   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 13},
            new("buy_14_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 14   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 14},
            new("buy_15_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 15   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 15},
            new("buy_16_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 16   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 16},
            new("buy_17_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 17   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 17},
            new("buy_18_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 18   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 18},
            new("buy_19_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 19   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 19},
            new("buy_20_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 20   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 20},
            new("buy_21_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 21   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 21},
            new("buy_22_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 22   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 22},
            new("buy_23_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 23   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 23},
            new("buy_24_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 24   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 24},
            new("buy_25_ap_spendsanity_pack")   { OnSpecialAction = (string action) => action == $"buy_{ModBoosterPacks.spendsanity}_pack" && CommonPatchMethods.GetTimesBoosterPackBought(ModBoosterPacks.spendsanity) >= 25   , DescriptionTermOverride = ModTerms.SpendsanityQuestDescription     , PossibleInPeacefulMode = true      , QuestGroup = EnumExtensionHandler.SpendsanityQuestGroupEnum      , RequiredCount = 25},

            #endregion
        };
    }
}
