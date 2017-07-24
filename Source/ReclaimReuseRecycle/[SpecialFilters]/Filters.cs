using System.Linq;
using DoctorVanGogh.ReclaimReuseRecycle;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public abstract class Filter_Corpse : SpecialThingFilterWorker {
        public sealed override bool Matches(Thing t) {
            return DoesMatch(t as Corpse);
        }

        protected abstract bool DoesMatch(Corpse corpse);

        public override bool AlwaysMatches(ThingDef def) {
            return false;
        }

        public override bool CanEverMatch(ThingDef def) {
            return def.IsCorpse;
        }
    }

    public abstract class Filter_Racial : Filter_Corpse {
        protected sealed override bool DoesMatch(Corpse corpse) {
            if (corpse == null)
                return false;

            RaceProperties race = corpse.InnerPawn.RaceProps;
            Pawn_HealthTracker healthTracker = corpse.InnerPawn.health;
            HediffSet diffSet = healthTracker.hediffSet;

            return AllowByRace(race) && AnyReclaimableParts(race, diffSet, healthTracker) == RequireReclaimableParts;
        }

        protected abstract bool AnyReclaimableParts(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker);
        protected abstract bool RequireReclaimableParts { get; }

        protected abstract bool AllowByRace(RaceProperties race);
    }

    public abstract class Filter_Biological : Filter_Racial {
        protected override bool AllowByRace(RaceProperties race) {
            return race.IsFlesh;
        }

        protected sealed override bool AnyReclaimableParts(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.hediffs.Any(
                              d => (d is Hediff_Implant || d is Hediff_AddedPart)
                                   && null != ThingDefGenerator_Reclaimed.GetExtractableDef(d.def.spawnThingOnRemoved, Util.HitpointsFactor(d.Part, diffSet)));
        }
    }

    public abstract class Filter_UnHarvestedBiological : Filter_Biological {
        protected sealed override bool RequireReclaimableParts => true;
    }
    public class Filter_UnHarvestedBiologicalHumanlike : Filter_UnHarvestedBiological {
        protected override bool AllowByRace(RaceProperties race) {
            return base.AllowByRace(race) && race.Humanlike;
        }
    }
    public class Filter_UnHarvestedBiologicalAnimal : Filter_UnHarvestedBiological {
        protected override bool AllowByRace(RaceProperties race) {
            return base.AllowByRace(race) && race.Animal;
        }
    }

    public abstract class Filter_HarvestedBiological : Filter_Biological {
        protected override bool RequireReclaimableParts => false;
    }
    public class Filter_HarvestedBiologicalHumanlike : Filter_HarvestedBiological {
        protected override bool AllowByRace(RaceProperties race) {
            return base.AllowByRace(race) && race.Humanlike;
        }
    }
    public class Filter_HarvestedBiologicalAnimal : Filter_HarvestedBiological {
        protected override bool AllowByRace(RaceProperties race) {
            return base.AllowByRace(race) && race.Humanlike;
        }
    }

    public abstract class Filter_Mechanoid : Filter_Racial {
        protected sealed override bool AllowByRace(RaceProperties race) {
            return race.IsMechanoid;
        }

        protected sealed override bool AnyReclaimableParts(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.GetNotMissingParts().Any(bpr => bpr.def.spawnThingOnRemoved != null
                                                           && null != ThingDefGenerator_Reclaimed.GetExtractableDef(bpr.def.spawnThingOnRemoved, Util.HitpointsFactor(bpr, diffSet))
                          );
        }
    }

    public class Filter_UnHarvestedMechanoid : Filter_Mechanoid {
        protected override bool RequireReclaimableParts => true;
    }

    public class Filter_HarvestedMechanoid : Filter_Mechanoid {
        protected override bool RequireReclaimableParts => false;
    }
}
