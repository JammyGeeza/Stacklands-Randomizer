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
        /// Handle the creation of idea (blueprint) cards.
        /// </summary>
        /// <param name="ideaIds">The card IDs of the idea(s) to be created.</param>
        public static bool HandleIdeas(List<string> ideaIds)
        {
            try
            {
                foreach (string id in ideaIds)
                {
                    // Create card
                    WorldManager.instance.CreateCard(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        id,
                        true,
                        false,
                        true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle Idea(s). Reason: '{ex.Message}'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handle a received item from the Archipelago server.
        /// </summary>
        /// <param name="itemInfo">The received <see cref="ItemInfo"/> to be handled.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void HandleItem(ItemInfo itemInfo, bool forceCreate = false)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemInfo.ItemName)) is Item mappedItem)
            {
                HandleItem(mappedItem, itemInfo, forceCreate);
            }
            else
            {
                Debug.LogError($"Item '{itemInfo.ItemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Handle a received item from the Archipelago server by the item name.
        /// </summary>
        /// <param name="itemName">The received item name to be handled.</param>
        /// <param name="sentBy">The player that this item was sent by.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void HandleItem(string itemName, bool forceCreate = false)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemName)) is Item mappedItem)
            {
                HandleItem(mappedItem, null, forceCreate);
            }
            else
            {
                Debug.LogError($"Item '{itemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Handle a received item from the Archipelago server by an <see cref="Item"/> mapping.
        /// </summary>
        /// <param name="mappedItem">The received <see cref="Item"/> to be handled.</param>
        /// <param name="sentBy">The player that this item was sent by.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        private static void HandleItem(Item mappedItem, ItemInfo? itemInfo, bool forceCreate = false)
        {
            Debug.Log($"Handling item '{mappedItem.Name}'...");

            // Get whether item has been handled previously or not
            bool itemhandled = IsItemHandled(mappedItem);

            // Check if we are in-game
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");

                // Log item (if forcing to create or is unhandled, log it as unhandled)
                LogItem(mappedItem, !forceCreate && itemhandled);
                return;
            }

            // If not forcing to create, check if item has already been handled
            if (!forceCreate && itemhandled)
            {
                Debug.Log($"Item '{mappedItem.Name}' has already been handled. Skipping...");
                return;
            }

            switch (mappedItem.ItemType)
            {
                case ItemType.BoosterPack:
                    {
                        // Log item as handled
                        LogItem(mappedItem, true);

                        // No need to do anything else, logging it to the logged items is enough.
                        // IsBoosterPackLogged(...) is called from Patches to unlock boosters.
                    }
                    break;

                case ItemType.Idea:
                    {
                        // Handle creation of ideas
                        bool result = HandleIdeas(mappedItem.ItemIds);

                        // Log idea with handled resuly
                        LogItem(mappedItem, result);
                    }
                    break;

                case ItemType.Resource:
                    {
                        // Handle creation of resources
                        bool result = HandleResources(mappedItem.ItemIds);

                        // Log resource with handled result
                        LogItem(mappedItem, result);
                    }
                    break;

                default:
                    {
                        Debug.LogError($"Unhandled item type '{mappedItem.ItemType}'");

                        // Log as unhandled
                        LogItem(mappedItem, false);
                    }
                    return;
            }

            // If not forcefully created and not sent from the current player...
            if (!forceCreate && itemInfo != null && !itemInfo.Player.Name.Equals(StacklandsRandomizer.instance.PlayerName))
            {
                // Display message
                StacklandsRandomizer.instance.DisplayNotification(
                    "Item Received!",
                    $"{mappedItem.Name} was sent to you by {itemInfo.Player.Name} ({itemInfo.LocationName})");
            }
        }

        /// <summary>
        /// Handle the creation of resource cards.
        /// </summary>
        /// <param name="ideaIds">The card IDs of the resource(s) to be created.</param>
        public static bool HandleResources(List<string> ideaIds)
        {
            try
            {
                // Get a random spawn position
                Vector3 spawnPosition = WorldManager.instance.GetRandomSpawnPosition();

                foreach (string id in ideaIds)
                {
                    // Create card
                    WorldManager.instance.CreateCard(
                        spawnPosition,
                        id,
                        true,
                        true, // <-- Add all to stack in same position
                        true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle Idea(s). Reason: '{ex.Message}'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if a booster pack item has already been logged.
        /// </summary>
        /// <param name="boosterId"></param>
        /// <returns></returns>
        public static bool IsBoosterPackLogged(string boosterId)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.ItemType == ItemType.BoosterPack && m.ItemIds.Contains(boosterId)) is Item item)
            {
                return IsItemHandled(item);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool IsItemHandled(Item item)
        {
            return WorldManager.instance.SaveExtraKeyValues.GetWithKey(item.Name) is SerializedKeyValuePair log
                && Convert.ToBoolean(log.Value);
        }

        /// <summary>
        /// Log an item as received from the server.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to be logged.</param>
        /// <param name="handled">Whether or not this item has been handled.</param>
        private static void LogItem(Item item, bool handled)
        {
            WorldManager.instance.SaveExtraKeyValues
                .SetOrAdd(item.Name, handled.ToString());
        }
    }
}
