using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public class Item
    {
        /// <summary>
        /// The name of this item (matched to what is received from AP Server)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The type of Stacklands item that has been received.
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// A list of all IDs that this item will unlock.
        /// </summary>
        public List<string> ItemIds { get; set; } = new();

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