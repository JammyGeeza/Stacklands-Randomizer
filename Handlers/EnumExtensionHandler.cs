using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class EnumExtensionHandler
    {
        /// <summary>
        /// Extended enum value for Mobsanity
        /// </summary>
        public static readonly QuestGroup MobsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Mobsanity");

        /// <summary>
        /// Extended enum value for Packsanity
        /// </summary>
        public static readonly QuestGroup PacksanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Packsanity");
    }
}
