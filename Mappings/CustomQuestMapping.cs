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

            new($"kill_a_{Cards.bear}")                 { DescriptionTermOverride = "Kill a Bear",                 OnSpecialAction = (string action) => action == $"{Cards.bear}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.dark_elf}")            { DescriptionTermOverride = "Kill a Dark Elf",             OnSpecialAction = (string action) => action == $"{Cards.dark_elf}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Forest_MainQuest,      Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.elf}")                 { DescriptionTermOverride = "Kill an Elf",                 OnSpecialAction = (string action) => action == $"{Cards.elf}_killed",                PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.elf_archer}")          { DescriptionTermOverride = "Kill an Elf Archer",          OnSpecialAction = (string action) => action == $"{Cards.elf_archer}_killed",         PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.enchanted_shroom}")    { DescriptionTermOverride = "Kill an Enchanted Shroom",    OnSpecialAction = (string action) => action == $"{Cards.enchanted_shroom}_killed",   PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.ent}")                 { DescriptionTermOverride = "Kill an Ent",                 OnSpecialAction = (string action) => action == $"{Cards.ent}_killed",                PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Forest_MainQuest,      Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.feral_cat}")            { DescriptionTermOverride = "Kill a Feral Cat",            OnSpecialAction = (string action) => action == $"{Cards.feral_cat}_killed",          PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.frog_man}")             { DescriptionTermOverride = "Kill a Frog Man",             OnSpecialAction = (string action) => action == $"{Cards.frog_man}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.ghost}")                { DescriptionTermOverride = "Kill a Ghost",                OnSpecialAction = (string action) => action == $"{Cards.ghost}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.giant_rat}")            { DescriptionTermOverride = "Kill a Giant Rat",            OnSpecialAction = (string action) => action == $"{Cards.giant_rat}_killed",          PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.giant_snail}")          { DescriptionTermOverride = "Kill a Giant Snail",          OnSpecialAction = (string action) => action == $"{Cards.giant_snail}_killed",        PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.goblin}")               { DescriptionTermOverride = "Kill a Goblin",               OnSpecialAction = (string action) => action == $"{Cards.goblin}_killed",             PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.goblin_archer}")        { DescriptionTermOverride = "Kill a Goblin Archer",        OnSpecialAction = (string action) => action == $"{Cards.goblin_archer}_killed",      PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.goblin_shaman}")        { DescriptionTermOverride = "Kill a Goblin Shaman",        OnSpecialAction = (string action) => action == $"{Cards.goblin_shaman}_killed",      PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.merman}")               { DescriptionTermOverride = "Kill a Merman",               OnSpecialAction = (string action) => action == $"{Cards.merman}_killed",             PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.mimic}")                { DescriptionTermOverride = "Kill a Mimic",                OnSpecialAction = (string action) => action == $"{Cards.mimic}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.mosquito}")             { DescriptionTermOverride = "Kill a Mosquito",             OnSpecialAction = (string action) => action == $"{Cards.mosquito}_killed",           PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.ogre}")                { DescriptionTermOverride = "Kill an Ogre",                OnSpecialAction = (string action) => action == $"{Cards.ogre}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Forest_MainQuest,      Type = CustomQuestType.Mobsanity },
            new($"kill_an_{Cards.orc_wizard}")          { DescriptionTermOverride = "Kill an Orc Wizard",          OnSpecialAction = (string action) => action == $"{Cards.orc_wizard}_killed",         PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.Forest_MainQuest,      Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.slime}")                { DescriptionTermOverride = "Kill a Slime",                OnSpecialAction = (string action) => action == $"{Cards.slime}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.small_slime}")          { DescriptionTermOverride = "Kill a Small Slime",          OnSpecialAction = (string action) => action == $"{Cards.small_slime}_killed",        PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.snake}")                { DescriptionTermOverride = "Kill a Snake",                OnSpecialAction = (string action) => action == $"{Cards.snake}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.tiger}")                { DescriptionTermOverride = "Kill a Tiger",                OnSpecialAction = (string action) => action == $"{Cards.tiger}_killed",              PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
            new($"kill_a_{Cards.wolf}")                 { DescriptionTermOverride = "Kill a Wolf",                 OnSpecialAction = (string action) => action == $"{Cards.wolf}_killed",               PossibleInPeacefulMode = false,      QuestGroup = QuestGroup.MainQuest,             Type = CustomQuestType.Mobsanity },
        };
    }
}
