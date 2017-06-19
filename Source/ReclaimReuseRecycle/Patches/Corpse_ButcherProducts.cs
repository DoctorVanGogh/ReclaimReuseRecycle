using System.Collections.Generic;
using Harmony;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(Corpse), nameof(Corpse.ButcherProducts))]
    // ReSharper disable once UnusedMember.Global
    public class Corpse_ButcherProducts {

        // ReSharper disable once UnusedMember.Global
        public static void Postfix(Corpse __instance, ref IEnumerable<Thing> __result, Pawn butcher, float efficiency) {
            List<Thing> result = new List<Thing>(__result);

            foreach (Hediff hediff in __instance.InnerPawn.health.hediffSet.hediffs) {
                if (hediff is Hediff_Implant || hediff is Hediff_AddedPart) {
                    Thing thing = ThingMaker.MakeThing(hediff.def.spawnThingOnRemoved);
                    // TODO: set thing hitpoints by part health/butcher skill/chance ??? in the end doesn't matter since reimplanted part health will always be at 100%
                    result.Add(thing);
                }                
            }
            
            __result = result;
        }
    }
}
