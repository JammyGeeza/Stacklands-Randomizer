using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public class Item
    {
        /// <summary>
        /// Gets or sets the name of this item (matched to what is received from AP Server)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of Stacklands item that has been received.
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// Gets or sets the ID of the item that this item will unlock.
        /// </summary>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets how many of this item should be received.
        /// </summary>
        public int Amount { get; set; } = 1;

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
}