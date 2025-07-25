using Stacklands_Randomizer_Mod.Mappings;
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

            new BoosterItem("Humble Beginnings Booster Pack"        , "basic"           , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Seeking Wisdom Booster Pack"           , "idea"            , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Reap & Sow Booster Pack"               , "farming"         , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Curious Cuisine Booster Pack"          , "cooking"         , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Logic and Reason Booster Pack"         , "idea2"           , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("The Armory Booster Pack"               , "equipment"       , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Explorers Booster Pack"                , "locations"       , BoosterItem.BoosterType.Unlock     ),
            new BoosterItem("Order and Structure Booster Pack"      , "structures"      , BoosterItem.BoosterType.Unlock     ),

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

            // Resource Booster Packs
            new BoosterItem("Resource Booster Pack"   , ModBoosterPacks.resource      , BoosterItem.BoosterType.Spawn  ),

            #endregion

            #region The Dark Forest

            new IdeaItem("Idea: Stable Portal"      , Cards.blueprint_stable_portal     ),

            #endregion

            #region Board Expansions

            // Expansion Bundles
            new CardItem("Board Expansion"          , ModCards.board_expansion              , Board.Mainland    ),

            #endregion

            #region Traps

            // Traps
            new MiscItem("Feed Villagers Trap"      , string.Empty                          , ItemHandler.TriggerFeedVillagers                                  , null          ),
            new MiscItem("Mob Trap"                 , string.Empty                          , () => ItemHandler.SpawnRandomCard(TrapMapping.MobTrapCards)       , null          ),
            new MiscItem("Sell Cards Trap"          , string.Empty                          , ItemHandler.TriggerSellCards                                      , null          ),
            new MiscItem("Strange Portal Trap"      , ModCards.trap_strange_portal          , () => ItemHandler.SpawnCard(ModCards.trap_strange_portal)         , null          ),

            #endregion
        };
    }
}
