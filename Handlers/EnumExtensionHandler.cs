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
        /// Extended enum value for Equipmentsanity quest group
        /// </summary>
        public static readonly QuestGroup EquipmentsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Equipmentsanity");

        /// <summary>
        /// Extended enum value for Foodsanity quest group
        /// </summary>
        public static readonly QuestGroup FoodsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Foodsanity");

        /// <summary>
        /// Extended enum value for Mobsanity quest group
        /// </summary>
        public static readonly QuestGroup MobsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Mobsanity");

        /// <summary>
        /// Extended enum value for Locationsanity quest group
        /// </summary>
        public static readonly QuestGroup LocationsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Locationsanity");

        ///// <summary>
        ///// Extended enum value for Packsanity quest group
        ///// </summary>
        //public static readonly QuestGroup PacksanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Packsanity");

        /// <summary>
        /// Extended enum value for Spendsanity quest group
        /// </summary>
        public static readonly QuestGroup SpendsanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Spendsanity");

        /// <summary>
        /// Extended enum value for Structuresanity quest group
        /// </summary>
        public static readonly QuestGroup StructuresanityQuestGroupEnum = EnumHelper.ExtendEnum<QuestGroup>("Structuresanity");
    }
}
