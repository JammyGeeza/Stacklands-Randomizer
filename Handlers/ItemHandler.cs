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
        /// Handle the unlocking of booster pack(s).
        /// </summary>
        /// <param name="ideaIds">The ID(s) of the booster pack(s) to be created.</param>
        public static void HandleBoosterPack(string boosterId)
        {
            try
            {
                if (!WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(boosterId))
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(boosterId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle Booster Pack(s). Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Handle the creation of idea (blueprint) cards.
        /// </summary>
        /// <param name="ideaIds">The card ID(s) of the idea(s) to be created.</param>
        public static void HandleIdea(string ideaId)
        {
            try
            {
                // Create card (automatically marks as found)
                WorldManager.instance.CreateCard(
                    WorldManager.instance.GetRandomSpawnPosition(),
                    ideaId,
                    true,
                    false,
                    true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle Idea(s). Reason: '{ex.Message}'.");
            }
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

            // Get whether item has been discovered previously this save or not
            bool itemDiscovered = IsItemDiscovered(mappedItem);

            Debug.Log($"Item '{mappedItem.Name}' already discovered: {itemDiscovered}");

            // Check if we are in-game
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");

                // Log item (only if resource / trap) as undiscovered if forced to create or its current discovered state
                LogResource(mappedItem, !forceCreate && itemDiscovered);
                return;
            }

            // If not forcing to create, check if item has already been discovered
            if (!forceCreate && itemDiscovered)
            {
                Debug.Log($"Item '{mappedItem.Name}' has already been handled. Skipping...");
                return;
            }

            // Text for notification
            string title = string.Empty;

            switch (mappedItem.ItemType)
            {
                case ItemType.BoosterPack:
                    {
                        // Handle unlocking of booster pack
                        HandleBoosterPack(mappedItem.ItemId);

                        title = $"Booster Pack Unlocked!";
                    }
                    break;

                case ItemType.Idea:
                    {
                        // Handle creation of idea
                        HandleIdea(mappedItem.ItemId);

                        title = $"Idea Unlocked!";
                    }
                    break;

                case ItemType.Resource:
                    {
                        // Handle creation of resource(s)
                        HandleResource(mappedItem.ItemId, mappedItem.Amount);

                        // Log as discovered
                        LogResource(mappedItem, true);

                        title = $"Resource{(mappedItem.Amount > 1 ? "s" : "")} Received!";
                    }
                    break;

                case ItemType.Trap:
                    {
                        // Handle creation of trap(s)
                        HandleTrap(mappedItem.ItemId, mappedItem.Amount);

                        // Log as discovered
                        LogResource(mappedItem, true);

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
            if (!forceCreate && itemInfo != null && !itemInfo.Player.Name.Equals(StacklandsRandomizer.instance.PlayerName))
            {
                // Display message
                StacklandsRandomizer.instance.DisplayNotification(
                    title,
                    $"{mappedItem.Name} was sent to you by {itemInfo.Player.Name}\n({itemInfo.LocationName})");
            }
        }

        /// <summary>
        /// Handle the creation of resource cards.
        /// </summary>
        /// <param name="ideaIds">The card IDs of the resource(s) to be created.</param>
        public static bool HandleResource(string resourceId, int amount)
        {
            try
            {
                // Create resources as a stack (automatically marks as found)
                WorldManager.instance.CreateCardStack(
                    WorldManager.instance.GetRandomSpawnPosition(),
                    amount,
                    resourceId,
                    false);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle Idea(s). Reason: '{ex.Message}'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handle the creation of trap cards.
        /// </summary>
        /// <param name="cardId">The card IDs of the trap(s) to be created.</param>
        public static bool HandleTrap(string cardId, int amount)
        {
            try
            {
                // Randomly place each trap
                for (int i = 0; i < amount; i++)
                {
                    WorldManager.instance.CreateCard(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        cardId,
                        true,
                        false,
                        true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to handle trap(s). Reason: '{ex.Message}'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if an item has already been discovered in this game save.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to check.</param>
        /// <returns><see cref="true"/> if discovered, <see cref="false"/> if not.</returns>
        private static bool IsItemDiscovered(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.BoosterPack:
                    {
                        return IsBoosterPackDiscovered(item.ItemId);
                    }

                case ItemType.Idea:
                    {
                        return IsIdeaDiscovered(item.ItemId);
                    }

                case ItemType.Resource:
                    {
                        return IsResourceDiscovered(item.Name);
                    }

                default:
                    {
                        Debug.LogError($"Unhandled ItemType '{item.ItemType}' in {nameof(IsItemDiscovered)}()");
                    }
                    return true; // <- Treat as discovered to try and prevent further action
            }
        }

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
        /// Check if a <see cref="Item"/> of type <see cref="ItemType.Resource"/> has been discovered in the current save.
        /// </summary>
        /// <param name="resourceName">The name of the <see cref="Item"/> to check.</param>
        /// <returns><see cref="true"/> if discovered, <see cref="false"/> if not.</returns>
        public static bool IsResourceDiscovered(string resourceName)
        {
            return WorldManager.instance.SaveExtraKeyValues.GetWithKey(resourceName) is SerializedKeyValuePair kvp
                && Convert.ToBoolean(kvp.Value);
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

        /// <summary>
        /// Log a <see cref="Item"/> of type <see cref="ItemType.Resource"/> as received.
        /// </summary>
        /// <param name="item">The resource <see cref="Item"/> to log.</param>
        /// <param name="discovered">Whether or not the resource has been discovered.</param>
        private static void LogResource(Item item, bool discovered)
        {
            // Check if item is a resource
            if (item.ItemType is ItemType.Resource or ItemType.Trap)
            {
                WorldManager.instance.SaveExtraKeyValues.SetOrAdd(item.Name, discovered.ToString());
            }
        }
    }
}
