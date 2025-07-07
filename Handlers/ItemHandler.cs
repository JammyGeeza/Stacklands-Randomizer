using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemHandler
    {
        /// <summary>
        /// Check if a booster pack has been discovered in the current save.
        /// </summary>
        /// <param name="boosterId">The ID of the booster pack to check.</param>
        /// <returns><see cref="true"/> if discovered, <see cref="false"/> if not.</returns>
        public static bool IsBoosterPackDiscovered(string boosterId)
        {
            return WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(boosterId);
        }

        /// <summary>
        /// Check if an idea has been discovered in the current save.
        /// </summary>
        /// <param name="ideaId">The ID of the idea to check.</param>
        /// <returns><see cref="true"/> if discovered, <see cref="false"/> if not.</returns>
        public static bool IsIdeaDiscovered(string ideaId)
        {
            return WorldManager.instance.CurrentSave.FoundCardIds.Contains(ideaId);
        }


        /// <summary>
        /// Log a filler <see cref="Item"/> (of type <see cref="ItemType.Resource"/>) as received in this session.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> (of type <see cref="ItemType.Resource"/>) to be logged.</param>
        /// <param name="totaloverride">(OPTIONAL) Override the current stored total. If left blank, current total will be incremented by 1.</param>
        public static void LogFillerItem(Item item, int? totaloverride = null)
        {
            //LogFillerItem(item.Name, totaloverride);

            if (item.ItemType is ItemType.Resource)
            {
                // Has this item already been logged?
                if (WorldManager.instance.SaveExtraKeyValues.GetWithKey(item.Name) is SerializedKeyValuePair kvp)
                {
                    // Add to count or override with new value, if provided
                    WorldManager.instance.SaveExtraKeyValues.SetOrAdd(item.Name, (totaloverride ?? Convert.ToInt32(kvp.Value) + 1).ToString());
                }
                else
                {
                    // Set count as 1 or override with new value, if provided.
                    WorldManager.instance.SaveExtraKeyValues.SetOrAdd(item.Name, (totaloverride ?? 1).ToString());
                }
            }
            else
            {
                Debug.LogError($"Invalid item type '{item.ItemType}' cannot be logged.");
            }
        }

        /// <summary>
        /// Spawn a received item from the Archipelago server.
        /// </summary>
        /// <param name="itemInfo">The received <see cref="ItemInfo"/> to be spawned.</param>
        public static void SpawnItem(ItemInfo itemInfo)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemInfo.ItemName)) is Item mappedItem)
            {
                SpawnItem(mappedItem, itemInfo);
            }
            else
            {
                Debug.LogError($"Item '{itemInfo.ItemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Spawn a received item from the Archipelago server by the item name.
        /// </summary>
        /// <param name="itemName">The received item name to be spawned.</param>
        public static void SpawnItem(string itemName)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemName)) is Item mappedItem)
            {
                SpawnItem(mappedItem, null);
            }
            else
            {
                Debug.LogError($"Item '{itemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Spawn a received item from the Archipelago server by an <see cref="Item"/> mapping.
        /// </summary>
        /// <param name="mappedItem">The received <see cref="Item"/> to be spawned.</param>
        /// <param name="itemInfo">Accompanying <see cref="ItemInfo"/> for in-game notification logic.</param>
        private static void SpawnItem(Item mappedItem, ItemInfo? itemInfo)
        {
            Debug.Log($"Handling item '{mappedItem.Name}'...");

            // Check if we are in-game
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");
                return;
            }

            // Text for notification
            string title = string.Empty;

            switch (mappedItem.ItemType)
            {
                case ItemType.BoosterPack:
                    {
                        // Handle unlocking of booster pack
                        UnlockBoosterPackItem(mappedItem);

                        title = $"Booster Pack Received!";
                    }
                    break;

                case ItemType.Idea:
                    {
                        // Handle creation of idea
                        SpawnIdeaItem(mappedItem);

                        title = $"Idea Received!";
                    }
                    break;

                case ItemType.Resource:
                    {
                        // Spawn the resource item
                        SpawnResourceItem(mappedItem);

                        title = $"Resource Received!";
                    }
                    break;

                case ItemType.Trap:
                    {
                        // Spawn the trap item
                        SpawnTrapItem(mappedItem);

                        title = $"Trap Received!";
                    }
                    break;

                default:
                    {
                        Debug.LogError($"Unhandled item type '{mappedItem.ItemType}'");
                    }
                    return;
            }

            // If not forcefully created and not sent from the current player...
            if (itemInfo != null && !itemInfo.Player.Name.Equals(StacklandsRandomizer.instance.PlayerName))
            {
                // Display message
                StacklandsRandomizer.instance.DisplayNotification(
                    title,
                    $"{mappedItem.Name} was sent to you by {itemInfo.Player.Name}\n({itemInfo.LocationName})");
            }
        }

        /// <summary>
        /// Spawn required cards for an <see cref="Item"/> of type <see cref="ItemType.Idea"/>.
        /// </summary>
        /// <param name="item">The idea item to be spawned.</param>
        public static void SpawnIdeaItem(Item item)
        {
            if (item.ItemType is ItemType.Idea)
            {
                try
                {
                    // Create card (automatically marks as found)
                    WorldManager.instance.CreateCard(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        item.ItemId,
                        true,
                        false,
                        true);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to spawn Idea item '{item.Name}'. Reason: '{ex.Message}'.");
                }
            }
            else
            {
                Debug.LogError($"Item '{item.Name}' is of type '{item.ItemType}' so cannot be spawned as an Idea.");
            }
        }

        /// <summary>
        /// Spawn required cards for an <see cref="Item"/> of type <see cref="ItemType.Resource"/>.
        /// </summary>
        /// <param name="item">The resource item to be spawned.</param>
        public static void SpawnResourceItem(Item item)
        {
            if (item.ItemType is ItemType.Resource)
            {
                try
                {
                    // Create resources as a stack (automatically marks as found)
                    WorldManager.instance.CreateCardStack(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        item.Amount,
                        item.ItemId,
                        false);

                    // Increment the received count of this filler item.
                    LogFillerItem(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to spawn Resource item '{item.Name}'. Reason: '{ex.Message}'.");
                }
            }
            else
            {
                Debug.LogError($"Item '{item.Name}' is of type '{item.ItemType}' so cannot be spawned as a Resource.");
            }
        }

        /// <summary>
        /// Spawn required cards for an <see cref="Item"/> of type <see cref="ItemType.Trap"/>.
        /// </summary>
        /// <param name="item">The trap item to be spawned.</param>
        public static void SpawnTrapItem(Item item)
        {
            if (item.ItemType is ItemType.Trap)
            {
                try
                {
                    // Randomly place each trap
                    for (int i = 0; i < item.Amount; i++)
                    {
                        WorldManager.instance.CreateCard(
                            WorldManager.instance.GetRandomSpawnPosition(),
                            item.ItemId,
                            true,
                            false,
                            true);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to spawn Trap item '{item.Name}'. Reason: '{ex.Message}'.");
                }
            }
            else
            {
                Debug.LogError($"Item '{item.Name}' is of type '{item.ItemType}' so cannot be spawned as a Trap.");
            }
        }

        /// <summary>
        /// Unlock required booster pack for an <see cref="Item"/> of type <see cref="ItemType.BoosterPack"/>.
        /// </summary>
        /// <param name="item">The booster pack to be unlocked.</param>
        public static void UnlockBoosterPackItem(Item item)
        {
            if (item.ItemType is ItemType.BoosterPack)
            {
                try
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(item.ItemId);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to spawn Booster Pack item '{item.Name}'. Reason: '{ex.Message}'.");
                }
            }
            else
            {
                Debug.LogError($"Item '{item.Name}' is of type '{item.ItemType}' so cannot be spawned as a Booster Pack.");
            }
        }
    }
}
