//using Archipelago.MultiClient.Net.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//namespace Stacklands_Randomizer_Mod
//{
//    public static class ItemSyncedHandler
//    {
//        /// <summary>
//        /// Get how many times a specified item has been received in this session.
//        /// </summary>
//        /// <param name="item">The <see cref="Item"/> to retrieve the count for.</param>
//        /// <returns>The amount of times the item has been received - returns 0 if not found.</returns>
//        private static int GetRepeatItemCount(Item item)
//        {
//            return WorldManager.instance.SaveExtraKeyValues.GetWithKey(item.Name) is SerializedKeyValuePair kvp
//                ? Convert.ToInt32(kvp.Value)
//                : 0;
//        }

//        /// <summary>
//        /// Sync a list of items to the current game save.
//        /// </summary>
//        /// <param name="itemNames">A list of all item names to be synced.</param>
//        public static void SyncItems(IEnumerable<string> itemNames, bool forceCreate = false)
//        {
//            // If not in game, skip
//            if (StacklandsRandomizer.instance.IsInGame)
//            {
//                SyncItems(itemNames.Select(name => ItemMapping.Map.SingleOrDefault(m => m.Name == name)).ToArray(), forceCreate);
//            }
//            else
//            {
//                Debug.Log($"Not currently in-game, skipping item sync...");
//            }
//        }

//        /// <summary>
//        /// Sync a list of items to the current game save.
//        /// </summary>
//        /// <param name="items">A list of all <see cref="ItemInfo"/> to be synced.</param>
//        public static void SyncItems(IEnumerable<ItemInfo> items, bool forceCreate = false)
//        {
//            if (StacklandsRandomizer.instance.IsInGame)
//            {
//                SyncItems(items.Select(item => ItemMapping.Map.SingleOrDefault(m => m.Name == item.ItemName)).ToArray(), forceCreate);
//            }
//            else
//            {
//                Debug.Log($"Not currently in-game, skipping item sync...");
//            }
//        }

//        /// <summary>
//        /// Sync a list of items to the current game save.
//        /// </summary>
//        /// <param name="items">A list of all <see cref="Item"/> to be synced.</param>
//        public static void SyncItems(IEnumerable<Item> items, bool forceCreate = false)
//        {
//            Debug.Log($"Syncing bulk set of {items.Count()} items...");

//            // Group items by item type
//            foreach (IGrouping<ItemType, Item> typeGroup in items.GroupBy(item => item.ItemType))
//            {
//                Debug.Log($"Handling {typeGroup.Count()} items of type '{typeGroup.Key}'...");

//                switch (typeGroup.Key)
//                {
//                    // Items in singles
//                    case ItemType.BoosterPack:
//                    case ItemType.Idea:
//                        {
//                            SyncSingularItems(typeGroup, forceCreate);
//                        }
//                        break;

//                    // Items in duplicates
//                    case ItemType.Buff:
//                    case ItemType.Resource:
//                    case ItemType.Structure:
//                        {
//                            SyncRepeatItems(typeGroup, forceCreate);
//                        }
//                        break;

//                    case ItemType.Trap:
//                        {
//                            Debug.LogWarning("Trap items are not synced to prevent a run being swamped with new trap items.");
//                        }
//                        break;

//                    default:
//                        {
//                            Debug.LogError($"Unhandled item type '{typeGroup.Key}'.");
//                        }
//                        break;
//                }
//            }
//        }

//        /// <summary>
//        /// Sync items that are only ever received once.
//        /// </summary>
//        /// <param name="singularitems">List of <see cref="Item"/> of type <see cref="ItemType.BoosterPack"/> or <see cref="ItemType.Idea"/> to be synced.</param>
//        /// <param name="forceCreate">Whether or not logic should be bypassed to force the creation of this item.</param>
//        private static void SyncSingularItems(IEnumerable<Item> singularitems, bool forceCreate = false)
//        {
//            if (!singularitems.Any())
//            {
//                Debug.LogWarning($"No items were provided to be synced.");
//                return;
//            }

//            switch (singularitems.FirstOrDefault().ItemType)
//            {
//                case ItemType.BoosterPack:
//                    {
//                        // Invoke received action for each booster pack
//                        foreach (Item booster in singularitems)
//                        {
//                            booster.ReceivedAction.Invoke(forceCreate);
//                        }
//                    }
//                    break;

//                case ItemType.Idea:
//                    {
//                        // Generate random position for stack
//                        Vector3 position = WorldManager.instance.GetRandomSpawnPosition();

//                        // Spawn each idea to the same stack
//                        foreach (Item idea in singularitems)
//                        {
//                            if (forceCreate || !ItemHandler.IsIdeaDiscovered(idea.ItemId))
//                            {
//                                Debug.Log($"Creating item '{idea.Name}'...");

//                                // Create card and attempt to stack
//                                WorldManager.instance.CreateCard(
//                                    position,
//                                    idea.ItemId,
//                                    true,
//                                    true,
//                                    true);
//                            }
//                            else
//                            {
//                                Debug.Log($"'{idea.Name}' has already been received. Skipping...");
//                            }
//                        }
//                    }
//                    break;
//            }

//            foreach(Item item in singularitems)
//            {
//                // Invoke received action
//                item.ReceivedAction.Invoke(forceCreate);
//            }
//        }

//        /// <summary>
//        /// Sync items that can be received multiple times.
//        /// </summary>
//        /// <param name="repeatItems">List of <see cref="Item"/> of type <see cref="ItemType.Buff"/>, <see cref="ItemType.Resource"/> <see cref="ItemType.Structure"/> to be synced.</param>
//        /// <param name="forceCreate">Whether or not logic should be bypassed to force the creation of this item.</param>
//        private static void SyncRepeatItems(IEnumerable<Item> repeatItems, bool forceCreate = false)
//        {
//            // Group buffs by item name
//            foreach (IGrouping<string, Item> itemGroup in repeatItems.GroupBy(item => item.Name))
//            {
//                // Get group count
//                int groupCount = itemGroup.Count();

//                Debug.Log($"Syncing {groupCount} of the '{itemGroup.Key}' item...");

//                // Get count logged in save (or set to 0 if forcing creation)
//                int sessionCount = !forceCreate ? GetRepeatItemCount(itemGroup.First()) : 0;

//                // If not forcing to create, check if item count matches session
//                if (!forceCreate && groupCount <= sessionCount)
//                {
//                    Debug.LogWarning($"Item '{itemGroup.Key}' has already been received {sessionCount} time(s) - skipping...");
//                    return;
//                }

//                Debug.Log($"Item '{itemGroup.Key}' has been received {groupCount - sessionCount} additional time(s) - creating...");

//                // Invoke received action for each un-received repeat item
//                for (int i = 0; i < groupCount - sessionCount; i++)
//                {
//                    Item buff = itemGroup.ElementAt(i);
//                    buff.ReceivedAction.Invoke(forceCreate);
//                }
//            }
//        }

//        /// <summary>
//        /// Sync a set of ideas to the current game save.
//        /// </summary>
//        /// <param name="ideas">List of Idea <see cref="Item"/> of type <see cref="ItemType.Idea"/> to be synced.</param>
//        /// <param name="forceCreate">Whether or not this item should be forcefully created.</param>
//        private static void SyncIdeas(IEnumerable<Item> ideas, bool forceCreate = false)
//        {
//            if (!StacklandsRandomizer.instance.IsInGame)
//            {
//                Debug.Log($"Not currently in game. Skipping...");
//                return;
//            }

//            // Generate random position for stack
//            Vector3 position = WorldManager.instance.GetRandomSpawnPosition();

//            foreach (Item idea in ideas)
//            {
//                if (forceCreate || !ItemHandler.IsIdeaDiscovered(idea.ItemId))
//                {
//                    Debug.Log($"Creating '{idea.Name}'...");

//                    // Create card and attempt to stack
//                    WorldManager.instance.CreateCard(
//                        position,
//                        idea.ItemId,
//                        true,
//                        true,
//                        true);
//                }
//                else
//                {
//                    Debug.Log($"'{idea.Name}' has already been received. Skipping...");
//                }
//            }
//        }
//    }
//}
