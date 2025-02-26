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
            new() { Name = "Humble Beginnings Booster Pack", ItemIds = [ "basic" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Seeking Wisdom Booster Pack", ItemIds = [ "idea" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Reap & Sow Booster Pack", ItemIds = [ "farming" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Curious Cuisine Booster Pack", ItemIds = [ "cooking" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Logic and Reason Booster Pack", ItemIds = [ "idea2" ], ItemType = ItemType.BoosterPack },
            new() { Name = "The Armory Booster Pack", ItemIds = [ "equipment" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Explorers Booster Pack", ItemIds = [ "locations" ], ItemType = ItemType.BoosterPack },
            new() { Name = "Order and Structure Booster Pack", ItemIds = [ "structures" ], ItemType = ItemType.BoosterPack },

            // Blueprint Bundles
            new() { Name = "Idea: Animal Pen", ItemIds = [ Cards.blueprint_animalpen ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Axe", ItemIds = [ Cards.blueprint_axe ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Bone Spear", ItemIds = [ Cards.blueprint_bone_spear ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Boomerang", ItemIds = [ Cards.blueprint_boomerang ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Breeding Pen", ItemIds = [ Cards.blueprint_breedingpen ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Brick", ItemIds = [ Cards.blueprint_brick ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Brickyard", ItemIds = [ Cards.blueprint_brickyard ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Butchery", ItemIds = [ Cards.blueprint_slaughterhouse ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Campfire", ItemIds = [ Cards.blueprint_campfire ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Chainmail Armor", ItemIds = [ Cards.blueprint_chainmail_armor ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Chicken", ItemIds = [ Cards.blueprint_chicken ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Club", ItemIds = [ Cards.blueprint_club ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Coin Chest", ItemIds = [ Cards.blueprint_coinchest ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Crane", ItemIds = [ Cards.blueprint_conveyor ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Dustbin", ItemIds = [ Cards.blueprint_trash_can ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Farm", ItemIds = [ Cards.blueprint_farm ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Fruit Salad", ItemIds = [ Cards.blueprint_fruitsalad ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Garden", ItemIds = [ Cards.blueprint_garden ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Growth", ItemIds = [ Cards.blueprint_growth ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Hammer", ItemIds = [ Cards.blueprint_hammer ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Hotpot", ItemIds = [ Cards.blueprint_hotpot ], ItemType = ItemType.Idea },
            new() { Name = "Idea: House", ItemIds = [ Cards.blueprint_house ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Bar", ItemIds = [ Cards.blueprint_iron_bar ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Mine", ItemIds = [ Cards.blueprint_mine ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Iron Shield", ItemIds = [ Cards.blueprint_iron_shield ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Lumber Camp", ItemIds = [ Cards.blueprint_lumbercamp ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Blade", ItemIds = [ Cards.blueprint_magic_blade ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Glue", ItemIds = [ Cards.blueprint_heavy_foundation ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Ring", ItemIds = [ Cards.blueprint_magic_ring ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Staff", ItemIds = [ Cards.blueprint_magic_staff ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Tome", ItemIds = [ Cards.blueprint_magic_tome ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Magic Wand", ItemIds = [ Cards.blueprint_magic_wand ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Market", ItemIds = [ Cards.blueprint_market ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Milkshake", ItemIds = [ Cards.blueprint_milkshake ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Offspring", ItemIds = [ Cards.blueprint_offspring ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Pickaxe", ItemIds = [ Cards.blueprint_pickaxe ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Plank", ItemIds = [ Cards.blueprint_planks ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Quarry", ItemIds = [ Cards.blueprint_quarry ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Resource Chest", ItemIds = [ Cards.blueprint_resourcechest ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Resource Magnet", ItemIds = [ Cards.blueprint_resource_magnet ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Sawmill", ItemIds = [ Cards.blueprint_sawmill ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Shed", ItemIds = [ Cards.blueprint_shed ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Slingshot", ItemIds = [ Cards.blueprint_slingshot ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Smelter", ItemIds = [ Cards.blueprint_smelting ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Smithy", ItemIds = [ Cards.blueprint_smithy ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Spear", ItemIds = [ Cards.blueprint_woodenweapons ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Spiked Plank", ItemIds = [ Cards.blueprint_spiked_plank ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Stew", ItemIds = [ Cards.blueprint_stew ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Stick", ItemIds = [ Cards.blueprint_carving ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Stove", ItemIds = [ Cards.blueprint_stove ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Sword", ItemIds = [ Cards.blueprint_ironweapons ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Temple", ItemIds = [ Cards.blueprint_temple ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Throwing Stars", ItemIds = [ Cards.blueprint_throwing_star ], ItemType = ItemType.Idea },
            new() { Name = "Idea: University", ItemIds = [ Cards.blueprint_university ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Warehouse", ItemIds = [ Cards.blueprint_warehouse ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Wizard Robe", ItemIds = [ Cards.blueprint_wizard_robe ], ItemType = ItemType.Idea },
            new() { Name = "Idea: Wooden Shield", ItemIds = [ Cards.blueprint_wooden_shield ], ItemType = ItemType.Idea },

            // Equipment Bundles
            new() { Name = "Club", ItemIds = [ Cards.club ], ItemType = ItemType.Resource },
            new() { Name = "Magic Wand", ItemIds = [ Cards.magic_wand ], ItemType = ItemType.Resource },
            new() { Name = "Spear", ItemIds = [ Cards.spear ], ItemType = ItemType.Resource },
            new() { Name = "Sword", ItemIds = [ Cards.sword ], ItemType = ItemType.Resource },
            new() { Name = "Wooden Shield", ItemIds = [ Cards.wooden_shield ], ItemType = ItemType.Resource },

            // Resource Bundles
            new() { Name = "Flint x5", ItemIds = [ Cards.flint, Cards.flint, Cards.flint, Cards.flint, Cards.flint ], ItemType = ItemType.Resource },
            new() { Name = "Iron Ore x5", ItemIds = [ Cards.iron_bar, Cards.iron_bar, Cards.iron_bar, Cards.iron_bar, Cards.iron_bar ], ItemType = ItemType.Resource },
            new() { Name = "Poop x5", ItemIds = [ Cards.poop, Cards.poop, Cards.poop, Cards.poop, Cards.poop ], ItemType = ItemType.Resource },
            new() { Name = "Stone x5", ItemIds = [ Cards.stone, Cards.stone, Cards.stone, Cards.stone, Cards.stone ], ItemType = ItemType.Resource },
            new() { Name = "Wood x5", ItemIds = [ Cards.wood, Cards.wood, Cards.wood, Cards.wood, Cards.wood ], ItemType = ItemType.Resource },

            #endregion

            #region Island Items

            // To be added...

            #endregion
        };
    }
}
