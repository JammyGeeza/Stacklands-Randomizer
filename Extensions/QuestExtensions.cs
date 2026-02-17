using System;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod.Extensions
{
    public static class QuestExtensions
    {
        /// <summary>
        /// Get the english translated description of a quest.
        /// </summary>
        /// <param name="quest">The quest to retrieve the description for.</param>
        /// <returns>A string containing the english translated description.</returns>
        public static string GetEnglishDescription(this Quest quest)
        {
            LoadedLocSet englishLocSet = StacklandsRandomizer.instance.EnglishLocSet;

            if (!string.IsNullOrWhiteSpace(quest.DescriptionTermOverride))
            {
                if (quest.RequiredCount != -1)
                {
                    return englishLocSet.TranslateTerm(quest.DescriptionTermOverride, LocParam.Create("count", quest.RequiredCount.ToString()));
                }
                else
                {
                    return englishLocSet.TranslateTerm(quest.DescriptionTermOverride);
                }
            }
            else
            {
                return englishLocSet.TranslateTerm(quest.DescriptionTerm);
            }
        }
    }
}
