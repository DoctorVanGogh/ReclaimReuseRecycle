using System.Linq;
using DoctorVanGogh.ReclaimReuseRecycle;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public abstract class Filter_Corpse : SpecialThingFilterWorker {
        private bool _requireReclaimableParts;

        protected Filter_Corpse(bool requireReclaimableParts) {
            _requireReclaimableParts = requireReclaimableParts;            
        }

        public sealed override bool Matches(Thing t) {
            return DoesMatch(t as Corpse);
        }

        public override bool AlwaysMatches(ThingDef def) {
            return false;
        }

        public override bool CanEverMatch(ThingDef def) {
            return def.IsCorpse;
        }

        protected virtual bool DoesMatch(Corpse corpse) {
            if (corpse == null)
                return false;

            RaceProperties race = corpse.InnerPawn.RaceProps;
            Pawn_HealthTracker healthTracker = corpse.InnerPawn.health;
            HediffSet diffSet = healthTracker.hediffSet;

            return ((race.Humanlike || race.Animal) && AnyReclaimablePartsOrganic(race, diffSet, healthTracker) == _requireReclaimableParts)
                   || (race.IsMechanoid && AnyReclaimablePartsMechanoid(race, diffSet, healthTracker) == _requireReclaimableParts);
        }

        private static bool AnyReclaimablePartsMechanoid(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.GetNotMissingParts().Any(bpr => bpr.def.spawnThingOnRemoved != null
                                                           && null != ThingDefGenerator_Reclaimed.GetExtractableDef(bpr.def.spawnThingOnRemoved, Util.HitpointsFactor(bpr, diffSet))
                          );
        }

        private static bool AnyReclaimablePartsOrganic(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.hediffs.Any(
                              d => (d is Hediff_Implant || d is Hediff_AddedPart)
                                   && null != ThingDefGenerator_Reclaimed.GetExtractableDef(d.def.spawnThingOnRemoved, Util.HitpointsFactor(d.Part, diffSet)));
        }

    }


    public class Filter_Harvested : Filter_Corpse {
        public Filter_Harvested()  : base(false){            
        }
    }

    public class Filter_UnHarvested : Filter_Corpse {
        public Filter_UnHarvested() : base(true) {
            
        }
    }    
}
