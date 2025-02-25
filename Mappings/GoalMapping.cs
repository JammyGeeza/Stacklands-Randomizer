using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class GoalMapping
    {
        public static readonly List<Goal> Map = new()
        {
            // Mainland Quests
            new() { Name = "Kill the Demon", QuestId = AllQuests.KillDemon.Id, Type = GoalType.KillDemon },
            new() { Name = "Kill the Demon Lord", QuestId = AllQuests.KillDemonLord.Id, Type = GoalType.KillDemonLord },
        };
    }
}
