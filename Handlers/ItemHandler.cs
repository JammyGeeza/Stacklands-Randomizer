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
        /// 
        /// </summary>
        /// <param name="items"></param>
        public static void HandleBulk(IEnumerable<string> itemIds, bool forceCreate = false)
        {
            HandleBulk(itemIds.Select(id => ItemMapping.Map.SingleOrDefault(m => m.ItemId == id)).ToArray(), forceCreate);
        }

        /// <summary>
        /// Handle a bulk set of received <see cref="ItemInfo"/>.
        /// </summary>
        /// <param name="items">The set of <see cref="ItemInfo"/> to be handled.</param>
        public static void HandleBulk(IEnumerable<ItemInfo> items, bool forceCreate = false)
        {
            HandleBulk(items.Select(item => ItemMapping.Map.SingleOrDefault(m => m.Name == item.ItemName)).ToArray(), forceCreate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public static void HandleBulk(IEnumerable<Item> items, bool forceCreate = false)
        {
            Debug.Log($"Handling bulk set of {items.Count()} items...");

            // Group by item type
            foreach (IGrouping<ItemType, Item> typeGroup in items.GroupBy(item => item.ItemType))
            {
                Debug.Log($"Handling {typeGroup.Count()} items of type '{typeGroup.Key}'...");

                switch (typeGroup.Key)
                {
                    case ItemType.BoosterPack:
                        {
                            // Handle booster packs
                            HandleBulkBoosterPacks(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Idea:
                        {
                            // Handle bulk ideas
                            HandleBulkIdeas(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Resource:
                        {
                            HandleBulkResources(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Trap:
                        {
                            HandleBulkTraps(typeGroup, forceCreate);
                        }
                        break;

                    default:
                        {
                            Debug.LogError($"Unhandled item type '{typeGroup.Key}'.");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Handle the unlocking of a booster pack.
        /// </summary>
        /// <param name="boosterId">The ID of the booster pack to be created.</param>
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
        /// 
        /// </summary>
        /// <param name="boosters">A list of items of type <see cref="ItemType.BoosterPack"/> to be handled.</param>
        /// <param name="forceCreate"></param>
        public static void HandleBulkBoosterPacks(IEnumerable<Item> boosters, bool forceCreate = false)
        {
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");
                return;
            }

            foreach (Item booster in boosters)
            {
                // If forced to create or not yet discovered, add to found booster IDs
                if (forceCreate || !IsBoosterPackDiscovered(booster.ItemId))
                {
                    Debug.Log($"Creating '{booster.Name}'...");
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(booster.ItemId);
                }
                else
                {
                    Debug.Log($"'{booster.Name}' already discovered. Skipping...");
                }
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
        /// Handle a bulk set of ideas.
        /// </summary>
        /// <param name="ideas">List of Idea items to be handled.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void HandleBulkIdeas(IEnumerable<Item> ideas, bool forceCreate = false)
        {
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");
                return;
            }

            // Generate random position for stack
            Vector3 position = WorldManager.instance.GetRandomSpawnPosition();

            foreach (Item idea in ideas)
            {
                if (forceCreate || !IsIdeaDiscovered(idea.ItemId))
                {
                    Debug.Log($"Creating '{idea.Name}'...");

                    // Create card and attempt to stack
                    WorldManager.instance.CreateCard(
                        position,
                        idea.ItemId,
                        true,
                        true,
                        true);
                }
                else
                {
                    Debug.Log($"'{idea.Name}' has already been received. Skipping...");
                }
            }
        }

        /// <summary>
        /// Handle a received item from the Archipelago server.
        /// </summary>
        /// <param name="itemInfo">The received <see cref="ItemInfo"/> to be handled.</param>
        public static void HandleItem(ItemInfo itemInfo)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemInfo.ItemName)) is Item mappedItem)
            {
                HandleItem(mappedItem, itemInfo);
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
        public static void HandleItem(string itemName)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemName)) is Item mappedItem)
            {
                HandleItem(mappedItem, null);
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
        /// <param name="itemInfo">Accompanying <see cref="ItemInfo"/> for in-game notification logic.</param>
        private static void HandleItem(Item mappedItem, ItemInfo? itemInfo)
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

                        // Add to discovery count
                        LogItem(mappedItem.Name);

                        title = $"Resource{(mappedItem.Amount > 1 ? "s" : "")} Received!";
                    }
                    break;

                case ItemType.Trap:
                    {
                        // Handle creation of trap(s)
                        HandleTrap(mappedItem.ItemId, mappedItem.Amount);

                        // Add to discovery count
                        LogItem(mappedItem.Name);

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
        /// 
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="forceCreate"></param>
        /// <returns></returns>
        public static void HandleBulkResources(IEnumerable<Item> resources, bool forceCreate = false)
        {
            // Group by item name
            foreach (IGrouping<string, Item> itemGroup in resources.GroupBy(item => item.Name))
            {
                // Get group count
                int count = itemGroup.Count();

                Debug.Log($"Handling {count} of '{itemGroup.Key}' resource item...");

                // Get count logged in save (or set to 0 if forcing creation)
                int saveCount = forceCreate ? 0 : CheckItem(itemGroup.Key);

                // If not forcing to create, check if logged item count matches
                if (!forceCreate && count <= saveCount)
                {
                    Debug.Log($"'{itemGroup.Key}' has already been received {saveCount} times. Skipping...");
                    return;
                }

                Debug.Log($"Creating {count - saveCount} of '{itemGroup.Key}' resource...");

                // Spawn missing sets
                for (int i = 0; i < count - saveCount; i++)
                {
                    Item resource = itemGroup.ElementAt(i);
                    HandleResource(resource.ItemId, resource.Amount);
                }

                // Update the received count for this resource
                LogItem(itemGroup.Key, count);
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

        public static void HandleBulkTraps(IEnumerable<Item> traps, bool forceCreate = false)
        {
            // TODO: Ignore trap items for bulk spawning - should only be spawned while playing.

            // Group by item name
            foreach (IGrouping<string, Item> itemGroup in traps.GroupBy(item => item.Name))
            {
                // Get group count
                int count = itemGroup.Count();

                Debug.Log($"Handling {count} of '{itemGroup.Key}' trap item...");

                // Get count logged in save (or set to 0 if forcing creation)
                int saveCount = forceCreate ? 0 : CheckItem(itemGroup.Key);

                // If not forcing to create, check if logged item count matches
                if (!forceCreate && count <= saveCount)
                {
                    Debug.Log($"'{itemGroup.Key}' has already been received {saveCount} times. Skipping...");
                    return;
                }

                Debug.Log($"Creating {count - saveCount} of '{itemGroup.Key}'...");

                // Spawn any missing sets
                for (int i = 0; i < count - saveCount; i++)
                {
                    Item resource = itemGroup.ElementAt(i);
                    HandleTrap(resource.ItemId, resource.Amount);
                }

                // Log total discovered amount
                LogItem(itemGroup.Key, count);
            }
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

                case ItemType.Trap:
                    {
                        return IsTrapDiscovered(item.Name);
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
        /// Check if a <see cref="Item"/> of type <see cref="ItemType.Trap"/> has been discovered in the current save.
        /// </summary>
        /// <param name="resourceName">The name of the <see cref="Item"/> to check.</param>
        /// <returns><see cref="true"/> if discovered, <see cref="false"/> if not.</returns>
        public static bool IsTrapDiscovered(string trapName)
        {
            return IsResourceDiscovered(trapName);
        }

        /// <summary>
        /// Check how many of an <see cref="Item"/> have been logged in this save. 
        /// </summary>
        /// <param name="item">The name of the item to check.</param>
        /// <returns>The amount of times this item has been logged.</returns>
        private static int CheckItem(string itemName)
        {
            // Attempt to get current logged count from save.
            if (WorldManager.instance.SaveExtraKeyValues.GetWithKey(itemName) is SerializedKeyValuePair kvp)
            {
                // If exists, return current count
                return Convert.ToInt32(kvp.Value);
            }
            else
            {
                // Otherwise, return zero
                return 0;
            }
        }

        /// <summary>
        /// Log an <see cref="Item"/> as received in this save.
        /// </summary>
        /// <param name="itemName">The name of the item to log.</param>
        /// <param name="total">(OPTIONAL) Override the current stored total.</param>
        private static void LogItem(string itemName, int? totalOverride = null)
        {
            // Check if item has already been logged
            if (WorldManager.instance.SaveExtraKeyValues.GetWithKey(itemName) is SerializedKeyValuePair kvp)
            {
                // If it has, update the current logged count
                WorldManager.instance.SaveExtraKeyValues.SetOrAdd(itemName, (totalOverride ?? Convert.ToInt32(kvp.Value) + 1).ToString());
            }
            else
            {
                // If it hasn't, create an entry
                WorldManager.instance.SaveExtraKeyValues.SetOrAdd(itemName, (totalOverride ?? 1).ToString());
            }
        }
    }
}
