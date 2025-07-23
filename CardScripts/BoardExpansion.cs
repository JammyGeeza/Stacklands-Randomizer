using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public class BoardExpansion : CardData
    {
        public override bool DetermineCanHaveCardsWhenIsRoot => false;

        protected override bool CanHaveCard(CardData otherCard)
        {
            return otherCard.Id == this.Id;
        }

        public override bool CanHaveCardsWhileHasStatus()
        {
            return false;
        }
    }
}
