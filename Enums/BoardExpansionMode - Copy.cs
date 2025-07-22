using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    [Flags]
    public enum Boards
    {
        None        = 0,
        Mainland    = 1 << 0,
        Forest      = 1 << 1,
    }
}
