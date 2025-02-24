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
    }
}