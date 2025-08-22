using HarmonyLib;
using Stacklands_Randomizer_Mod.Constants;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="GameScreen"/> class.
    /// </summary>
    [HarmonyPatch(typeof(GameScreen))]
    public class GameSceeen_Patches
    {
        #region Private Methods
        private static List<AchievementElement> CreateQuestElements(RectTransform parent, List<Quest> quests, bool addLabels = true)
        {
            List<AchievementElement> list = new List<AchievementElement>();
            foreach (Transform item in parent)
            {
                if (!item.name.StartsWith("DontDestroy"))
                {
                    UnityEngine.Object.Destroy(item.gameObject);
                }
            }

            // Order quests by completion and by group
            List<IGrouping<string, Quest>> questGroups = quests
                .OrderBy(q => !QuestManager.instance.QuestIsComplete(q))
                .GroupBy(q => EnumHelper.GetName<QuestGroup>((int)q.QuestGroup).ToLower())
                .OrderBy(g => GetQuestGroupOrder(g.Key))
                .ToList();

            foreach (IGrouping<string, Quest> group in questGroups)
            {
                if (addLabels)
                {
                    // Create expandable section
                    ExpandableLabel expandableLabel = UnityEngine.Object.Instantiate(PrefabManager.instance.AchievementElementLabelPrefab).GetComponent<ExpandableLabel>();
                    expandableLabel.transform.SetParentClean(parent);
                    expandableLabel.Tag = group.Key;

                    string achievementGroupName = $"{GetQuestGroupName(group.Key)} ({group.Count(q => QuestManager.instance.QuestIsComplete(q))}/{group.Count()})";
                    expandableLabel.SetText(achievementGroupName);
                    expandableLabel.SetExpanded(true);

                    // Add quests to expandable section (only include completed quests if 'Hide Quests' option is false
                    foreach (Quest quest in group.Where(q => !StacklandsRandomizer.instance.HideCompletedQuests || !QuestManager.instance.QuestIsComplete(q)))
                    {
                        AchievementElement achievementElement = UnityEngine.Object.Instantiate(PrefabManager.instance.AchievementElementPrefab);
                        achievementElement.SetQuest(quest);
                        expandableLabel.Children.Add(achievementElement.gameObject);
                        achievementElement.gameObject.SetActive(false);

                        achievementElement.transform.SetParentClean(parent);
                        list.Add(achievementElement);
                    }
                }
            }

            return list;
        }

        private static string GetQuestGroupName(string group)
        {
            return SokLoc.Translate($"questgroup_{group.ToLower()}");
        }

        private static int GetQuestGroupOrder(string group)
        {
            return group.ToLower() switch
            {
                "starter" => 0,
                "mainquest" => 1,
                "forest_mainquest" => 2,
                "fighting" => 3,
                "equipment" => 4,
                "cooking" => 5,
                "exploration" => 6,
                "resources" => 7,
                "building" => 8,
                "survival" => 9,
                "equipmentsanity" => 10,
                "foodsanity" => 11,
                "locationsanity" => 12,
                "mobsanity" => 10,
                //"packsanity" => 11,
                "spendsanity" => 11,
                "structuresanity" => 12,
                "other" => 13,
                _ => 13
            };
        }

        private static void SetFromWasExpandedDict(ExpandableLabel[] labels, Dictionary<object, bool> wasExpanded)
        {
            foreach (ExpandableLabel expandableLabel in labels)
            {
                if (wasExpanded.ContainsKey(expandableLabel.Tag))
                {
                    expandableLabel.SetExpanded(wasExpanded[expandableLabel.Tag]);
                }
            }
        }

        private static Dictionary<object, bool> WasExpandedDict(ExpandableLabel[] labels)
        {
            Dictionary<object, bool> dictionary = new Dictionary<object, bool>();
            foreach (ExpandableLabel expandableLabel in labels)
            {
                dictionary[expandableLabel.Tag] = expandableLabel.IsExpanded;
            }

            return dictionary;
        }

        #endregion

        /// <summary>
        /// Prevent automatic 'Quest Completed' notifications, as we will be sending our own.
        /// </summary>
        [HarmonyPatch(nameof(GameScreen.AddNotification))]
        [HarmonyPrefix]
        public static bool OnAddNotification_PreventQuestCompletedNotification(ref string title, ref string text)
        {
            // Disable if title contains quest completion
            return !title.Equals(SokLoc.Translate("label_quest_completed"));
        }

        /// <summary>
        /// Add an event handler to the game speed button click to override pause.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void OnAwake_OverridePause(GameScreen __instance)
        {
            StacklandsRandomizer.instance.ModLogger.Log($"{nameof(GameScreen)}.Awake Postfix!");

            // Is pause disabled?
            if (!StacklandsRandomizer.instance.Options.PauseTimeEnabled)
            {
                // Add an additional handler to game speed button click
                __instance.GameSpeedButton.Clicked += delegate
                {
                    // If speed is due to toggle to 0f next...
                    if (WorldManager.instance.SpeedUp == 5f)
                    {
                        // Set to 0f already so that it skips time pause
                        WorldManager.instance.SpeedUp = 0f;
                    }
                };
            }
        }

        /// <summary>
        /// Display all relevant quests to the run in the Quest Log.
        /// </summary>
        [HarmonyPatch(nameof(GameScreen.UpdateQuestLog))]
        [HarmonyPrefix]
        public static bool OnUpdateQuestLog_ShowAllRelevantQuests(GameScreen __instance, ref List<CustomButton> ___questButtons)
        {
            if (QuestManager.instance == null)
            {
                return false;
            }

            // TODO:    The 'UnsupportedQuests' implementation will fall over now that The Island is included.
            //          Some quests can be ignored entirely, put these into the 'Unsupported' list and then have a separate list
            //          that contains all 'Island' quests that show up under 'Mainland' and hide these when Island is not selected.

            Dictionary<object, bool> dictionary = WasExpandedDict(__instance.QuestsParent.GetComponentsInChildren<ExpandableLabel>());
            IEnumerable<Quest> source = [
                .. QuestManager.instance.AllQuests.Where(q =>
                    !UnsupportedQuests.List.Contains(q.Id)                                                                                                              // Quest is not in the specified unsupported list
                    && (q.QuestLocation is Location.Mainland && StacklandsRandomizer.instance.Options.QuestChecks.HasFlag(QuestCheckFlags.Mainland)                     // Quest location is Mainland and Mainland is enabled
                        || q.QuestLocation is Location.Forest && StacklandsRandomizer.instance.Options.QuestChecks.HasFlag(QuestCheckFlags.Forest)                      // Quest location is Forest and Forest is enabled
                        || q.QuestLocation is Location.Island && StacklandsRandomizer.instance.Options.QuestChecks.HasFlag(QuestCheckFlags.Island)                      // Quest location is Island and Island is enabled
                    )                     
                    && (StacklandsRandomizer.instance.Options.EquipmentsanityEnabled || q.QuestGroup != EnumExtensionHandler.EquipmentsanityQuestGroupEnum)             // Equipmentsanity is enabled OR quest group is not Equipmentsanity
                    && (StacklandsRandomizer.instance.Options.FoodsanityEnabled || q.QuestGroup != EnumExtensionHandler.FoodsanityQuestGroupEnum)                       // Foodsanity is enabled OR quest group is not Foodsanity
                    && (StacklandsRandomizer.instance.Options.LocationsanityEnabled || q.QuestGroup != EnumExtensionHandler.LocationsanityQuestGroupEnum)               // Locationsanity is enabled OR quest group is not Locationsanity
                    && (StacklandsRandomizer.instance.Options.MobsanityEnabled || q.QuestGroup != EnumExtensionHandler.MobsanityQuestGroupEnum)                         // Mobsanity is enabled OR quest group is not Mobsanity
                    //&& (StacklandsRandomizer.instance.Options.PacksanityEnabled || q.QuestGroup != EnumExtensionHandler.PacksanityQuestGroupEnum)                     // Packsanity is enabled OR quest group is not Packsanity
                    && (StacklandsRandomizer.instance.Options.Spendsanity is not Spendsanity.Off || q.QuestGroup != EnumExtensionHandler.SpendsanityQuestGroupEnum)     // Spendsanity is enabled OR quest group is not Spendsanity   
                    && (StacklandsRandomizer.instance.Options.StructuresanityEnabled || q.QuestGroup != EnumExtensionHandler.StructuresanityQuestGroupEnum))            // Structuresanity is enabled OR quest group is not Structuresanity
            ];

            __instance.questElements = CreateQuestElements(__instance.QuestsParent, source.ToList());
            ___questButtons = (from x in __instance.QuestsParent.GetComponentsInChildren<CustomButton>()
                            where x.enabled
                            select x).ToList();

            // Need to do this as we can't use the ref inside the anonymous function (delegate)
            List<CustomButton> questButtons = ___questButtons;

            for (int i = 0; i < ___questButtons.Count - 1; i++)
            {
                ___questButtons[i].ExplicitNavigationChanged += delegate (CustomButton cb, Navigation nav)
                {
                    int num = questButtons.IndexOf(cb);
                    nav.selectOnUp = ((num == 0) ? __instance.QuestsButton : questButtons[num - 1]);
                    nav.selectOnDown = questButtons[num + 1];
                    return nav;
                };
            }

            ExpandableLabel[] componentsInChildren = __instance.QuestsParent.GetComponentsInChildren<ExpandableLabel>();
            foreach (AchievementElement questElement in __instance.questElements)
            {
                if (questElement.IsNew)
                {
                    dictionary[questElement.MyQuest.QuestGroup] = true;
                }
            }

            SetFromWasExpandedDict(componentsInChildren, dictionary);

            // Prevent the original method from doing anything
            return false;
        }
    }
}
