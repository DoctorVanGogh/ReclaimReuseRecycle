using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Harmony;
using Verse;
using Verse.Noise;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(Corpse), nameof(Corpse.ButcherProducts))]
    // ReSharper disable once UnusedMember.Global
    public class Corpse_ButcherProducts {

        // remove any parts to-be-returned before actual butchering takes place, so 'missing parts %' get's correct material amount
        public static void Prefix(Corpse __instance, ref object __state, Pawn butcher, float efficiency) {
            RaceProperties race = __instance.InnerPawn.RaceProps;

            List<Thing> extras = new List<Thing>();

            // biologicals: check for any added parts/implants
            if (race.IsFlesh) {
                // can't change body markup while enumerating - store changes
                List<Hediff> modifications = new List<Hediff>();

                foreach (Hediff hediff in __instance.InnerPawn.health.hediffSet.hediffs) {
                    if (hediff is Hediff_Implant || hediff is Hediff_AddedPart) {
                        extras.Add(CreateThing(hediff.def.spawnThingOnRemoved, hediff.Part, __instance.InnerPawn.health.hediffSet));
                        modifications.Add(hediff);
                    }
                }

                // 'remove' any parts we'll return
                foreach (Hediff modification in modifications) {
                    __instance.InnerPawn.health.RemoveHediff(modification);
                    __instance.InnerPawn.health.hediffSet.AddDirect(
                                  HediffMaker.MakeHediff(
                                      RimWorld.HediffDefOf.MissingBodyPart,
                                      __instance.InnerPawn,
                                      modification.Part));
                }
            }

            // mechanoids: check for removable parts 
            if (race.IsMechanoid) {
                // can't change body markup while enumerating - store chan ges
                List<BodyPartRecord> partsToRemove = new List<BodyPartRecord>();

                foreach (BodyPartRecord part in __instance.InnerPawn.health.hediffSet.GetNotMissingParts()) {
                    ThingDef spawnOnRemove = part.def.spawnThingOnRemoved;

                    if (spawnOnRemove != null) {
                        extras.Add(CreateThing(spawnOnRemove, part, __instance.InnerPawn.health.hediffSet));
                        partsToRemove.Add(part);
                    }                                                          
                }


                // remove parts
                foreach (var partRecord in partsToRemove) {
                    __instance.InnerPawn.health.hediffSet.AddDirect(
                                  HediffMaker.MakeHediff(
                                      RimWorld.HediffDefOf.MissingBodyPart,
                                      __instance.InnerPawn,
                                      partRecord));
                }

            }
            __state = extras;
        }

        private static Thing CreateThing(ThingDef def, BodyPartRecord part, HediffSet injuries) {
            Thing thing = ThingMaker.MakeThing(def);

#if TRACE
            Log.Message($"Create Thing: {def.defName}; Max HP: {def.BaseMaxHitPoints}; Part: {part.def.defName} ({injuries.GetPartHealth(part)}/{part.def.GetMaxHealth(injuries.pawn)} HP)");
#endif
            thing.HitPoints = (int)Math.Round(thing.MaxHitPoints * HitpointsFactor(part, injuries));
            return thing;
        }

        private static float HitpointsFactor(BodyPartRecord part, HediffSet hediffSet) {
            return hediffSet.GetPartHealth(part)/part.def.GetMaxHealth(hediffSet.pawn);
        }


        // ReSharper disable once UnusedMember.Global
        public static void Postfix(Corpse __instance, object __state, ref IEnumerable<Thing> __result, Pawn butcher, float efficiency) {
            List<Thing> result = new List<Thing>(__result);

            List<Thing> extras = __state as List<Thing>;
            if (extras != null)
                result.AddRange(extras);

            __result = result;
        }

    }
}
