using System.Collections.Generic;
using System.Linq;
using DoctorVanGogh.ReclaimReuseRecycle;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public abstract class Filter_Corpse : SpecialThingFilterWorker {
        private Complexity? _complexity;

        protected Filter_Corpse(Complexity? complexity) {
            _complexity = complexity;            
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


            if (_complexity == null)
                return (race.IsMechanoid && !GetReclaimablePartsMechanoid(race, diffSet, healthTracker).Any())
                       || ((race.Humanlike || race.Animal) && !GetReclaimablePartsOrganic(race, diffSet, healthTracker).Any());

            return (race.IsMechanoid && GetReclaimablePartsMechanoid(race, diffSet, healthTracker).Any(pd => pd.Complexity == _complexity))
                   || ((race.Humanlike || race.Animal) && GetReclaimablePartsOrganic(race, diffSet, healthTracker).Any(pd => pd.Complexity == _complexity));

        }

        private static IEnumerable<PackedThingDef> GetReclaimablePartsMechanoid(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.GetNotMissingParts()
                .Where(bpr => bpr.def.spawnThingOnRemoved != null)
                .Select(bpr => ThingDefGenerator_Reclaimed.GetExtractableDef(bpr.def.spawnThingOnRemoved, Util.HitpointsFactor(bpr, diffSet)))
                .Where(d => d != null);
        }


        private static IEnumerable<PackedThingDef> GetReclaimablePartsOrganic(RaceProperties race, HediffSet diffSet, Pawn_HealthTracker healthTracker) {
            return diffSet.hediffs
                          .Where(d => (d is Hediff_Implant || d is Hediff_AddedPart) && d.def.spawnThingOnRemoved != null)
                          .Select(d => ThingDefGenerator_Reclaimed.GetExtractableDef(d.def.spawnThingOnRemoved, Util.HitpointsFactor(d.Part, diffSet)))
                          .Where(d => d != null);
        }
        
    }


    public class Filter_Harvested : Filter_Corpse {
        public Filter_Harvested()  : base(null){            
        }

        #region HACKS for non corpse corpses - they always count as fully harvested.... "Thanks RBB!"
        public override bool CanEverMatch(ThingDef def) {
            return def.IsWithinCategory(ThingCategoryDefOf.Corpses);
        }

        protected override bool DoesMatch(Corpse corpse) {
            return corpse == null || base.DoesMatch(corpse);
        }
        #endregion
    }

    public class Filter_Unharvested_Primitive : Filter_Corpse {
        public Filter_Unharvested_Primitive() : base(Complexity.Primitive) {
            
        }
    }

    public class Filter_Unharvested_Advanced : Filter_Corpse {
        public Filter_Unharvested_Advanced() : base(Complexity.Advanced) {

        }
    }

    public class Filter_Unharvested_Glittertech : Filter_Corpse {
        public Filter_Unharvested_Glittertech() : base(Complexity.Glittertech) {

        }
    }

}
