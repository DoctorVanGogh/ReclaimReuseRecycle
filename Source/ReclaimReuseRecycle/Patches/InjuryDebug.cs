using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle.Patches {
    [HarmonyPatch(typeof(ThingWithComps), nameof(Thing.GetGizmos))]
    class InjuryDebug {

        static void Postfix(ThingWithComps __instance, ref IEnumerable<Gizmo> __result) {
            var c = __instance as Corpse;

            if (c == null || !DebugSettings.godMode)
                return;

            var l = __result.ToList();

            l.Add(new Command_Action {
                      defaultLabel = "Fail Ridic.",
                      defaultDesc = "Give ridiculous failure injuries",
                      action = () => {
                                   HarvestUtility.GiveInjuriesOperationFailureRidiculousUnrestricted(c.InnerPawn);
                               }
                  });

            l.Add(new Command_Action {
                      defaultLabel = "Fail Catas.",
                    defaultDesc = "Give catastropic failure injuries",
                action = () => {
                                   HarvestUtility.GiveInjuriesOperationFailureCatastrophicUnrestricted(c.InnerPawn,
                                                                                                       c.InnerPawn.health.hediffSet.GetNotMissingParts().RandomElementByWeight(bpr => bpr.coverageAbs));
                               }
                  });
            l.Add(new Command_Action {
                      defaultLabel = "Fail Minor",
                      defaultDesc = "Give minor failure injuries",
                      action = () => {
                                   HarvestUtility.GiveInjuriesOperationFailureMinorUnrestricted(c.InnerPawn,
                                                                                                c.InnerPawn.health.hediffSet.GetNotMissingParts().RandomElementByWeight(bpr => bpr.coverageAbs));
                               }
                  });

            __result = l;
        }
    }
}
