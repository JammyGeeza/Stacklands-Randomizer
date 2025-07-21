using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemMapping
    {
        public static readonly List<Item> Map = new()
        {
            #region Mainland

            new BoosterItem("Humble Beginnings Booster Pack"        , "basic"      ),
            new BoosterItem("Seeking Wisdom Booster Pack"           , "idea"       ),
            new BoosterItem("Reap & Sow Booster Pack"               , "farming"    ),
            new BoosterItem("Curious Cuisine Booster Pack"          , "cooking"    ),
            new BoosterItem("Logic and Reason Booster Pack"         , "idea2"      ),
            new BoosterItem("The Armory Booster Pack"               , "equipment"  ),
            new BoosterItem("Explorers Booster Pack"                , "locations"  ),
            new BoosterItem("Order and Structure Booster Pack"      , "structures" ),

            // Blueprint Bundles
            new IdeaItem("Idea: Animal Pen"           , Cards.blueprint_animalpen           ),
            new IdeaItem("Idea: Axe"                  , Cards.blueprint_axe                 ),
            new IdeaItem("Idea: Bone Spear"           , Cards.blueprint_bone_spear          ),
            new IdeaItem("Idea: Boomerang"            , Cards.blueprint_boomerang           ),
            new IdeaItem("Idea: Breeding Pen"         , Cards.blueprint_breedingpen         ),
            new IdeaItem("Idea: Brick"                , Cards.blueprint_brick               ),
            new IdeaItem("Idea: Brickyard"            , Cards.blueprint_brickyard           ),
            new IdeaItem("Idea: Butchery"             , Cards.blueprint_slaughterhouse      ),
            new IdeaItem("Idea: Campfire"             , Cards.blueprint_campfire            ),
            new IdeaItem("Idea: Chainmail Armor"      , Cards.blueprint_chainmail_armor     ),
            new IdeaItem("Idea: Charcoal"             , Cards.blueprint_charcoal            ),
            new IdeaItem("Idea: Chicken"              , Cards.blueprint_chicken             ),
            new IdeaItem("Idea: Club"                 , Cards.blueprint_club                ),
            new IdeaItem("Idea: Coin Chest"           , Cards.blueprint_coinchest           ),
            new IdeaItem("Idea: Cooked Meat"          , Cards.blueprint_cookedmeat          ),
            new IdeaItem("Idea: Crane"                , Cards.blueprint_conveyor            ),
            new IdeaItem("Idea: Dustbin"              , Cards.blueprint_trash_can           ),
            new IdeaItem("Idea: Farm"                 , Cards.blueprint_farm                ),
            new IdeaItem("Idea: Frittata"             , Cards.blueprint_frittata            ),
            new IdeaItem("Idea: Fruit Salad"          , Cards.blueprint_fruitsalad          ),
            new IdeaItem("Idea: Garden"               , Cards.blueprint_garden              ),
            new IdeaItem("Idea: Growth"               , Cards.blueprint_growth              ),
            new IdeaItem("Idea: Hammer"               , Cards.blueprint_hammer              ),
            new IdeaItem("Idea: Hotpot"               , Cards.blueprint_hotpot              ),
            new IdeaItem("Idea: House"                , Cards.blueprint_house               ),
            new IdeaItem("Idea: Iron Bar"             , Cards.blueprint_iron_bar            ),
            new IdeaItem("Idea: Iron Mine"            , Cards.blueprint_mine                ),
            new IdeaItem("Idea: Iron Shield"          , Cards.blueprint_iron_shield         ),
            new IdeaItem("Idea: Lumber Camp"          , Cards.blueprint_lumbercamp          ),
            new IdeaItem("Idea: Magic Blade"          , Cards.blueprint_magic_blade         ),
            new IdeaItem("Idea: Magic Glue"           , Cards.blueprint_heavy_foundation    ),
            new IdeaItem("Idea: Magic Ring"           , Cards.blueprint_magic_ring          ),
            new IdeaItem("Idea: Magic Staff"          , Cards.blueprint_magic_staff         ),
            new IdeaItem("Idea: Magic Tome"           , Cards.blueprint_magic_tome          ),
            new IdeaItem("Idea: Magic Wand"           , Cards.blueprint_magic_wand          ),
            new IdeaItem("Idea: Market"               , Cards.blueprint_market              ),
            new IdeaItem("Idea: Mess Hall"            , Cards.blueprint_mess_hall           ),
            new IdeaItem("Idea: Milkshake"            , Cards.blueprint_milkshake           ),
            new IdeaItem("Idea: Omelette"             , Cards.blueprint_omelette            ),
            new IdeaItem("Idea: Offspring"            , Cards.blueprint_offspring           ),
            new IdeaItem("Idea: Pickaxe"              , Cards.blueprint_pickaxe             ),
            new IdeaItem("Idea: Plank"                , Cards.blueprint_planks              ),
            new IdeaItem("Idea: Quarry"               , Cards.blueprint_quarry              ),
            new IdeaItem("Idea: Resource Chest"       , Cards.blueprint_resourcechest       ),
            new IdeaItem("Idea: Sawmill"              , Cards.blueprint_sawmill             ),
            new IdeaItem("Idea: Shed"                 , Cards.blueprint_shed                ),
            new IdeaItem("Idea: Slingshot"            , Cards.blueprint_slingshot           ),
            new IdeaItem("Idea: Smelter"              , Cards.blueprint_smelting            ),
            new IdeaItem("Idea: Smithy"               , Cards.blueprint_smithy              ),
            new IdeaItem("Idea: Spear"                , Cards.blueprint_woodenweapons       ),
            new IdeaItem("Idea: Spiked Plank"         , Cards.blueprint_spiked_plank        ),
            new IdeaItem("Idea: Stew"                 , Cards.blueprint_stew                ),
            new IdeaItem("Idea: Stick"                , Cards.blueprint_carving             ),
            new IdeaItem("Idea: Stove"                , Cards.blueprint_stove               ),
            new IdeaItem("Idea: Sword"                , Cards.blueprint_ironweapons         ),
            new IdeaItem("Idea: Temple"               , Cards.blueprint_temple              ),
            new IdeaItem("Idea: Throwing Stars"       , Cards.blueprint_throwing_star       ),
            new IdeaItem("Idea: University"           , Cards.blueprint_university          ),
            new IdeaItem("Idea: Warehouse"            , Cards.blueprint_warehouse           ),
            new IdeaItem("Idea: Wooden Shield"        , Cards.blueprint_wooden_shield       ),

            // Resource Bundles
            // TODO: Potentially replace these with an AP Resource Bundle booster pack?
            new StackItem("Apple Tree"          , Cards.apple_tree      , 1     , string.Empty     ),
            new StackItem("Berry x3"            , Cards.berry           , 3     , string.Empty     ),
            new StackItem("Berry Bush"          , Cards.berrybush       , 1     , string.Empty     ),
            new StackItem("Coin"                , Cards.gold            , 1     , string.Empty     ), 
            new StackItem("Coin x5"             , Cards.gold            , 5     , string.Empty     ), 
            new StackItem("Coin x10"            , Cards.gold            , 10    , string.Empty     ),
            new StackItem("Egg x3"              , Cards.egg             , 3     , string.Empty     ), 
            new StackItem("Flint x3"            , Cards.flint           , 3     , string.Empty     ), 
            new StackItem("Iron Deposit"        , Cards.iron_deposit    , 1     , string.Empty     ), 
            new StackItem("Iron Ore x3"         , Cards.iron_ore        , 3     , string.Empty     ), 
            new StackItem("Milk x3"             , Cards.milk            , 3     , string.Empty     ),   
            new StackItem("Rock"                , Cards.rock            , 1     , string.Empty     ),   
            new StackItem("Stick x3"            , Cards.stick           , 3     , string.Empty     ), 
            new StackItem("Stone x3"            , Cards.stone           , 3     , string.Empty     ),
            new StackItem("Tree"                , Cards.tree            , 1     , string.Empty     ),
            new StackItem("Wood x3"             , Cards.wood            , 3     , string.Empty     ),

            #endregion

            #region The Dark Forest

            new IdeaItem("Idea: Stable Portal"  , Cards.blueprint_stable_portal ),

            #endregion

            #region Board Expansions

            // Expansion Bundles
            new StackItem("Board Expansion: Shed"           , Cards.shed            , 1     , Board.Mainland      ),
            new StackItem("Board Expansion: Warehouse"      , Cards.warehouse       , 1     , Board.Mainland      ),

            #endregion

            #region Traps

            // Traps
            new MiscItem("Feed Villagers Trap"      , string.Empty      , ItemHandler.TriggerFeedVillagers      , null     ),
            //new MiscItem("Flip Trap"                , string.Empty      , ItemReceivedHandler.FlipRandomCard            , null     ),
            new MiscItem("Mob Trap"                 , string.Empty      , ItemHandler.SpawnRandomMob            , null     ),
            new MiscItem("Sell Cards Trap"          , string.Empty      , ItemHandler.TriggerSellCards          , null     ),
            new MiscItem("Structure Trap"           , string.Empty      , ItemHandler.SpawnRandomStructure      , null     ),

            #endregion
        };



        //public static readonly List<Item> Map = new()
        //{
        //    #region Mainland Items

        //    // Booster Pack Bundles
        //    new() { Name = "Humble Beginnings Booster Pack"     , ItemId = "basic"                              , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Seeking Wisdom Booster Pack"        , ItemId = "idea"                               , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Reap & Sow Booster Pack"            , ItemId = "farming"                            , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Curious Cuisine Booster Pack"       , ItemId = "cooking"                            , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Logic and Reason Booster Pack"      , ItemId = "idea2"                              , ItemType = ItemType.BoosterPack },
        //    new() { Name = "The Armory Booster Pack"            , ItemId = "equipment"                          , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Explorers Booster Pack"             , ItemId = "locations"                          , ItemType = ItemType.BoosterPack },
        //    new() { Name = "Order and Structure Booster Pack"   , ItemId = "structures"                         , ItemType = ItemType.BoosterPack },

        //    // Blueprint Bundles
        //    new() { Name = "Idea: Animal Pen"                   , ItemId = Cards.blueprint_animalpen            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Axe"                          , ItemId = Cards.blueprint_axe                  , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Bone Spear"                   , ItemId = Cards.blueprint_bone_spear           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Boomerang"                    , ItemId = Cards.blueprint_boomerang            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Breeding Pen"                 , ItemId = Cards.blueprint_breedingpen          , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Brick"                        , ItemId = Cards.blueprint_brick                , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Brickyard"                    , ItemId = Cards.blueprint_brickyard            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Butchery"                     , ItemId = Cards.blueprint_slaughterhouse       , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Campfire"                     , ItemId = Cards.blueprint_campfire             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Chainmail Armor"              , ItemId = Cards.blueprint_chainmail_armor      , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Charcoal"                     , ItemId = Cards.blueprint_charcoal             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Chicken"                      , ItemId = Cards.blueprint_chicken              , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Club"                         , ItemId = Cards.blueprint_club                 , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Coin Chest"                   , ItemId = Cards.blueprint_coinchest            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Cooked Meat"                  , ItemId = Cards.blueprint_cookedmeat           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Crane"                        , ItemId = Cards.blueprint_conveyor             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Dustbin"                      , ItemId = Cards.blueprint_trash_can            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Farm"                         , ItemId = Cards.blueprint_farm                 , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Frittata"                     , ItemId = Cards.blueprint_frittata             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Fruit Salad"                  , ItemId = Cards.blueprint_fruitsalad           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Garden"                       , ItemId = Cards.blueprint_garden               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Growth"                       , ItemId = Cards.blueprint_growth               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Hammer"                       , ItemId = Cards.blueprint_hammer               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Hotpot"                       , ItemId = Cards.blueprint_hotpot               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: House"                        , ItemId = Cards.blueprint_house                , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Iron Bar"                     , ItemId = Cards.blueprint_iron_bar             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Iron Mine"                    , ItemId = Cards.blueprint_mine                 , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Iron Shield"                  , ItemId = Cards.blueprint_iron_shield          , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Lumber Camp"                  , ItemId = Cards.blueprint_lumbercamp           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Blade"                  , ItemId = Cards.blueprint_magic_blade          , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Glue"                   , ItemId = Cards.blueprint_heavy_foundation     , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Ring"                   , ItemId = Cards.blueprint_magic_ring           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Staff"                  , ItemId = Cards.blueprint_magic_staff          , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Tome"                   , ItemId = Cards.blueprint_magic_tome           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Magic Wand"                   , ItemId = Cards.blueprint_magic_wand           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Market"                       , ItemId = Cards.blueprint_market               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Mess Hall"                    , ItemId = Cards.blueprint_mess_hall            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Milkshake"                    , ItemId = Cards.blueprint_milkshake            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Omelette"                     , ItemId = Cards.blueprint_omelette             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Offspring"                    , ItemId = Cards.blueprint_offspring            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Pickaxe"                      , ItemId = Cards.blueprint_pickaxe              , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Plank"                        , ItemId = Cards.blueprint_planks               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Quarry"                       , ItemId = Cards.blueprint_quarry               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Resource Chest"               , ItemId = Cards.blueprint_resourcechest        , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Sawmill"                      , ItemId = Cards.blueprint_sawmill              , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Shed"                         , ItemId = Cards.blueprint_shed                 , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Slingshot"                    , ItemId = Cards.blueprint_slingshot            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Smelter"                      , ItemId = Cards.blueprint_smelting             , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Smithy"                       , ItemId = Cards.blueprint_smithy               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Spear"                        , ItemId = Cards.blueprint_woodenweapons        , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Spiked Plank"                 , ItemId = Cards.blueprint_spiked_plank         , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Stew"                         , ItemId = Cards.blueprint_stew                 , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Stick"                        , ItemId = Cards.blueprint_carving              , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Stove"                        , ItemId = Cards.blueprint_stove                , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Sword"                        , ItemId = Cards.blueprint_ironweapons          , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Temple"                       , ItemId = Cards.blueprint_temple               , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Throwing Stars"               , ItemId = Cards.blueprint_throwing_star        , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: University"                   , ItemId = Cards.blueprint_university           , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Warehouse"                    , ItemId = Cards.blueprint_warehouse            , ItemType = ItemType.Idea },
        //    new() { Name = "Idea: Wooden Shield"                , ItemId = Cards.blueprint_wooden_shield        , ItemType = ItemType.Idea },

        //    // Resource Bundles
        //    new() { Name = "Apple Tree"                         , ItemId = Cards.apple_tree                     , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Berry x3"                           , ItemId = Cards.berry                          , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Berry Bush"                         , ItemId = Cards.berrybush                      , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Coin"                               , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Coin x5"                            , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 5 },
        //    new() { Name = "Coin x10"                           , ItemId = Cards.gold                           , ItemType = ItemType.Resource, Amount = 10},
        //    new() { Name = "Egg x3"                             , ItemId = Cards.egg                            , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Flint x3"                           , ItemId = Cards.flint                          , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Iron Deposit"                       , ItemId = Cards.iron_deposit                   , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Iron Ore x3"                        , ItemId = Cards.iron_ore                       , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Milk x3"                            , ItemId = Cards.milk                           , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Rock"                               , ItemId = Cards.rock                           , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Stick x3"                           , ItemId = Cards.stick                          , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Stone x3"                           , ItemId = Cards.stone                          , ItemType = ItemType.Resource, Amount = 3 },
        //    new() { Name = "Tree"                               , ItemId = Cards.tree                           , ItemType = ItemType.Resource, Amount = 1 },
        //    new() { Name = "Wood x3"                            , ItemId = Cards.wood                           , ItemType = ItemType.Resource, Amount = 3 },

        //    // Expansion Bundles
        //    new() { Name = "Board Expansion: Shed"              , ItemId = Cards.shed                           , ItemType = ItemType.Structure, Amount = 1 },
        //    new() { Name = "Board Expansion: Warehouse"         , ItemId = Cards.warehouse                      , ItemType = ItemType.Structure, Amount = 1 },

        //    // Trap Bundles
        //    new() { Name = "Chickens"                           , ItemId = ModCards.chicken                     , ItemType = ItemType.Trap, Amount = 5 },
        //    new() { Name = "Goop"                               , ItemId = Cards.goop                           , ItemType = ItemType.Trap, Amount = 15 },
        //    new() { Name = "Rabbits"                            , ItemId = ModCards.rabbit                      , ItemType = ItemType.Trap, Amount = 5 },
        //    new() { Name = "Rat"                                , ItemId = ModCards.rat                         , ItemType = ItemType.Trap, Amount = 1 },
        //    new() { Name = "Slime"                              , ItemId = ModCards.slime                       , ItemType = ItemType.Trap, Amount = 1 },
        //    new() { Name = "Snake"                              , ItemId = ModCards.snake                       , ItemType = ItemType.Trap, Amount = 1 },

        //    #endregion

        //    #region The Dark Forest Items

        //    new() { Name = "Idea: Stable Portal"                , ItemId = Cards.blueprint_stew                 , ItemType = ItemType.Idea },

        //    #endregion

        //    #region Island Items

        //    // To be added...

        //    #endregion
        //};
    }
}
