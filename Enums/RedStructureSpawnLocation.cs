using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public enum RedStructureSpawnLocation : int
    {
        Vanilla     = 0, // Spawn as per vanilla
        TopLeft     = 1, // Spawn in the top-left corner
        TopRight    = 2, // Spawn in the top-right corner
        BottomLeft  = 3, // Spawn in the bottom-left corner
        BottomRight = 4, // Spawn in the bottom-right corner
        Middle      = 5, // Spawn in the middle of the board
    }
}
