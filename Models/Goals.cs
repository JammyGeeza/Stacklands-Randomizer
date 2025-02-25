using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public class Goal
    {
        /// <summary>
        /// The full name of the goal.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Quest ID for the goal.
        /// </summary>
        public string QuestId { get; set; }

        /// <summary>
        /// The type of goal - corresponds to the goals set in the options.
        /// </summary>
        public GoalType Type { get; set; }
    }
}
