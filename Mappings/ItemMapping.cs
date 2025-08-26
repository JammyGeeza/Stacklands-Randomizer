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

            // Booster Packs
            new BoosterItem("Humble Beginnings Booster Pack"        , "basic"                               , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Seeking Wisdom Booster Pack"           , "idea"                                , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Reap & Sow Booster Pack"               , "farming"                             , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Curious Cuisine Booster Pack"          , "cooking"                             , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Logic and Reason Booster Pack"         , "idea2"                               , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("The Armory Booster Pack"               , "equipment"                           , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Explorers Booster Pack"                , "locations"                           , BoosterItem.BoosterType.Unlock    , Board.Mainland),
            new BoosterItem("Order and Structure Booster Pack"      , "structures"                          , BoosterItem.BoosterType.Unlock    , Board.Mainland),

            // Resource Booster Packs
            new BoosterItem("Mainland Resource Booster Pack"        , ModBoosterPacks.mainland_resource     , BoosterItem.BoosterType.Spawn     , Board.Mainland),

            // Ideas
            new IdeaItem("Idea: Animal Pen"         , Cards.blueprint_animalpen       ),
            new IdeaItem("Idea: Axe"                , Cards.blueprint_axe             ),
            new IdeaItem("Idea: Bone Spear"         , Cards.blueprint_bone_spear      ),
            new IdeaItem("Idea: Boomerang"          , Cards.blueprint_boomerang       ),
            new IdeaItem("Idea: Breeding Pen"       , Cards.blueprint_breedingpen     ),
            new IdeaItem("Idea: Brick"              , Cards.blueprint_brick           ),
            new IdeaItem("Idea: Brickyard"          , Cards.blueprint_brickyard       ),
            new IdeaItem("Idea: Butchery"           , Cards.blueprint_slaughterhouse  ),
            new IdeaItem("Idea: Campfire"           , Cards.blueprint_campfire        ),
            new IdeaItem("Idea: Chainmail Armor"    , Cards.blueprint_chainmail_armor ),
            new IdeaItem("Idea: Charcoal"           , Cards.blueprint_charcoal        ),
            new IdeaItem("Idea: Chicken"            , Cards.blueprint_chicken         ),
            new IdeaItem("Idea: Club"               , Cards.blueprint_club            ),
            new IdeaItem("Idea: Coin Chest"         , Cards.blueprint_coinchest       ),
            new IdeaItem("Idea: Cooked Meat"        , Cards.blueprint_cookedmeat      ),
            new IdeaItem("Idea: Crane"              , Cards.blueprint_conveyor        ),
            new IdeaItem("Idea: Dustbin"            , Cards.blueprint_trash_can       ),
            new IdeaItem("Idea: Farm"               , Cards.blueprint_farm            ),
            new IdeaItem("Idea: Frittata"           , Cards.blueprint_frittata        ),
            new IdeaItem("Idea: Fruit Salad"        , Cards.blueprint_fruitsalad      ),
            new IdeaItem("Idea: Garden"             , Cards.blueprint_garden          ),
            new IdeaItem("Idea: Growth"             , Cards.blueprint_growth          ),
            new IdeaItem("Idea: Hammer"             , Cards.blueprint_hammer          ),
            new IdeaItem("Idea: Hotpot"             , Cards.blueprint_hotpot          ),
            new IdeaItem("Idea: House"              , Cards.blueprint_house           ),
            new IdeaItem("Idea: Iron Bar"           , Cards.blueprint_iron_bar        ),
            new IdeaItem("Idea: Iron Mine"          , Cards.blueprint_mine            ),
            new IdeaItem("Idea: Iron Shield"        , Cards.blueprint_iron_shield     ),
            new IdeaItem("Idea: Lumber Camp"        , Cards.blueprint_lumbercamp      ),
            new IdeaItem("Idea: Magic Blade"        , Cards.blueprint_magic_blade     ),
            new IdeaItem("Idea: Magic Glue"         , Cards.blueprint_heavy_foundation),
            new IdeaItem("Idea: Magic Ring"         , Cards.blueprint_magic_ring      ),
            new IdeaItem("Idea: Magic Staff"        , Cards.blueprint_magic_staff     ),
            new IdeaItem("Idea: Magic Tome"         , Cards.blueprint_magic_tome      ),
            new IdeaItem("Idea: Magic Wand"         , Cards.blueprint_magic_wand      ),
            new IdeaItem("Idea: Market"             , Cards.blueprint_market          ),
            new IdeaItem("Idea: Milkshake"          , Cards.blueprint_milkshake       ),
            new IdeaItem("Idea: Offspring"          , Cards.blueprint_offspring       ),
            new IdeaItem("Idea: Omelette"           , Cards.blueprint_omelette        ),
            new IdeaItem("Idea: Pickaxe"            , Cards.blueprint_pickaxe         ),
            new IdeaItem("Idea: Plank"              , Cards.blueprint_planks          ),
            new IdeaItem("Idea: Quarry"             , Cards.blueprint_quarry          ),
            new IdeaItem("Idea: Resource Chest"     , Cards.blueprint_resourcechest   ),
            new IdeaItem("Idea: Sawmill"            , Cards.blueprint_sawmill         ),
            new IdeaItem("Idea: Shed"               , Cards.blueprint_shed            ),
            new IdeaItem("Idea: Slingshot"          , Cards.blueprint_slingshot       ),
            new IdeaItem("Idea: Smelter"            , Cards.blueprint_smelting        ),
            new IdeaItem("Idea: Smithy"             , Cards.blueprint_smithy          ),
            new IdeaItem("Idea: Spear"              , Cards.blueprint_woodenweapons   ),
            new IdeaItem("Idea: Spiked Plank"       , Cards.blueprint_spiked_plank    ),
            new IdeaItem("Idea: Stew"               , Cards.blueprint_stew            ),
            new IdeaItem("Idea: Stick"              , Cards.blueprint_carving         ),
            new IdeaItem("Idea: Stove"              , Cards.blueprint_stove           ),
            new IdeaItem("Idea: Sword"              , Cards.blueprint_ironweapons     ),
            new IdeaItem("Idea: Temple"             , Cards.blueprint_temple          ),
            new IdeaItem("Idea: Throwing Stars"     , Cards.blueprint_throwing_star   ),
            new IdeaItem("Idea: University"         , Cards.blueprint_university      ),
            new IdeaItem("Idea: Warehouse"          , Cards.blueprint_warehouse       ),
            new IdeaItem("Idea: Wooden Shield"      , Cards.blueprint_wooden_shield   ),

            #endregion

            #region The Dark Forest

            new IdeaItem("Idea: Stable Portal"      , Cards.blueprint_stable_portal   ),

            #endregion

            #region The Island

            // TODO:    Use target board to determine where to spawn booster packs (if they are spawnable)

            // Booster Packs
            new BoosterItem("On the Shore Booster Pack"             , "island2"                         , BoosterItem.BoosterType.Unlock    , Board.Island),
            new BoosterItem("Island of Ideas Booster Pack"          , "island_ideas_1"                  , BoosterItem.BoosterType.Unlock    , Board.Island),
            new BoosterItem("Grilling and Brewing Booster Pack"     , "island_cooking"                  , BoosterItem.BoosterType.Unlock    , Board.Island),
            new BoosterItem("Island Insights Booster Pack"          , "island_ideas_2"                  , BoosterItem.BoosterType.Unlock    , Board.Island),
            new BoosterItem("Advanced Archipelago Booster Pack"     , "island_advanced"                 , BoosterItem.BoosterType.Unlock    , Board.Island),
            new BoosterItem("Enclave Explorers Booster Pack"        , "island_locations"                , BoosterItem.BoosterType.Unlock    , Board.Island),

            // Resource Booster Packs
            new BoosterItem("Island Resource Booster Pack"          , ModBoosterPacks.island_resource   , BoosterItem.BoosterType.Spawn     , Board.Island),

            // Ideas
            new IdeaItem("Idea: Aquarium"               , Cards.blueprint_aquarium          ),
            new IdeaItem("Idea: Blunderbuss"            , Cards.blueprint_blunderbuss       ),
            new IdeaItem("Idea: Bone Staff"             , Cards.blueprint_bone_staff        ),
            new IdeaItem("Idea: Bow"                    , Cards.blueprint_bow               ),
            new IdeaItem("Idea: Bottle of Rum"          , Cards.blueprint_rum               ),
            new IdeaItem("Idea: Bottle of Water"        , Cards.blueprint_fill_bottle       ),
            new IdeaItem("Idea: Broken Bottle"          , Cards.blueprint_broken_bottle     ),
            new IdeaItem("Idea: Cathedral"              , Cards.blueprint_cathedral         ),
            new IdeaItem("Idea: Ceviche"                , Cards.blueprint_ceviche           ),
            new IdeaItem("Idea: Charcoal"               , Cards.blueprint_charcoal          ),
            new IdeaItem("Idea: Coin"                   , Cards.blueprint_coin              ),
            new IdeaItem("Idea: Composter"              , Cards.blueprint_composter         ),
            new IdeaItem("Idea: Crossbow"               , Cards.blueprint_crossbow          ),
            new IdeaItem("Idea: Distillery"             , Cards.blueprint_distillery        ),
            new IdeaItem("Idea: Empty Bottle"           , Cards.blueprint_bottle            ),
            new IdeaItem("Idea: Fabric"                 , Cards.blueprint_fabric            ),
            new IdeaItem("Idea: Fishing Rod"            , Cards.blueprint_fishing_rod       ),
            new IdeaItem("Idea: Fish Trap"              , Cards.blueprint_fish_trap         ),
            new IdeaItem("Idea: Forest Amulet"          , Cards.blueprint_amulet_of_forest  ),
            new IdeaItem("Idea: Frigate"                , Cards.blueprint_frigate           ),
            new IdeaItem("Idea: Glass"                  , Cards.blueprint_glass             ),
            new IdeaItem("Idea: Gold Bar"               , Cards.blueprint_gold_bar          ),
            new IdeaItem("Idea: Gold Mine"              , Cards.blueprint_gold_mine         ),
            new IdeaItem("Idea: Golden Chestplate"      , Cards.blueprint_gold_chestplate   ),
            new IdeaItem("Idea: Greenhouse"             , Cards.blueprint_greenhouse        ),
            new IdeaItem("Idea: Lighthouse"             , Cards.blueprint_lighthouse        ),
            new IdeaItem("Idea: Mess Hall"              , Cards.blueprint_mess_hall         ),
            new IdeaItem("Idea: Mountain Amulet"        , Cards.blueprint_amulet_of_mountain),
            new IdeaItem("Idea: Resource Magnet"        , Cards.blueprint_resource_magnet   ),
            new IdeaItem("Idea: Rope"                   , Cards.blueprint_rope              ),
            new IdeaItem("Idea: Rowboat"                , Cards.blueprint_rowboat           ),
            new IdeaItem("Idea: Sacred Key"             , Cards.blueprint_sacred_key        ),
            new IdeaItem("Idea: Sail"                   , Cards.blueprint_sail              ),
            new IdeaItem("Idea: Sand Quarry"            , Cards.blueprint_sand_quarry       ),
            new IdeaItem("Idea: Sandstone"              , Cards.blueprint_sandstone         ),
            new IdeaItem("Idea: Seafood Stew"           , Cards.blueprint_seafood_stew      ),
            new IdeaItem("Idea: Shell Chest"            , Cards.blueprint_shell_chest       ),
            new IdeaItem("Idea: Sloop"                  , Cards.blueprint_sloop             ),
            new IdeaItem("Idea: Sushi"                  , Cards.blueprint_sushi             ),
            new IdeaItem("Idea: Tamago Sushi"           , Cards.blueprint_tamago_sushi      ),
            new IdeaItem("Idea: Wishing Well"           , Cards.blueprint_wishing_well      ),
            new IdeaItem("Idea: Wizard Robe"            , Cards.blueprint_wizard_robe       ),

            #endregion

            #region Board Expansions

            // Expansion Bundles
            new CardItem("Mainland Board Expansion"                 , ModCards.board_expansion      , Board.Mainland),
            new CardItem("Island Board Expansion"                   , ModCards.board_expansion      , Board.Island),

            #endregion

            #region Traps

            // Traps
            new MiscItem("Feed Villagers Trap"      , string.Empty                          , ItemHelper.TriggerFeedVillagers                                  , null      ),
            new MiscItem("Mob Trap"                 , string.Empty                          , () => ItemHelper.SpawnRandomCard(TrapMapping.MobTrapCards)       , null      ),
            new MiscItem("Sell Cards Trap"          , string.Empty                          , ItemHelper.TriggerSellCards                                      , null      ),
            new MiscItem("Strange Portal Trap"      , ModCards.trap_strange_portal          , () => ItemHelper.SpawnCard(ModCards.trap_strange_portal)         , null      ),

            #endregion
        };
    }
}
