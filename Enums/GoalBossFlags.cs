using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    [Flags]
    public enum GoalBossFlags
    {
        None            = 0,
        KillDemon       = 1 << 0,
        KillWickedWitch = 1 << 1,
        KillDemonLord   = 1 << 2,
    }
}
