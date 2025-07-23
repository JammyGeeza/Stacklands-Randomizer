using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public enum BoardExpansionMode : int
    {
        Ideas = 0,  // Player builds 'Idea: Shed' and 'Idea: Warehouse' as normal.
        Items = 1,  // Uses 'Board Expansion' cards.
    }
}
