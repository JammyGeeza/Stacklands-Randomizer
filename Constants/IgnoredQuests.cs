using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod.Constants
{
    public static class IgnoredQuests
    {
        /// <summary>
        /// List of Quest IDs to always ignore.
        /// </summary>
        public static readonly string[] AlwaysIgnore =
        [
            AllQuests.FindDeathWorld.Id,                 // Figure out how to summon the Spirit of Death
            AllQuests.FindGreedWorld.Id,                 // Figure out how to summon the Spirit of Greed
            AllQuests.FindHappinessWorld.Id,             // Figure out how to summon the Spirit of Happiness
            AllQuests.CreateAltar.Id,                    // Talk to the Shaman to gain secret knowledge
            AllQuests.CitiesGetBar.Id,                   // Smelt some Ore
        ];

        /// <summary>
        /// List of Quest IDs to ignore if The Island is disabled.
        /// </summary>
        public static readonly string[] IgnoreIfIslandDisabled = 
        [
            AllQuests.BuildARowboat.Id,
            AllQuests.BuildACathedral.Id,
            AllQuests.BringIslandRelicToCathedral.Id,
            AllQuests.KillDemonLord.Id,
            AllQuests.BreakBottle.Id,
            AllQuests.WearCrabScaleArmor.Id,
            AllQuests.CraftAmuletForest.Id
        ];
    }
}
