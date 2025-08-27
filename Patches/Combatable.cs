using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Stacklands_Randomizer_Mod.Patches
{
    
    [HarmonyPatch(typeof(Combatable))]
    public class Combatable_Patches
    {
        /// <summary>
        /// Trigger the special action completed when a demon dies, as currently it does not. This prevents the location check from completing.
        /// </summary>
        [HarmonyPatch(nameof(Combatable.Damage))]
        [HarmonyPrefix]
        public static bool OnDamage_InterceptTrapCombatables(Combatable __instance, ref int damage)
        {
            // Intercept if combatable name starts with 'Trap'
            if (__instance.Name.StartsWith("Trap "))
            {
                // Is it already dead?
                bool alreadyDead = __instance.HealthPoints <= 0;

                __instance.HealthPoints -= damage;
                __instance.HealthPoints = Mathf.Max(__instance.HealthPoints, 0);
                __instance.StunTimer = 0.05f;

                GameCamera.instance.Screenshake = 0.3f;

                __instance.MyGameCard.SetHitEffect(delegate
                {
                    if (!alreadyDead && __instance.HealthPoints <= 0)
                    {
                        __instance.InAttack = false;

                        // Removed section here that triggers special action to prevent trap combatables completing checks

                        __instance.Die();
                    }
                });

                __instance.MyGameCard.RotWobble(0.5f);
                __instance.MyGameCard.transform.localScale *= 1.5f;

                // Prevent original method from triggering
                return false;
            }

            // Continue as normal
            return true;
        }
    }
}
