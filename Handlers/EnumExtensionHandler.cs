using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class EnumExtensionHandler
    {
        /// <summary>
        /// Extended enum value for Archipelago location
        /// </summary>
        public static readonly Location ArchipelagoLocationEnum = EnumHelper.ExtendEnum<Location>("Archipelago");

        /// <summary>
        /// Extended enum value for Mobsanity quest group
        /// </summary>
        public static readonly QuestGroup MobsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Mobsanity");

        /// <summary>
        /// Extended enum value for Packsanity quest group
        /// </summary>
        public static readonly QuestGroup PacksanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Packsanity");
    }
}
