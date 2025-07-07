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
                .GroupBy(q => q is CustomQuest cq ? cq.CustomQuestGroup.ToString().ToLower() : q.QuestGroup.ToString().ToLower())
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

                    // Add quests to expandable section
                    foreach (Quest quest in group)
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
                "mobsanity" => 10,
                "other" => 11,
                _ => 12,
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
            Debug.Log($"{nameof(GameScreen)}.Awake Postfix!");

            // Is pause disabled?
            if (!StacklandsRandomizer.instance.IsPauseEnabled)
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

            Dictionary<object, bool> dictionary = WasExpandedDict(__instance.QuestsParent.GetComponentsInChildren<ExpandableLabel>());
            IEnumerable<Quest> source = [
                .. QuestManager.instance.AllQuests.Where(q =>
                    !UnsupportedQuests.List.Contains(q.Id) // Quest is not in the specified unsupported list
                    && ((q.QuestLocation is Location.Mainland && (q is not CustomQuest cq || (StacklandsRandomizer.instance.MobsanityEnabled && cq.CustomQuestGroup is CustomQuestGroup.Mobsanity))) // Quest is in Mainland (excluding Mobsanity quests if disabled)
                    || (q.QuestLocation is Location.Forest && StacklandsRandomizer.instance.DarkForestEnabled))) // or Quest is in The Dark Forest and Dark Forest is enabled
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
