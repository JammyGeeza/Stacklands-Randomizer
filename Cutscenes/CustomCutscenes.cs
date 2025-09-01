using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    internal static class CustomCutscenes
    {
        /// <summary>
        /// Force the 'Feed Villagers' cutscene.
        /// </summary>
        public static IEnumerator FeedVillagers()
        {
            // Ignore if current location is forest
            if (WorldManager.instance.CurrentBoard.Location is Location.Forest)
                yield return 0;

            // Set title
            EndOfMonthCutscenes.CutsceneTitle = "Feed Villagers Trap";

            // Trigger feed villagers cutscene
            yield return EndOfMonthCutscenes.FeedVillagers();

            // If villagers didn't starve, continue
            if (!WorldManager.instance.VillagersStarvedAtEndOfMoon)
            {
                // Wait for 'okay' click
                yield return Cutscenes.WaitForContinueClicked(SokLoc.Translate("label_okay"));

                // Continue game
                GameCanvas.instance.SetScreen<GameScreen>();
                WorldManager.instance.SpeedUp = 1f;
            }

            // Reset game state
            GameCamera.instance.TargetPositionOverride = null;
            WorldManager.instance.currentAnimation = null;
            WorldManager.instance.currentAnimationRoutine = null;
        }

        /// <summary>
        /// Check if a card can be sold.
        /// </summary>
        /// <param name="card"></param>
        private static bool CardCanBeSold(GameCard card)
        {
            if (card.MyBoard.IsCurrent && !card.IsEquipped)
            {
                MonthlyRequirementResult monthlyRequirementResult = card.CardData.MonthlyRequirementResult;
                if ((monthlyRequirementResult == null || !(monthlyRequirementResult.results?.Count > 0)) && card.CardData.GetValue() != -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Force the 'Sell Cards' cutscene.
        /// </summary>
        /// <param name="amount">The amount of cards to force to be sold.</param>
        public static IEnumerator SellCards(int amount)
        {
            // Ignore if current location is forest
            if (WorldManager.instance.CurrentBoard.Location is Location.Forest)
                yield return 0;

            // Get starting card count
            int startingCount = WorldManager.instance.GetCardCount();

            // Get total sellable cards count
            int sellableCount = WorldManager.instance.GetAllCardsOnBoard(WorldManager.instance.CurrentBoard.Id)
                .Count(c => CardCanBeSold(c));

            // Calculate how many cards to sell
            int cardsToSellCount = amount < sellableCount
                ? amount
                : sellableCount;

            // Can't sell any, bail
            if (cardsToSellCount <= 0)
            {
                yield break;
            }

            // Calculate target count
            int targetCount = startingCount - cardsToSellCount;

            // Set cutscene text and wait for continue click
            EndOfMonthCutscenes.CutsceneTitle = "Sell Cards Trap";
            EndOfMonthCutscenes.CutsceneText = SokLoc.Translate("label_too_many_cards", LocParam.Plural("count", cardsToSellCount));
            yield return Cutscenes.WaitForContinueClicked(
                SokLoc.Translate("label_sell_x_cards", LocParam.Plural("count", cardsToSellCount)));

            // Loop until card sell limit has been satisfied
            WorldManager.instance.RemovingCards = true;
            while (WorldManager.instance.GetCardCount() > targetCount)
            {
                GameCamera.instance.TargetPositionOverride = null;

                // Get remaining to be sold
                int remaining = WorldManager.instance.GetCardCount() - targetCount;

                // Display remaining
                EndOfMonthCutscenes.CutsceneText = SokLoc.Translate("label_too_many_cards", LocParam.Plural("count", remaining));
                EndOfMonthCutscenes.CutsceneTitle = SokLoc.Translate("label_sell_cards_to_continue");
                yield return null;
            }

            EndOfMonthCutscenes.CutsceneText = SokLoc.Translate("label_too_many_cards", LocParam.Plural("count", 0));
            EndOfMonthCutscenes.CutsceneTitle = SokLoc.Translate("label_sell_cards_to_continue");

            // Wait a moment
            yield return new WaitForSeconds(1.5f);

            // Reset game state
            GameCanvas.instance.SetScreen<GameScreen>();
            WorldManager.instance.RemovingCards = false;
            GameCamera.instance.TargetPositionOverride = null;
            WorldManager.instance.currentAnimation = null;
            WorldManager.instance.currentAnimationRoutine = null;
        }
    }
}
