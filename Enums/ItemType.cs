using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public enum ItemType : int
    {
        BoosterPack = 0, // Booster Pack unlocks
        Idea = 1,        // Blueprints
        Resource = 2,    // Resource Cards
        Structure = 3,   // Structures
        Trap = 4,        // Traps
        Buff = 5,        // Buffs (Board Expansion, for example)

        Stack = 6,
        Misc = 7,
        Card = 8
    }
}
