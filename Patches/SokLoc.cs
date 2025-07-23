using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod.Patches
{
    
    [HarmonyPatch(typeof(SokLoc))]
    public class SokLoc_Patches
    {
        /// <summary>
        /// Trigger the special action completed when a demon dies, as currently it does not. This prevents the location check from completing.
        /// </summary>
        [HarmonyPatch("LoadTermsFromFile")]
        [HarmonyPostfix]
        public static void OnReadyUpMods_TriggerSpecialAction(SokLoc __instance, ref string path, ref bool disableWarning)
        {
            string[][] array = SokLoc.ParseTableFromTsv(File.ReadAllText(path));
            int languageColumnIndex = SokLoc.GetLanguageColumnIndex(array, "English");
            if (languageColumnIndex == -1)
            {
                return;
            }

            for (int i = 1; i < array.Length; i++)
            {
                string term = array[i][0];
                string fullText = array[i][languageColumnIndex];
                term = term.Trim().ToLower();
                if (string.IsNullOrEmpty(term))
                {
                    continue;
                }

                SokTerm sokTerm = new SokTerm(StacklandsRandomizer.instance.EnglishLocSet, term, fullText);
                if (StacklandsRandomizer.instance.EnglishLocSet.TermLookup.ContainsKey(term))
                {
                    if (!disableWarning)
                    {
                        StacklandsRandomizer.instance.ModLogger.LogError("Term " + term + " has been found more than once in the localisation sheet. Using last item in sheet.");
                    }

                    StacklandsRandomizer.instance.EnglishLocSet.TermLookup[term] = sokTerm;
                    StacklandsRandomizer.instance.EnglishLocSet.AllTerms.RemoveAll((SokTerm x) => x.Id == term);
                    StacklandsRandomizer.instance.EnglishLocSet.AllTerms.Add(sokTerm);
                }
                else
                {
                    StacklandsRandomizer.instance.EnglishLocSet.AllTerms.Add(sokTerm);
                    StacklandsRandomizer.instance.EnglishLocSet.TermLookup.Add(term, sokTerm);
                }
            }
        }
    }
}
