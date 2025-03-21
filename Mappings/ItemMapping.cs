using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemMapping
    {
        public static readonly List<Item> Map = new()
        {
            #region Mainland Items

            // Booster Pack Bundles
            new() { Name = "Humble Beginnings Booster Pack"     , ItemId = "basic"                              , ItemType = ItemType.BoosterPack },
            new() { Name = "Seeking Wisdom Booster Pack"        , ItemId = "idea"                               , ItemType = ItemType.BoosterPack },
            new() { Name = "Reap & Sow Booster Pack"            , ItemId = "farming"                            , ItemType = ItemType.BoosterPack },
            new() { Name = "Curious Cuisine Booster Pack"       , ItemId = "cooking"                            , ItemType = ItemType.BoosterPack },
            new() { Name = "Logic and Reason Booster Pack"      , ItemId = "idea2"                              , ItemType = ItemType.BoosterPack },
            new() { Name = "The Armory Booster Pack"            , ItemId = "equipment"                          , ItemType = ItemType.BoosterPack },
            new() { Name = "Explorers Booster Pack"             , ItemId = "locations"                          , ItemType = ItemType.BoosterPack },
            new() { Name = "Order and Structure Booster Pack"   , ItemId = "structures"                         , ItemType = ItemType.BoosterPack },

            // Blueprint Bundles
            new() { Name = "Idea: Animal Pen"                   , ItemId = Cards.blueprint_animalpen            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Axe"                          , ItemId = Cards.blueprint_axe                  , ItemType = ItemType.Idea },
            new() { Name = "Idea: Bone Spear"                   , ItemId = Cards.blueprint_bone_spear           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Boomerang"                    , ItemId = Cards.blueprint_boomerang            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Breeding Pen"                 , ItemId = Cards.blueprint_breedingpen          , ItemType = ItemType.Idea },
            new() { Name = "Idea: Brick"                        , ItemId = Cards.blueprint_brick                , ItemType = ItemType.Idea },
            new() { Name = "Idea: Brickyard"                    , ItemId = Cards.blueprint_brickyard            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Butchery"                     , ItemId = Cards.blueprint_slaughterhouse       , ItemType = ItemType.Idea },
            new() { Name = "Idea: Campfire"                     , ItemId = Cards.blueprint_campfire             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Chainmail Armor"              , ItemId = Cards.blueprint_chainmail_armor      , ItemType = ItemType.Idea },
            new() { Name = "Idea: Charcoal"                     , ItemId = Cards.blueprint_charcoal             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Chicken"                      , ItemId = Cards.blueprint_chicken              , ItemType = ItemType.Idea },
            new() { Name = "Idea: Club"                         , ItemId = Cards.blueprint_club                 , ItemType = ItemType.Idea },
            new() { Name = "Idea: Coin Chest"                   , ItemId = Cards.blueprint_coinchest            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Cooked Meat"                  , ItemId = Cards.blueprint_cookedmeat           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Crane"                        , ItemId = Cards.blueprint_conveyor             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Dustbin"                      , ItemId = Cards.blueprint_trash_can            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Farm"                         , ItemId = Cards.blueprint_farm                 , ItemType = ItemType.Idea },
            new() { Name = "Idea: Frittata"                     , ItemId = Cards.blueprint_frittata             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Fruit Salad"                  , ItemId = Cards.blueprint_fruitsalad           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Garden"                       , ItemId = Cards.blueprint_garden               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Growth"                       , ItemId = Cards.blueprint_growth               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Hammer"                       , ItemId = Cards.blueprint_hammer               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Hotpot"                       , ItemId = Cards.blueprint_hotpot               , ItemType = ItemType.Idea },
            new() { Name = "Idea: House"                        , ItemId = Cards.blueprint_house                , ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Bar"                     , ItemId = Cards.blueprint_iron_bar             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Mine"                    , ItemId = Cards.blueprint_mine                 , ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Shield"                  , ItemId = Cards.blueprint_iron_shield          , ItemType = ItemType.Idea },
            new() { Name = "Idea: Lumber Camp"                  , ItemId = Cards.blueprint_lumbercamp           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Blade"                  , ItemId = Cards.blueprint_magic_blade          , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Glue"                   , ItemId = Cards.blueprint_heavy_foundation     , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Ring"                   , ItemId = Cards.blueprint_magic_ring           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Staff"                  , ItemId = Cards.blueprint_magic_staff          , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Tome"                   , ItemId = Cards.blueprint_magic_tome           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Wand"                   , ItemId = Cards.blueprint_magic_wand           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Market"                       , ItemId = Cards.blueprint_market               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Mess Hall"                    , ItemId = Cards.blueprint_mess_hall            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Milkshake"                    , ItemId = Cards.blueprint_milkshake            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Omelette"                     , ItemId = Cards.blueprint_omelette             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Offspring"                    , ItemId = Cards.blueprint_offspring            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Pickaxe"                      , ItemId = Cards.blueprint_pickaxe              , ItemType = ItemType.Idea },
            new() { Name = "Idea: Plank"                        , ItemId = Cards.blueprint_planks               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Quarry"                       , ItemId = Cards.blueprint_quarry               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Resource Chest"               , ItemId = Cards.blueprint_resourcechest        , ItemType = ItemType.Idea },
            new() { Name = "Idea: Sawmill"                      , ItemId = Cards.blueprint_sawmill              , ItemType = ItemType.Idea },
            new() { Name = "Idea: Shed"                         , ItemId = Cards.blueprint_shed                 , ItemType = ItemType.Idea },
            new() { Name = "Idea: Slingshot"                    , ItemId = Cards.blueprint_slingshot            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Smelter"                      , ItemId = Cards.blueprint_smelting             , ItemType = ItemType.Idea },
            new() { Name = "Idea: Smithy"                       , ItemId = Cards.blueprint_smithy               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Spear"                        , ItemId = Cards.blueprint_woodenweapons        , ItemType = ItemType.Idea },
            new() { Name = "Idea: Spiked Plank"                 , ItemId = Cards.blueprint_spiked_plank         , ItemType = ItemType.Idea },
            new() { Name = "Idea: Stew"                         , ItemId = Cards.blueprint_stew                 , ItemType = ItemType.Idea },
            new() { Name = "Idea: Stick"                        , ItemId = Cards.blueprint_carving              , ItemType = ItemType.Idea },
            new() { Name = "Idea: Stove"                        , ItemId = Cards.blueprint_stove                , ItemType = ItemType.Idea },
            new() { Name = "Idea: Sword"                        , ItemId = Cards.blueprint_ironweapons          , ItemType = ItemType.Idea },
            new() { Name = "Idea: Temple"                       , ItemId = Cards.blueprint_temple               , ItemType = ItemType.Idea },
            new() { Name = "Idea: Throwing Stars"               , ItemId = Cards.blueprint_throwing_star        , ItemType = ItemType.Idea },
            new() { Name = "Idea: University"                   , ItemId = Cards.blueprint_university           , ItemType = ItemType.Idea },
            new() { Name = "Idea: Warehouse"                    , ItemId = Cards.blueprint_warehouse            , ItemType = ItemType.Idea },
            new() { Name = "Idea: Wooden Shield"                , ItemId = Cards.blueprint_wooden_shield        , ItemType = ItemType.Idea },

            // Resource Bundles
            new() { Name = "Apple Tree"                         , ItemId = Cards.apple_tree                     , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Berry x3"                           , ItemId = Cards.berry                          , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Berry Bush"                         , ItemId = Cards.berrybush                      , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Coin"                               , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Coin x5"                            , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 5 },
            new() { Name = "Coin x10"                           , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 10},
            new() { Name = "Egg x3"                             , ItemId = Cards.egg                            , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Flint x3"                           , ItemId = Cards.flint                          , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Iron Deposit"                       , ItemId = Cards.iron_deposit                   , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Iron Ore x3"                        , ItemId = Cards.iron_ore                       , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Milk x3"                            , ItemId = Cards.milk                           , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Rock"                               , ItemId = Cards.rock                           , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Stick x3"                           , ItemId = Cards.stick                          , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Stone x3"                           , ItemId = Cards.stone                          , ItemType = ItemType.Resource, Amount = 3 },
            new() { Name = "Tree"                               , ItemId = Cards.tree                           , ItemType = ItemType.Resource, Amount = 1 },
            new() { Name = "Wood x3"                            , ItemId = Cards.wood                           , ItemType = ItemType.Resource, Amount = 3 },

            // Trap Bundles
            new() { Name = "Get Gooped!"                        , ItemId = Cards.goop                           , ItemType = ItemType.Trap, Amount = 20 },

            #endregion

            #region The Dark Forest Items

            new() { Name = "Idea: Stable Portal"                , ItemId = Cards.blueprint_stew                 , ItemType = ItemType.Idea },

            #endregion

            #region Island Items

            // To be added...

            #endregion
        };
    }
}
