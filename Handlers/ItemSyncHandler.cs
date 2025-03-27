using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemSyncHandler
    {
        /// <summary>
        /// Retrieve how many times a specified Filler item has currently been received in this session.
        /// </summary>
        /// <param name="fillerItem">The <see cref="Item"/> of type <see cref="ItemType.Resource"/> to retrieve the count for.</param>
        /// <returns>The amount of times the item has been received or -1 if error.</returns>
        private static int GetFillerItemCount(Item fillerItem)
        {
            return fillerItem.ItemType switch
            {
                ItemType.Resource => WorldManager.instance.SaveExtraKeyValues.GetWithKey(fillerItem.Name) is SerializedKeyValuePair kvp
                    ? Convert.ToInt32(kvp.Value)
                    : 0,

                _ => -1
            };
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="itemNames">A list of all item names to be synced.</param>
        public static void SyncItems(IEnumerable<string> itemNames, bool forceCreate = false)
        {
            SyncItems(itemNames.Select(id => ItemMapping.Map.SingleOrDefault(m => m.ItemId == id)).ToArray(), forceCreate);
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="items">A list of all <see cref="ItemInfo"/> to be synced.</param>
        public static void SyncItems(IEnumerable<ItemInfo> items, bool forceCreate = false)
        {
            SyncItems(items.Select(item => ItemMapping.Map.SingleOrDefault(m => m.Name == item.ItemName)).ToArray(), forceCreate);
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="items">A list of all <see cref="Item"/> to be synced.</param>
        public static void SyncItems(IEnumerable<Item> items, bool forceCreate = false)
        {
            Debug.Log($"Handling bulk set of {items.Count()} items...");

            // Group items by item type
            foreach (IGrouping<ItemType, Item> typeGroup in items.GroupBy(item => item.ItemType))
            {
                Debug.Log($"Handling {typeGroup.Count()} items of type '{typeGroup.Key}'...");

                switch (typeGroup.Key)
                {
                    case ItemType.BoosterPack:
                        {
                            // Sync booster packs
                            SyncBoosterPacks(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Idea:
                        {
                            // Sync ideas
                            SyncIdeas(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Resource:
                        {
                            SyncResources(typeGroup, forceCreate);
                        }
                        break;

                    case ItemType.Trap:
                        {
                            // If a player returns to a game and receives multiple traps, it can completely ruin an entire run.
                            // To prevent this, traps are only spawned during an active session and not synced when returning to a session.

                            Debug.Log($"Trap items are skipped in the sync process to prevent swamping with traps.");
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
        /// Sync a set of booster packs to the current game save.
        /// </summary>
        /// <param name="boosters">List of Booster Pack <see cref="Item"/> of type <see cref="ItemType.BoosterPack"/> to be synced.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void SyncBoosterPacks(IEnumerable<Item> boosters, bool forceCreate = false)
        {
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                Debug.Log($"Not currently in game. Skipping...");
                return;
            }

            foreach (Item booster in boosters)
            {
                // If forced to create or not yet discovered, add to found booster IDs
                if (forceCreate || !ItemHandler.IsBoosterPackDiscovered(booster.ItemId))
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
        /// Sync a set of ideas to the current game save.
        /// </summary>
        /// <param name="ideas">List of Idea <see cref="Item"/> of type <see cref="ItemType.Idea"/> to be synced.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void SyncIdeas(IEnumerable<Item> ideas, bool forceCreate = false)
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
                if (forceCreate || !ItemHandler.IsIdeaDiscovered(idea.ItemId))
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
        /// Sync a set of resources to the current game save.
        /// </summary>
        /// <param name="resources">List of Resource <see cref="Item"/> of type <see cref="ItemType.Resource"/> to be synced.</param>
        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
        public static void SyncResources(IEnumerable<Item> resources, bool forceCreate = false)
        {
            // Group by item name
            foreach (IGrouping<string, Item> itemGroup in resources.GroupBy(item => item.Name))
            {
                // Get group count
                int groupCount = itemGroup.Count();

                Debug.Log($"Handling {groupCount} of '{itemGroup.Key}' resource item...");

                // Get count logged in save (or set to 0 if forcing creation)
                int sessionCount = !forceCreate ? GetFillerItemCount(itemGroup.First()) : 0;

                // If not forcing to create, check if item count matches session
                if (!forceCreate && groupCount <= sessionCount)
                {
                    Debug.Log($"'{itemGroup.Key}' has already been received {sessionCount} times. Skipping...");
                    return;
                }

                Debug.Log($"Creating {groupCount - sessionCount} of '{itemGroup.Key}' resource...");

                // Spawn missing sets
                for (int i = 0; i < groupCount - sessionCount; i++)
                {
                    // Spawn resource stack
                    Item resource = itemGroup.ElementAt(i);
                    ItemHandler.SpawnResourceItem(resource);
                }
            }
        }
    }
}
