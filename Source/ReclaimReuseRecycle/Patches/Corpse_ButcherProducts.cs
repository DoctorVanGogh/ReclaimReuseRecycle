using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Harmony;
using Verse;
using Verse.Noise;
using System.Linq;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(Corpse), nameof(Corpse.ButcherProducts))]
    // ReSharper disable once UnusedMember.Global
    public class Corpse_ButcherProducts {

        // remove any parts to-be-returned before actual butchering takes place, so 'missing parts %' get's correct material amount
        public static void Prefix(Corpse __instance, ref object __state, Pawn butcher, float efficiency) {
            ButcherState state = new ButcherState();

            RaceProperties race = __instance.InnerPawn.RaceProps;
            Pawn_HealthTracker healthTracker = __instance.InnerPawn.health;
            HediffSet diffSet = healthTracker.hediffSet;



            // biologicals: check for any added parts/implants
            if (race.IsFlesh) {
                foreach (Hediff hediff in diffSet.hediffs.Where(d => d is Hediff_Implant || d is Hediff_AddedPart)) {
                    state.ReturnPart(hediff, diffSet);
                }
            }

            // mechanoids: check for removable parts 
            if (race.IsMechanoid) {
                foreach (var t in diffSet.GetNotMissingParts().Select(bpr => new {Part = bpr, Spawn = bpr.def.spawnThingOnRemoved}).Where(t => t.Spawn != null)) {
                    state.ExtractPart(t.Spawn, diffSet, t.Part);
                }
            }

            LogHealth("R³: Before prefix change", healthTracker);

            // remove recorded parts (to get correct 'missing parts %' during actual butcher results calculation)
            foreach (BodyPartRecord partRecord in state.HeChanges.PartsToRemove)
                diffSet.AddDirect(
                    HediffMaker.MakeHediff(
                        RimWorld.HediffDefOf.MissingBodyPart,
                        __instance.InnerPawn,
                        partRecord));


            LogHealth("R³: After prefix change", healthTracker);

            __state = state;
        }


        // ReSharper disable once UnusedMember.Global
        public static void Postfix(Corpse __instance, object __state, ref IEnumerable<Thing> __result, Pawn butcher, float efficiency) {

            ButcherState state = __state as ButcherState;
            if (state != null) {
                List<Thing> result = new List<Thing>(__result);

                // generate extra results
                foreach (ThingProperties extraResult in state.ExtraResults) 
                    result.Add(Util.CreateThing(extraResult.Def, extraResult.HealthPercentage));

                #region roll back hediff changes
                Pawn_HealthTracker healthTracker = __instance.InnerPawn.health;
                HediffSet diffSet = healthTracker.hediffSet;

                var injectedPartRemovals = diffSet.hediffs.OfType<Hediff_MissingPart>()
                                                  .Join(
                                                      state.HeChanges.PartsToRemove.SelectMany(p => p.GetDescendants(true)),
                                                      d => d.Part,
                                                      p => p,
                                                      (d, p) => d);

                foreach (Hediff_MissingPart missingPart in injectedPartRemovals.ToArray()) {
                    healthTracker.RemoveHediff(missingPart);
                }

                foreach (var hiddenDiff in state.HeChanges.HiddenHeDiffs.OrderBy(d => d, new HeDiffComparer_AddedPartsThenImplants())) {
                    healthTracker.hediffSet.AddDirect(hiddenDiff);                    
                }

                LogHealth("R³: After postfix restore", healthTracker);
                #endregion

                __result = result;
            }
        }


        [Conditional("TRACE")]
        private static void LogHealth(string message, Pawn_HealthTracker healthTracker) {
            Util.LogMessage($"{message}:\r\n{Scribe.saver.DebugOutputFor(healthTracker)}");
        }
    }
}
