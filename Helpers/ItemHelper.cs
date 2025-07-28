using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public static class ItemHelper
    {
        /// <summary>
        /// Get a random position on a board.
        /// </summary>
        /// <param name="board">The board to get a random position for.</param>
        /// <returns>A randomly generated position within the board's bounds.</returns>
        private static Vector3 GetRandomBoardPosition(GameBoard board)
        {
            Bounds bounds = board.WorldBounds;

            // Select random X,Z coordinates within board bounds
            float x = Mathf.Lerp(bounds.min.x, bounds.max.x, UnityEngine.Random.Range(0.1f, 0.9f));
            float z = Mathf.Lerp(bounds.min.z, bounds.max.z, UnityEngine.Random.Range(0.1f, 0.9f));

            return new Vector3(x, 0f, z);
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
        /// Log a filler <see cref="Item"/> (of type <see cref="ItemType.Resource"/>) as received in this session.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> (of type <see cref="ItemType.Resource"/>) to be logged.</param>
        /// <param name="totaloverride">(OPTIONAL) Override the current stored total. If left blank, current total will be incremented by 1.</param>
        public static void MarkAsReceived(Item item, int? totaloverride = null)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Marking item '{item.Name}' as received.");

            if (totaloverride is not null)
            {
                KeyValueHelper.SetExtraKeyValue(item.Name, totaloverride.Value);
            }
            else
            {
                // Get existing value and replace - incrementing if exists
                int currentCount = KeyValueHelper.GetExtraKeyValue(item.Name);
                KeyValueHelper.SetExtraKeyValue(item.Name, currentCount > 0 ? currentCount + 1 : 1);
            }

        }

        /// <summary>
        /// Spawn a received item from the Archipelago server.
        /// </summary>
        /// <param name="itemInfo">The received <see cref="ItemInfo"/> to be spawned.</param>
        public static void ReceiveItem(ItemInfo itemInfo)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemInfo.ItemName)) is Item mappedItem)
            {
                ReceiveItem(mappedItem, itemInfo);
            }
            else
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Item '{itemInfo.ItemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Spawn a received item from the Archipelago server by the item name.
        /// </summary>
        /// <param name="itemName">The received item name to be spawned.</param>
        public static void ReceiveItem(string itemName)
        {
            if (ItemMapping.Map.SingleOrDefault(m => m.Matches(itemName)) is Item mappedItem)
            {
                ReceiveItem(mappedItem, null);
            }
            else
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Item '{itemName}' could not be found in the items mapping.");
            }
        }

        /// <summary>
        /// Spawn a received item from the Archipelago server by an <see cref="Item"/> mapping.
        /// </summary>
        /// <param name="mappedItem">The received <see cref="Item"/> to be spawned.</param>
        /// <param name="itemInfo">Accompanying <see cref="ItemInfo"/> for in-game notification logic.</param>
        private static void ReceiveItem(Item mappedItem, ItemInfo? itemInfo)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Handling item '{mappedItem.Name}'...");

            // Ignore if not currently in-game
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                StacklandsRandomizer.instance.ModLogger.LogWarning($"Not currently in game - skipping received item.");
                return;
            }

            switch (mappedItem)
            {
                case BoosterItem booster:
                    {
                        if (booster.Type is BoosterItem.BoosterType.Spawn)
                        {
                            // Spawn booster pack to current board
                            SpawnBoosterPack(booster.ItemId);

                            // Mark item as received
                            MarkAsReceived(booster);
                        }
                        else
                        {
                            // Unlock booster pack
                            UnlockBoosterPack(booster.ItemId);
                        }
                    }
                    break;

                case CardItem card:
                    {
                        // Spawn card to specified board
                        SpawnCardToBoard(card.BoardId, card.ItemId);

                        // Mark item as received
                        MarkAsReceived(card);
                    }
                    break;

                case IdeaItem idea:
                    {
                        // Check if idea has not yet been discovered
                        if (!IsIdeaDiscovered(idea.ItemId))
                        {
                            // Spawn card to current board
                            SpawnCard(idea.ItemId);
                        }
                    }
                    break;

                case MiscItem misc:
                    {
                        // Trigger received action
                        misc.ReceivedAction.Invoke();

                        // Mark item as received
                        MarkAsReceived(misc);
                    }
                    break;

                case StackItem stack:
                    {
                        // Spawn stack to specified board
                        SpawnStackToBoard(stack.BoardId, stack.ItemId, stack.Amount);

                        // Mark item as received
                        MarkAsReceived(stack);
                    }
                    break;

                default:
                    {
                        StacklandsRandomizer.instance.ModLogger.LogWarning($"Item '{mappedItem.Name}' was not handled due to it being an unhandled item type.");
                    }
                    break;
            }

            // If not forcefully created and not sent from the current player...
            if (itemInfo != null && !itemInfo.Player.Name.Equals(StacklandsRandomizer.instance.PlayerName))
            {
                // Display message
                StacklandsRandomizer.instance.DisplayNotification(
                    "Item Received",
                    $"{mappedItem.Name} was sent to you by {itemInfo.Player.Name}\n({itemInfo.LocationName})");
            }
        }

        /// <summary>
        /// Spawn a booster pack to the current board.
        /// </summary>
        /// <param name="boosterId">The ID of the booster pack to be spawned.</param>
        /// <param name="position">(Optional) The position to spawn the booster card in.</param>
        public static void SpawnBoosterPack(string boosterId, Vector3? position = null)
        {
            try
            {
                // Use spawn position or generate one if not provided
                Vector3 spawnPosition = position ?? WorldManager.instance.GetRandomSpawnPosition();

                // Create booster pack
                WorldManager.instance.CreateBoosterpack(
                    spawnPosition,
                    boosterId);
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to spawn booster '{boosterId}'. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Spawn a card to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="position">(Optional) The position to spawn the card in.</param>
        /// <param name="checkAddToStack">(Optional) Add to an existing stack if one exists in the same position.</param>
        public static void SpawnCard(string cardId, Vector3? position = null, bool checkAddToStack = false)
        {
            try
            {
                // Use spawn position or generate one if not provided
                Vector3 spawnPosition = position ?? WorldManager.instance.GetRandomSpawnPosition();

                // Create card (automatically marks as found)
                WorldManager.instance.CreateCard(
                    spawnPosition,
                    cardId,
                    true,
                    checkAddToStack,
                    true);
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to spawn card '{cardId}'. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Spawn a card to a specified board.
        /// </summary>
        /// <param name="boardId">The ID of the board to spawn to.</param>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="position">(Optional) The position to spawn the card at.</param>
        public static void SpawnCardToBoard(string boardId, string cardId, Vector3? position = null, bool checkAddToStack = false)
        {
            try
            {
                // If the target board is the current board, spawn as normal
                if (WorldManager.instance.CurrentBoard.Id == boardId)
                {
                    SpawnCard(cardId, position);
                }

                // Otherwise, spawn to target board
                else if (WorldManager.instance.GetBoardWithId(boardId) is { } board)
                {
                    // Use spawn position or generate one if not provided
                    Vector3 spawnPosition = position ?? GetRandomBoardPosition(board);

                    // Create the card out of view and play no sound
                    CardData cardData = WorldManager.instance.CreateCard(
                        spawnPosition,
                        cardId,
                        true,
                        checkAddToStack,
                        false);

                    // Get normalized board position from world position
                    Vector2 normalizedSpawnPosition = board.WorldPosToNormalizedPos(spawnPosition);

                    // Immediately send the card to the target board
                    WorldManager.instance.SendToBoard(cardData.MyGameCard, board, normalizedSpawnPosition);
                }
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to spawn card '{cardId}' to the board '{boardId}'. Reason: '{ex.Message}'");
            }
        }

        /// <summary>
        /// Spawn a random card from a list of card IDs.
        /// </summary>
        /// <param name="cardIds"></param>
        /// <param name="position"></param>
        /// <param name="checkAddToStack"></param>
        public static void SpawnRandomCard(string[] cardIds, Vector3? position = null, bool checkAddToStack = false)
        {
            // Randomly select card index
            int index = UnityEngine.Random.Range(0, cardIds.Length - 1);

            // Spawn card to current board
            SpawnCard(cardIds[index], position, checkAddToStack);
        }

        public static void SpawnRandomCardAsTrap(string[] cardIds, Vector3? position = null)
        {
            // Randomly select card index
            int index = UnityEngine.Random.Range(0, cardIds.Length - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="cardIds"></param>
        /// <param name="position"></param>
        /// <param name="checkAddToStack"></param>
        public static void SpawnRandomCardToBoard(string boardId, string[] cardIds, Vector3? position = null, bool checkAddToStack = false)
        {
            // Randomly select card index
            int index = UnityEngine.Random.Range(0, cardIds.Length - 1);

            // Spawn card to board
            SpawnCardToBoard(boardId, cardIds[index], position, checkAddToStack);
        }

        /// <summary>
        /// Spawn a card stack to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the card to spawn.</param>
        /// <param name="amount">The amount to spawn in the stack.</param>
        /// <param name="position">(Optional) The position to spawn the stack at.</param>
        public static void SpawnStack(string cardId, int amount, Vector3? position = null)
        {
            try
            {
                // Use spawn position or generate one if not provided
                Vector3 spawnPosition = position ?? WorldManager.instance.GetRandomSpawnPosition();

                // Create stack
                WorldManager.instance.CreateCardStack(
                    spawnPosition,
                    amount,
                    cardId,
                    false);
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to spawn card '{cardId}'. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Spawn a card stack to a specified board.
        /// </summary>
        /// <param name="boardId">The ID of the board to spawn to.</param>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="amount">The amount of this card to spawn.</param>
        /// <param name="log">Whether or not to log receipt of this card.</param>
        public static void SpawnStackToBoard(string boardId, string cardId, int amount, Vector3? position = null)
        {
            try
            {
                // If no board, spawn to current board or if the target board is the current board, spawn as normal
                if (string.IsNullOrEmpty(boardId) || WorldManager.instance.CurrentBoard.Id == boardId)
                {
                    SpawnStack(cardId, amount, position);
                }

                // Otherwise, spawn to target board
                else if (WorldManager.instance.GetBoardWithId(boardId) is { } board)
                {
                    // Use spawn position or generate one if not provided
                    Vector3 spawnPosition = position ?? GetRandomBoardPosition(board);

                    // Create stack
                    GameCard rootCard = WorldManager.instance.CreateCardStack(
                        spawnPosition,
                        amount,
                        cardId,
                        false);

                    StacklandsRandomizer.instance.ModLogger.Log($"Attempting to send {rootCard.CardNameText.text} stack to board: {board.Id}");

                    // Get normalized board position from world position
                    Vector2 normalizedSpawnPosition = board.WorldPosToNormalizedPos(spawnPosition);

                    // Immediately send the card to the target board
                    WorldManager.instance.SendStackToBoard(rootCard, board, normalizedSpawnPosition);
                }
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to spawn card '{cardId}' to the board '{boardId}'. Reason: '{ex.Message}'");
            }
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="itemNames">A list of all item names to be synced.</param>
        /// <param name="isNewRun">Whether this sync is for a new run - if so, certain items will be force-spawned.</param>
        public static void SyncItems(IEnumerable<string> itemNames, bool isNewRun = false)
        {
            SyncItems(itemNames.Select(name => ItemMapping.Map.SingleOrDefault(m => m.Name == name)).ToArray(), isNewRun);
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="items">A list of all <see cref="ItemInfo"/> to be synced.</param>
        /// <param name="isNewRun">Whether this sync is for a new run - if so, certain items will be force-spawned.</param>
        public static void SyncItems(IEnumerable<ItemInfo> items, bool isNewRun = false)
        {
            SyncItems(items.Select(item => ItemMapping.Map.SingleOrDefault(m => m.Name == item.ItemName)).ToArray(), isNewRun);
        }

        /// <summary>
        /// Sync a list of items to the current game save.
        /// </summary>
        /// <param name="items">A list of all <see cref="Item"/> to be synced.</param>
        /// <param name="isNewRun">Whether this sync is for a new run - if so, certain items will be force-spawned.</param>
        public static void SyncItems(IEnumerable<Item> items, bool isNewRun = false)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing bulk set of {items.Count()} items...");

            // Ignore if not currently in-game
            if (!StacklandsRandomizer.instance.IsInGame)
            {
                StacklandsRandomizer.instance.ModLogger.LogWarning($"Not currently in-game - skipping item sync...");
                return;
            }

            // Group items by item type
            foreach (IGrouping<ItemType, Item> typeGroup in items.GroupBy(item => item.ItemType))
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Handling {typeGroup.Count()} items of type '{typeGroup.Key}'...");

                switch (typeGroup.Key)
                {
                    // Items in singles
                    case ItemType.BoosterPack:
                        {
                            SyncBoosters(typeGroup.Select(booster => booster as BoosterItem));
                        }
                        break;

                    case ItemType.Card:
                        {
                            SyncCards(typeGroup.Select(card => card as CardItem));
                        }
                        break;

                    case ItemType.Idea:
                        {
                            SyncIdeas(typeGroup.Select(idea => idea as IdeaItem), isNewRun);
                        }
                        break;

                    case ItemType.Misc:
                        {
                            SyncMiscs(typeGroup.Select(misc => misc as MiscItem));
                        }
                        break;

                    case ItemType.Stack:
                        {
                            SyncStacks(typeGroup.Select(stack => stack as StackItem));
                        }
                        break;

                    default:
                        {
                            StacklandsRandomizer.instance.ModLogger.LogWarning($"Item group '{typeGroup.Key}' was skipped due to it being an unhandled item type.");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Sync booster items with current progress.
        /// </summary>
        /// <param name="boosterItems">The booster items to be synced.</param>
        private static void SyncBoosters(IEnumerable<BoosterItem> boosterItems)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing {boosterItems.Count()} booster pack items...");

            // Group by booster type
            foreach (IGrouping<BoosterItem.BoosterType, BoosterItem> itemGroup in boosterItems.GroupBy(booster => booster.Type))
            {
                // Handle by booster type
                if (itemGroup.Key is BoosterItem.BoosterType.Spawn)
                {
                    // Group by item name
                    foreach (IGrouping<string, BoosterItem> spawnGroup in itemGroup.GroupBy(booster => booster.Name))
                    {
                        // Get group and session counts
                        int groupCount = spawnGroup.Count();
                        int sessionCount = KeyValueHelper.GetExtraKeyValue(spawnGroup.Key);

                        // Check if group count matches session count
                        if (groupCount <= sessionCount)
                        {
                            StacklandsRandomizer.instance.ModLogger.LogWarning($"Skipping booster item '{spawnGroup.Key}' because it has already been received {sessionCount} time(s).");
                            break;
                        }

                        StacklandsRandomizer.instance.ModLogger.Log($"Creating {groupCount - sessionCount} additional un-redeemed '{spawnGroup.Key}' booster pack items...");

                        // Invoke sync action for each un-received repeat item
                        for (int i = 0; i < groupCount - sessionCount; i++)
                        {
                            BoosterItem booster = spawnGroup.ElementAt(i);

                            // Spawn booster pack
                            SpawnBoosterPack(booster.ItemId);

                            // If first in list
                            if (i == 0)
                            {
                                // Mark as received and correct stored count
                                MarkAsReceived(booster, groupCount);
                            }
                        }
                    }
                }
                else if (itemGroup.Key is BoosterItem.BoosterType.Unlock)
                {
                    // Handle each booster
                    foreach (BoosterItem booster in itemGroup)
                    {
                        // If not yet discovered, unlock booster pack
                        if (!IsBoosterPackDiscovered(booster.ItemId))
                        {
                            UnlockBoosterPack(booster.ItemId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sync card items with current progress.
        /// </summary>
        /// <param name="cardItems">The card items to be synced.</param>
        private static void SyncCards(IEnumerable<CardItem> cardItems)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing {cardItems.Count()} card items...");

            foreach (IGrouping<string, CardItem> itemGroup in cardItems.GroupBy(card => card.Name))
            {
                // Get group and session counts
                int groupCount = itemGroup.Count();
                int sessionCount = KeyValueHelper.GetExtraKeyValue(itemGroup.Key);

                StacklandsRandomizer.instance.ModLogger.Log($"'{itemGroup.Key}' card item has been received {groupCount} times");
                StacklandsRandomizer.instance.ModLogger.Log($"'{itemGroup.Key}' card item has already been received {sessionCount} times");

                // If not forcing to create and item count does not breach session count
                if (groupCount <= sessionCount)
                {
                    StacklandsRandomizer.instance.ModLogger.LogWarning($"Skipping card item '{itemGroup.Key}' because it has already been received {sessionCount} time(s).");
                    break;
                }

                StacklandsRandomizer.instance.ModLogger.Log($"Creating {groupCount - sessionCount} additional un-redeemed '{itemGroup.Key}' card items...");

                // Get board for item(s) and get spawn position
                GameBoard board = WorldManager.instance.GetBoardWithId(cardItems.First().BoardId);
                Vector3 spawnPosition = GetRandomBoardPosition(board);

                for (int i = 0; i < groupCount - sessionCount; i++)
                {
                    // Get card item
                    CardItem card = itemGroup.ElementAt(i);

                    // Spawn card to board
                    SpawnCardToBoard(card.BoardId, card.ItemId, spawnPosition, true);

                    // If the first in the list
                    if (i == 0)
                    {
                        // Mark as received and correct stored count
                        MarkAsReceived(card, groupCount);
                    }
                }
            }
        }

        /// <summary>
        /// Sync idea items with current progress.
        /// </summary>
        /// <param name="ideaItems">The idea items to be synced.</param>
        /// <param name="isNewRun">Whether this sync is for a new run - if so, will force-spawn the idea cards.</param>
        private static void SyncIdeas(IEnumerable<IdeaItem> ideaItems, bool isNewRun = false)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing {ideaItems.Count()} idea items...");
            
            // Get a random spawn position
            Vector3 spawnPosition = WorldManager.instance.GetRandomSpawnPosition();

            // Spawn ideas
            foreach (IdeaItem idea in ideaItems)
            {
                // Spawn if forced to create or idea not yet discovered
                if (isNewRun || !IsIdeaDiscovered(idea.ItemId))
                {
                    SpawnCard(idea.ItemId, spawnPosition, true);
                }

            }
        }

        /// <summary>
        /// Sync misc items with current progress.
        /// </summary>
        /// <param name="miscItems">The misc items to be synced.</param>
        private static void SyncMiscs(IEnumerable<MiscItem> miscItems)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing {miscItems.Count()} misc items...");

            foreach (IGrouping<string, MiscItem> itemGroup in miscItems.GroupBy(misc => misc.Name))
            {
                // Get group and session counts
                int groupCount = itemGroup.Count();
                int sessionCount = KeyValueHelper.GetExtraKeyValue(itemGroup.First().Name);

                // If not forcing to create, check if item count matches session
                if (groupCount <= sessionCount)
                {
                    StacklandsRandomizer.instance.ModLogger.LogWarning($"Skipping misc item '{itemGroup.Key}' because it has already been received {sessionCount} time(s).");
                    break;
                }

                StacklandsRandomizer.instance.ModLogger.Log($"Triggering {groupCount - sessionCount} additional un-redeemed '{itemGroup.Key}' card items...");

                // Invoke sync action for each un-received repeat item
                for (int i = 0; i < groupCount - sessionCount; i++)
                {
                    // Get misc item and invoke sync action
                    MiscItem misc = itemGroup.ElementAt(i);
                    misc.SyncAction?.Invoke();

                    // If first in list
                    if (i == 0)
                    {
                        // Mark as received and correct stored count
                        MarkAsReceived(misc, groupCount);
                    }
                }
            }
        }

        /// <summary>
        /// Sync stack items with current progress.
        /// </summary>
        /// <param name="stackItems">The stack items to be synced.</param>
        /// <param name="forceCreate">(Optional) Bypass logic and force the stack items to be created.</param>
        private static void SyncStacks(IEnumerable<StackItem> stackItems, bool forceCreate = false)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"Syncing {stackItems.Count()} stack items...");

            foreach (IGrouping<string, StackItem> itemGroup in stackItems.GroupBy(misc => misc.Name))
            { 
                // Get group and session counts
                int groupCount = itemGroup.Count();
                int sessionCount = KeyValueHelper.GetExtraKeyValue(itemGroup.First().Name);

                // If not forcing to create, check if item count matches session
                if (!forceCreate && groupCount <= sessionCount)
                {
                    StacklandsRandomizer.instance.ModLogger.LogWarning($"Skipping stack item '{itemGroup.Key}' because it has already been received {sessionCount} time(s).");
                    break;
                }

                StacklandsRandomizer.instance.ModLogger.Log($"Creating {groupCount - sessionCount} additional un-redeemed '{itemGroup.Key}' card items...");

                // Invoke sync action for each un-received repeat item
                for (int i = 0; i < groupCount - sessionCount; i++)
                {
                    StackItem stack = itemGroup.ElementAt(i);

                    // Spawn idea if forced to create or if idea has not been discovered yet
                    SpawnStackToBoard(stack.BoardId, stack.ItemId, stack.Amount);

                    // Mark item as received and override receive count to ensure correct value
                    MarkAsReceived(stack, i);
                }
            }
        }

        /// <summary>
        /// Trigger the 'Feed Villagers' eating phase.
        /// NOTE: Will not be performed if player is currently in The Dark Forest, as it is not possible to bring food.
        /// </summary>
        public static void TriggerFeedVillagers()
        {
            // Check if not currently in dark forest
            if (WorldManager.instance.CurrentBoard.Id != Board.Forest)
            {
                // Queue the cutscene
                WorldManager.instance.QueueCutscene(CustomCutscenes.FeedVillagers());
            }
        }

        /// <summary>
        /// Trigger the 'Sell Cards' phase.
        /// </summary>
        public static void TriggerSellCards()
        {
            // Check if not currently in dark forest
            if (WorldManager.instance.CurrentBoard.Id != Board.Forest)
            {
                // Queue the cutscene
                WorldManager.instance.QueueCutscene(
                    CustomCutscenes.SellCards(
                        StacklandsRandomizer.instance.Options.SellCardTrapAmount));
            }
        }

        /// <summary>
        /// Unlock a booster pack if it has not already been found.
        /// </summary>
        /// <param name="boosterId">The booster pack to be unlocked.</param>
        public static void UnlockBoosterPack(string boosterId, bool forceUnlock = false)
        {
            try
            {
                StacklandsRandomizer.instance.ModLogger.Log($"Unlocking booster pack '{boosterId}'...");

                // Unlock booster if forcing unlock or has not already been unlocked
                if (forceUnlock || !IsBoosterPackDiscovered(boosterId))
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(boosterId);
                }
                else
                {
                    StacklandsRandomizer.instance.ModLogger.LogWarning($"Booster '{boosterId}' has already been discovered - skipping...");
                }
            }
            catch (Exception ex)
            {
                StacklandsRandomizer.instance.ModLogger.LogError($"Failed to unlock booster pack '{boosterId}'. Reason: '{ex.Message}'.");
            }
        }
    }
}
