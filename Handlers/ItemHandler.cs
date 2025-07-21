using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections;
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

                case ItemType.Structure:
                    {
                        // Spawn the structure item
                        //SpawnStructureItem(mappedItem);

                        title = $"Structure Received!";
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
        /// Spawn a card to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="log">Whether or not to log receipt of this card.</param>
        public static void SpawnCard(string cardId, bool log = false)
        {
            try
            {
                // Create card (automatically marks as found)
                WorldManager.instance.CreateCard(
                    WorldManager.instance.GetRandomSpawnPosition(),
                    cardId,
                    true,
                    false,
                    true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to spawn card '{cardId}'. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Spawn a card to a specified board.
        /// </summary>
        /// <param name="boardId">The ID of the board to spawn to.</param>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="log">Whether or not to log receipt of this card.</param>
        public static void SpawnCardToBoard(string boardId, string cardId, bool log = false)
        {
            try
            {   
                // If the target board is the current board, spawn as normal
                if (WorldManager.instance.CurrentBoard.Id == boardId)
                {
                    SpawnCard(cardId, log);
                }

                // Otherwise, spawn to target board
                else if (WorldManager.instance.GetBoardWithId(boardId) is { } board)
                {
                    // Create the card out of view and play no sound
                    CardData cardData = WorldManager.instance.CreateCard(
                        Vector3.zero,
                        cardId,
                        true,
                        false,
                        false);

                    // Immediately send the card to the target board
                    WorldManager.instance.SendToBoard(cardData.MyGameCard, board, Vector2.zero);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to spawn card '{cardId}' to the board '{boardId}'. Reason: '{ex.Message}'");
            }
        }

        /// <summary>
        /// Spawn an idea to the current board.
        /// </summary>
        /// <param name="cardId"></param>
        public static void SpawnIdea(string cardId)
        {
            // Spawn if idea has not been found already
            if (!WorldManager.instance.HasFoundCard(cardId))
            {
                SpawnCard(cardId, false);
            }
        }

        /// <summary>
        /// Spawn a stack of resources to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the resource to spawn.</param>
        /// <param name="amount">The amount of the resource to spawn.</param>
        /// <param name="log">Whether or not to log receipt of this resource.</param>
        public static void SpawnResource(string cardId, int amount, bool log = true)
        {
            SpawnStack(cardId, amount, log);
        }

        /// <summary>
        /// Spawn a structure to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the structure to spawn.</param>
        /// <param name="log">Whether or not to log receipt of this structure.</param>
        public static void SpawnStructure(string cardId, bool log = false)
        {
            SpawnCard(cardId, log);
        }

        /// <summary>
        /// Spawn a card stack to the current board.
        /// </summary>
        /// <param name="cardId">The ID of the card to spawn.</param>
        /// <param name="amount">The amount to spawn in the stack.</param>
        /// <param name="log">Whether or not to log receipt of this card.</param>
        public static void SpawnStack(string cardId, int amount, bool log = false)
        {
            try
            {
                // Create stack
                WorldManager.instance.CreateCardStack(
                    WorldManager.instance.GetRandomSpawnPosition(),
                    amount,
                    cardId,
                    false);

                // Log receipt of item if required
                if (log)
                {
                    //LogFillerItem(cardId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to spawn card '{cardId}'. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Spawn a card stack to a specified board.
        /// </summary>
        /// <param name="boardId">The ID of the board to spawn to.</param>
        /// <param name="cardId">The ID of the card to be spawned.</param>
        /// <param name="amount">The amount of this card to spawn.</param>
        /// <param name="log">Whether or not to log receipt of this card.</param>
        public static void SpawnStackToBoard(string boardId, string cardId, int amount, bool log = false)
        {
            try
            {
                // If the target board is the current board, spawn as normal
                if (WorldManager.instance.CurrentBoard.Id == boardId)
                {
                    SpawnStack(cardId, amount, log);
                }

                // Otherwise, spawn to target board
                else if (WorldManager.instance.GetBoardWithId(boardId) is { } board)
                {
                    Debug.Log($"Found board: {board.Id}");

                    // Create stack
                    GameCard rootCard = WorldManager.instance.CreateCardStack(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        amount,
                        cardId,
                        false);

                    Debug.Log($"Attempting to send {rootCard.CardNameText.text} stack to board: {board.Id}");

                    // Immediately send the card to the target board
                    WorldManager.instance.SendStackToBoard(rootCard, board, Vector2.zero);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to spawn card '{cardId}' to the board '{boardId}'. Reason: '{ex.Message}'");
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
        /// 
        /// </summary>
        public static void TriggerFlipRandomCard()
        {
            // Get all cards on current board
            List<GameCard> cards = WorldManager.instance.GetAllCardsOnBoard(WorldManager.instance.CurrentBoard.Id);

            // Select one at random
            int index = UnityEngine.Random.Range(0, cards.Count - 1);

            // Flip the card over
            if (cards.ElementAt(index) is { } card)
            {
                //card.IsDemoCard = true;
                card.SetFaceUp(false);
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
        /// <param name="amount">The amount of cards that must be sold immediately.</param>
        public static void TriggerSellCards(int amount)
        {
            // Check if not currently in dark forest
            if (WorldManager.instance.CurrentBoard.Id != Board.Forest)
            {
                // Queue the cutscene
                WorldManager.instance.QueueCutscene(CustomCutscenes.SellCards(amount));
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

        /// <summary>
        /// Unlock a booster pack if it has not already been found.
        /// </summary>
        /// <param name="boosterId">The booster pack to be unlocked.</param>
        public static void UnlockBoosterPack(string boosterId)
        {
            try
            {   
                // Add booster if not already unlocked
                if (!WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(boosterId))
                {
                    WorldManager.instance.CurrentSave.FoundBoosterIds.Add(boosterId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to spawn Booster Pack item '{boosterId}'. Reason: '{ex.Message}'.");
            }
        }
    }
}
