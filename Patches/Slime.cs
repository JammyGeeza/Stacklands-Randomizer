using HarmonyLib;
using Stacklands_Randomizer_Mod.Constants;
using System.Reflection.Emit;
using UnityEngine;

namespace Stacklands_Randomizer_Mod
{
    /// <summary>
    /// Patches for the <see cref="Slime"/> class.
    /// </summary>
    [HarmonyPatch(typeof(Slime))]
    public class Slime_Patches
    {
        /// <summary>
        /// Get the base 'Die' method from <see cref="Mob"/> parent class.
        /// </summary>
        private static readonly Action<Mob> CallBaseDie = CreateBaseCaller();

        /// <summary>
        /// When a Slime dies, if it's a Trap Slime, spawn trap small slimes instead.
        /// </summary>
        [HarmonyPatch("Die")]
        [HarmonyPrefix]
        public static bool OnDie_InterceptIfTrap(Slime __instance)
        {
            // Ignore if not prefixed with 'Trap'
            if (__instance.Name.StartsWith("Trap "))
            {
                // Spawn three 'Trap Small Slimes' instead
                for (int i = 0; i < 3; i++)
                {
                    WorldManager.instance.CreateCard(
                        __instance.transform.position,
                        ModCards.trap_small_slime,
                        faceUp: true,
                        checkAddToStack: false
                    ).MyGameCard.SendIt();
                }

                // Call base 'Die' method
                CallBaseDie(__instance);

                // Ignore original Slime.Die() 
                return false;
            }

            // Continue as normal
            return true;
        }

        /// <summary>
        /// Get the base method for
        /// </summary>
        /// <returns></returns>
        private static Action<Mob> CreateBaseCaller()
        {
            var baseMi = AccessTools.DeclaredMethod(typeof(Mob), "Die", Type.EmptyTypes);
            if (baseMi == null)
            {
                StacklandsRandomizer.instance.ModLogger.LogError("ERROR: Could not resolve Mob.Die() base method for Slime class.");
                return _ => { };
            }

            DynamicMethod dynamicMethod = new DynamicMethod(
                "__callbase_Mob_Die",
                typeof(void),
                new[] { typeof(Mob) },
                typeof(Slime),
                true);

            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, baseMi);
            il.Emit(OpCodes.Ret);

            return (Action<Mob>)dynamicMethod.CreateDelegate(typeof(Action<Mob>));
        }
    }
}
