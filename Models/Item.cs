using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public class Item
    {
        /// <summary>
        /// Gets or sets the ID of the item that this item will unlock.
        /// </summary>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of Stacklands item that has been received.
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// Gets or sets the name of this item (matched to what is received from AP Server)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets how many of this item should be received.
        /// </summary>
        //public int Amount { get; set; } = 1;

        

        /// <summary>
        /// Constructor for Item class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="itemid">The ID of the card item.</param>
        /// <param name="itemType">The type of item.</param>
        public Item(string name, string itemid, ItemType itemType)
        {
            Name = name;
            ItemId = itemid;
            ItemType = itemType;
        }

        /// <summary>
        /// Check if an <see cref="ItemInfo"/> matches this item.
        /// </summary>
        /// <param name="item">The <see cref="ItemInfo"/> to be compared.</param>
        /// <returns><see cref="true"/> if match, <see cref="false"/> if not.</returns>
        public bool Matches(ItemInfo item)
        {
            return this.Matches(item.ItemName);
        }

        /// <summary>
        /// Check if an <see cref="Item"/> matches this item.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to be compared.</param>
        /// <returns><see cref="true"/> if match, <see cref="false"/> if not.</returns>
        public bool Matches(Item item)
        {
            return this.Matches(item.Name);
        }

        /// <summary>
        /// Check if an item name matches this item.
        /// </summary>
        /// <param name="item">The name to be compared.</param>
        /// <returns><see cref="true"/> if match, <see cref="false"/> if not.</returns>
        public bool Matches(string item)
        {
            return this.Name.Equals(item, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class BoosterItem : Item
    {
        public enum BoosterType
        {
            Spawn,
            Unlock,
        }

        /// <summary>
        /// Gets or sets the type of booster.
        /// </summary>
        public BoosterType Type { get; set; }

        public BoosterItem(string name, string boosterId, BoosterType boosterType) : base(name, boosterId, ItemType.BoosterPack)
        {
            Type = boosterType;
        }
    }

    public class CardItem : Item
    {
        /// <summary>
        /// The ID of the board to spawn this item to. Will default to current board if left blank.
        /// </summary>
        public string BoardId { get; private set; }

        public CardItem(string name, string cardId, string boardId) : base(name, cardId, ItemType.Card)
        {
            BoardId = boardId;
        }
    }

    public class IdeaItem : Item
    {
        public IdeaItem(string name, string ideaId) : base(name, ideaId, ItemType.Idea)
        {

        }
    }

    public class MiscItem : Item
    {
        /// <summary>
        /// The action to perform when this item is received.
        /// </summary>
        public Action ReceivedAction { get; set; }

        /// <summary>
        /// The action to perform when this item is synced.
        /// </summary>
        public Action<bool>? SyncAction { get; set; }

        public MiscItem(string name, string itemId, Action receivedAction, Action<bool>? syncAction = null) : base(name, itemId, ItemType.Misc)
        {
            ReceivedAction = receivedAction;
            SyncAction = syncAction;
        }
    }

    public class StackItem : Item
    {
        /// <summary>
        /// The amount of this item to include in the stack.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The ID of the board to create the item on.
        /// </summary>
        public string BoardId { get; set; } = string.Empty;

        public StackItem(string name, string cardId, int amount, string board) : base(name, cardId, ItemType.Stack)
        {
            Amount = amount;
            BoardId = board;

            //ReceivedAction = () => ItemHandler.HandleStack(this);
            //SyncAction = (bool forceCreate, Vector3? position) => ItemHandler.HandleStack(this, position);
        }
    }
}