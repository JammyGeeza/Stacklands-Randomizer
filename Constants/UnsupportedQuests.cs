using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod.Constants
{
    public static class UnsupportedQuests
    {
        public static readonly string[] List = 
        [
            AllQuests.BuildARowboat.Id,                  // Build a Rowboat
            AllQuests.BuildACathedral.Id,                // Build a Cathedral
            AllQuests.BringIslandRelicToCathedral.Id,    // Bring the Island Relic to the Cathedral
            AllQuests.FindDeathWorld.Id,                 // Figure out how to summon the Spirit of Death
            AllQuests.FindGreedWorld.Id,                 // Figure out how to summon the Spirit of Greed
            AllQuests.FindHappinessWorld.Id,             // Figure out how to summon the Spirit of Happiness
            AllQuests.KillDemonLord.Id,                  // Kill the Demon Lord
            AllQuests.CreateAltar.Id,                    // Talk to the Shaman to gain secret knowledge
            AllQuests.CitiesGetBar.Id,                   // Smelt some Ore
        ];
    }
}
