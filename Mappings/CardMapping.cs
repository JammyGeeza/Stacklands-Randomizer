using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod.Mappings
{
    public static class CardMapping
    {
        public static readonly List<string> UnblockableCards = new()
        {
            Cards.key,
            Cards.treasure_chest,
        }
    }
}
