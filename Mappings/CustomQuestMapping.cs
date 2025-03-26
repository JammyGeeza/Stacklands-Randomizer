using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class CustomQuestMapping
    {
        public static readonly List<CustomQuest> Map = new()
        {
            /* ----- Additional Quests ----- */
            
            // To be added in future...

            /* ----- Mobsanity Quests ----- */

            new($"kill_a_{Cards.bear}")                 { OnSpecialAction = (string action) => action == $"{Cards.bear}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.dark_elf}")             { OnSpecialAction = (string action) => action == $"{Cards.dark_elf}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.elf}")                 { OnSpecialAction = (string action) => action == $"{Cards.elf}_killed",                PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.elf_archer}")          { OnSpecialAction = (string action) => action == $"{Cards.elf_archer}_killed",         PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.enchanted_shroom}")    { OnSpecialAction = (string action) => action == $"{Cards.enchanted_shroom}_killed",   PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.ent}")                 { OnSpecialAction = (string action) => action == $"{Cards.ent}_killed",                PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.feral_cat}")            { OnSpecialAction = (string action) => action == $"{Cards.feral_cat}_killed",          PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.frog_man}")             { OnSpecialAction = (string action) => action == $"{Cards.frog_man}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.ghost}")                { OnSpecialAction = (string action) => action == $"{Cards.ghost}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.giant_rat}")            { OnSpecialAction = (string action) => action == $"{Cards.giant_rat}_killed",          PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.giant_snail}")          { OnSpecialAction = (string action) => action == $"{Cards.giant_snail}_killed",        PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.goblin}")               { OnSpecialAction = (string action) => action == $"{Cards.goblin}_killed",             PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.goblin_archer}")        { OnSpecialAction = (string action) => action == $"{Cards.goblin_archer}_killed",      PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.goblin_shaman}")        { OnSpecialAction = (string action) => action == $"{Cards.goblin_shaman}_killed",      PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.merman}")               { OnSpecialAction = (string action) => action == $"{Cards.merman}_killed",             PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.mimic}")                { OnSpecialAction = (string action) => action == $"{Cards.mimic}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.mosquito}")             { OnSpecialAction = (string action) => action == $"{Cards.mosquito}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.ogre}")                { OnSpecialAction = (string action) => action == $"{Cards.ogre}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_an_{Cards.orc_wizard}")          { OnSpecialAction = (string action) => action == $"{Cards.orc_wizard}_killed",         PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.slime}")                { OnSpecialAction = (string action) => action == $"{Cards.slime}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.small_slime}")          { OnSpecialAction = (string action) => action == $"{Cards.small_slime}_killed",        PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.snake}")                { OnSpecialAction = (string action) => action == $"{Cards.snake}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.tiger}")                { OnSpecialAction = (string action) => action == $"{Cards.tiger}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
            new($"kill_a_{Cards.wolf}")                 { OnSpecialAction = (string action) => action == $"{Cards.wolf}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Fighting,             CustomQuestGroup = CustomQuestGroup.Mobsanity },
        };
    }
}
