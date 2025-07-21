using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemMapping
    {
        public static readonly List<Item> MapV2 = new()
        {
            new() { Name = "Humble Beginnings Booster Pack"     , ReceivedAction = () => ItemHandler.UnlockBoosterPack("basic")                                 },
            new() { Name = "Seeking Wisdom Booster Pack"        , ReceivedAction = () => ItemHandler.UnlockBoosterPack("idea")                                  },
            new() { Name = "Reap & Sow Booster Pack"            , ReceivedAction = () => ItemHandler.UnlockBoosterPack("farming")                               },
            new() { Name = "Curious Cuisine Booster Pack"       , ReceivedAction = () => ItemHandler.UnlockBoosterPack("cooking")                               },
            new() { Name = "Logic and Reason Booster Pack"      , ReceivedAction = () => ItemHandler.UnlockBoosterPack("idea2")                                 },
            new() { Name = "The Armory Booster Pack"            , ReceivedAction = () => ItemHandler.UnlockBoosterPack("equipment")                             },
            new() { Name = "Explorers Booster Pack"             , ReceivedAction = () => ItemHandler.UnlockBoosterPack("locations")                             },
            new() { Name = "Order and Structure Booster Pack"   , ReceivedAction = () => ItemHandler.UnlockBoosterPack("structures")                            },

            // Blueprint Bundles
            new() { Name = "Idea: Animal Pen"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_animalpen)                       },
            new() { Name = "Idea: Axe"                          , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_axe)                             },
            new() { Name = "Idea: Bone Spear"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_bone_spear)                      },
            new() { Name = "Idea: Boomerang"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_boomerang)                       },
            new() { Name = "Idea: Breeding Pen"                 , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_breedingpen)                     },
            new() { Name = "Idea: Brick"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_brick)                           },
            new() { Name = "Idea: Brickyard"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_brickyard)                       },
            new() { Name = "Idea: Butchery"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_slaughterhouse)                  },
            new() { Name = "Idea: Campfire"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_campfire)                        },
            new() { Name = "Idea: Chainmail Armor"              , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_chainmail_armor)                 },
            new() { Name = "Idea: Charcoal"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_charcoal)                        },
            new() { Name = "Idea: Chicken"                      , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_chicken)                         },
            new() { Name = "Idea: Club"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_club)                            },
            new() { Name = "Idea: Coin Chest"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_coinchest)                       },
            new() { Name = "Idea: Cooked Meat"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_cookedmeat)                      },
            new() { Name = "Idea: Crane"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_conveyor)                        },
            new() { Name = "Idea: Dustbin"                      , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_trash_can)                       },
            new() { Name = "Idea: Farm"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_farm)                            },
            new() { Name = "Idea: Frittata"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_frittata)                        },
            new() { Name = "Idea: Fruit Salad"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_fruitsalad)                      },
            new() { Name = "Idea: Garden"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_garden)                          },
            new() { Name = "Idea: Growth"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_growth)                          },
            new() { Name = "Idea: Hammer"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_hammer)                          },
            new() { Name = "Idea: Hotpot"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_hotpot)                          },
            new() { Name = "Idea: House"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_house)                           },
            new() { Name = "Idea: Iron Bar"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_iron_bar)                        },
            new() { Name = "Idea: Iron Mine"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_mine)                            },
            new() { Name = "Idea: Iron Shield"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_iron_shield)                     },
            new() { Name = "Idea: Lumber Camp"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_lumbercamp)                      },
            new() { Name = "Idea: Magic Blade"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_magic_blade)                     },
            new() { Name = "Idea: Magic Glue"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_heavy_foundation)                },
            new() { Name = "Idea: Magic Ring"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_magic_ring)                      },
            new() { Name = "Idea: Magic Staff"                  , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_magic_staff)                     },
            new() { Name = "Idea: Magic Tome"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_magic_tome)                      },
            new() { Name = "Idea: Magic Wand"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_magic_wand)                      },
            new() { Name = "Idea: Market"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_market)                          },
            new() { Name = "Idea: Mess Hall"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_mess_hall)                       },
            new() { Name = "Idea: Milkshake"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_milkshake)                       },
            new() { Name = "Idea: Omelette"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_omelette)                        },
            new() { Name = "Idea: Offspring"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_offspring)                       },
            new() { Name = "Idea: Pickaxe"                      , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_pickaxe)                         },
            new() { Name = "Idea: Plank"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_planks)                          },
            new() { Name = "Idea: Quarry"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_quarry)                          },
            new() { Name = "Idea: Resource Chest"               , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_resourcechest)                   },
            new() { Name = "Idea: Sawmill"                      , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_sawmill)                         },
            new() { Name = "Idea: Shed"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_shed)                            },
            new() { Name = "Idea: Slingshot"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_slingshot)                       },
            new() { Name = "Idea: Smelter"                      , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_smelting)                        },
            new() { Name = "Idea: Smithy"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_smithy)                          },
            new() { Name = "Idea: Spear"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_woodenweapons)                   },
            new() { Name = "Idea: Spiked Plank"                 , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_spiked_plank)                    },
            new() { Name = "Idea: Stew"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_stew)                            },
            new() { Name = "Idea: Stick"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_carving)                         },
            new() { Name = "Idea: Stove"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_stove)                           },
            new() { Name = "Idea: Sword"                        , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_ironweapons)                     },
            new() { Name = "Idea: Temple"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_temple)                          },
            new() { Name = "Idea: Throwing Stars"               , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_throwing_star)                   },
            new() { Name = "Idea: University"                   , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_university)                      },
            new() { Name = "Idea: Warehouse"                    , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_warehouse)                       },
            new() { Name = "Idea: Wooden Shield"                , ReceivedAction = () => ItemHandler.SpawnCard(Cards.blueprint_wooden_shield)                   },

            // Resource Bundles
            new() { Name = "Apple Tree"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.apple_tree, true)                          },
            new() { Name = "Berry x3"                           , ReceivedAction = () => ItemHandler.SpawnStack(Cards.berry, 3, true)                           },
            new() { Name = "Berry Bush"                         , ReceivedAction = () => ItemHandler.SpawnCard(Cards.berrybush, true)                           },
            new() { Name = "Coin"                               , ReceivedAction = () => ItemHandler.SpawnCard(Cards.gold, true)                                },
            new() { Name = "Coin x5"                            , ReceivedAction = () => ItemHandler.SpawnStack(Cards.gold, 5, true)                            },
            new() { Name = "Coin x10"                           , ReceivedAction = () => ItemHandler.SpawnStack(Cards.gold, 10, true)                           },
            new() { Name = "Egg x3"                             , ReceivedAction = () => ItemHandler.SpawnStack(Cards.egg, 3, true)                             },
            new() { Name = "Flint x3"                           , ReceivedAction = () => ItemHandler.SpawnStack(Cards.flint, 3, true)                           },
            new() { Name = "Iron Deposit"                       , ReceivedAction = () => ItemHandler.SpawnCard(Cards.iron_deposit, true)                        },
            new() { Name = "Iron Ore x3"                        , ReceivedAction = () => ItemHandler.SpawnStack(Cards.iron_ore, 3, true)                        },
            new() { Name = "Milk x3"                            , ReceivedAction = () => ItemHandler.SpawnStack(Cards.milk, 3, true)                            },
            new() { Name = "Rock"                               , ReceivedAction = () => ItemHandler.SpawnCard(Cards.rock, true)                                },
            new() { Name = "Stick x3"                           , ReceivedAction = () => ItemHandler.SpawnStack(Cards.stick, 3, true)                           },
            new() { Name = "Stone x3"                           , ReceivedAction = () => ItemHandler.SpawnStack(Cards.stone, 3, true)                           },
            new() { Name = "Tree"                               , ReceivedAction = () => ItemHandler.SpawnCard(Cards.tree, true)                                },
            new() { Name = "Wood x3"                            , ReceivedAction = () => ItemHandler.SpawnStack(Cards.wood, 3, true)                            },

            // Expansion Bundles
            new() { Name = "Board Expansion: Shed"              , ReceivedAction = () => ItemHandler.SpawnCardToBoard(Board.Mainland, Cards.shed, true)         },
            new() { Name = "Board Expansion: Warehouse"         , ReceivedAction = () => ItemHandler.SpawnCardToBoard(Board.Mainland, Cards.warehouse, true)    },

            // Traps
            new() { Name = "Eat Trap"                           , ReceivedAction = () => ItemHandler.TriggerFeedVillagers()                                     },
            //new() { Name = "Flip Trap"                          , ReceivedAction = () => ItemHandler.TriggerEvent()                                             },
            new() { Name = "Mob Trap"                           , ReceivedAction = () => ItemHandler.SpawnCard(Cards.rat, true)                                 },
            new() { Name = "Structure Trap"                     , ReceivedAction = () => ItemHandler.SpawnCard(Cards.strange_portal, true)                      },

        };



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

            // Expansion Bundles
            new() { Name = "Board Expansion: Shed"              , ItemId = Cards.shed                           , ItemType = ItemType.Structure, Amount = 1 },
            new() { Name = "Board Expansion: Warehouse"         , ItemId = Cards.warehouse                      , ItemType = ItemType.Structure, Amount = 1 },

            // Trap Bundles
            new() { Name = "Chickens"                           , ItemId = ModCards.chicken                     , ItemType = ItemType.Trap, Amount = 5 },
            new() { Name = "Goop"                               , ItemId = Cards.goop                           , ItemType = ItemType.Trap, Amount = 15 },
            new() { Name = "Rabbits"                            , ItemId = ModCards.rabbit                      , ItemType = ItemType.Trap, Amount = 5 },
            new() { Name = "Rat"                                , ItemId = ModCards.rat                         , ItemType = ItemType.Trap, Amount = 1 },
            new() { Name = "Slime"                              , ItemId = ModCards.slime                       , ItemType = ItemType.Trap, Amount = 1 },
            new() { Name = "Snake"                              , ItemId = ModCards.snake                       , ItemType = ItemType.Trap, Amount = 1 },

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
