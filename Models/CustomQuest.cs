using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public class CustomQuest(string id) : Quest(id)
    {
        /// <summary>
        /// The type of custom quest.
        /// </summary>
        public CustomQuestGroup CustomQuestGroup { get; set; } = CustomQuestGroup.Archipelago;
    }
}
