using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    public class TrapStrangePortal : StrangePortal
    {
        private float SpawnTimer;

        public override bool CanBeDragged => false;
        public override bool DetermineCanHaveCardsWhenIsRoot => false;

        protected override bool CanHaveCard(CardData otherCard)
        {
            return false;
        }

        public override bool CanHaveCardsWhileHasStatus()
        {
            return false;
        }

        public override void UpdateCard()
        {
            if (!TransitionScreen.InTransition && !WorldManager.instance.InAnimation)
            {
                if (!MyGameCard.TimerRunning)
                {
                    MyGameCard.StartTimer(SpawnTime, SpawnTrapCreature, SokLoc.Translate("new_portal_shaking"), GetActionId("SpawnTrapCreature"));
                    if (SpawnTimer > 0f)
                    {
                        MyGameCard.CurrentTimerTime = SpawnTimer;
                    }
                }
            }

            base.UpdateCard();
        }

        [TimedAction("spawn_trap_creature")]
        public void SpawnTrapCreature()
        {
            List<EnemySetCardBag> list = new List<EnemySetCardBag>();
            if (WorldManager.instance.CurrentMonth >= 24)
            {
                list.Add(EnemySetCardBag.BasicEnemy);
                list.Add(EnemySetCardBag.AdvancedEnemy);
                list.Add(EnemySetCardBag.Forest_BasicEnemy);
            }
            else if (WorldManager.instance.CurrentMonth >= 16)
            {
                list.Add(EnemySetCardBag.BasicEnemy);
                list.Add(EnemySetCardBag.AdvancedEnemy);
            }
            else
            {
                list.Add(EnemySetCardBag.BasicEnemy);
            }

            int value = Mathf.RoundToInt((float)Mathf.Max(12, WorldManager.instance.CurrentMonth) * 1.5f);
            value = Mathf.Clamp(value, 0, 70);

            foreach (CardIdWithEquipment item in SpawnHelper.GetEnemiesToSpawn(WorldManager.instance.GameDataLoader.GetSetCardBagForEnemyCardBagList(list), value))
            {
                Combatable obj = WorldManager.instance.CreateCard(base.transform.position, item, faceUp: false, checkAddToStack: false) as Combatable;
                obj.HealthPoints = obj.ProcessedCombatStats.MaxHealth;
                obj.MyGameCard.SendIt();

                // Prefix enemy with 'Trap'
                obj.nameOverride = $"Trap {obj.Name}";
            }

            MyGameCard.DestroyCard(spawnSmoke: true);
        }
    }
}
