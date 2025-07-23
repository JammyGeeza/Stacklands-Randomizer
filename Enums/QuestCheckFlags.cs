using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    [Flags]
    public enum GoalFlags
    {
        None                    = 0,
        Kill_the_Demon          = 1 << 0,
        Kill_the_Wicked_Witch   = 1 << 1,
    }
}
