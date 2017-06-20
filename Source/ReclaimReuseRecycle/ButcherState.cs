using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    /// <summary>
    /// Utility class to preserve state data between Harmony <see cref="Corpse.ButcherProducts"/> Pre &amp; Postfix invocations
    /// </summary>
    class ButcherState {
        public ICollection<ThingProperties> ExtraResults { get; private set; }

        public HeDiffTracker HeChanges { get; private set; }

        public ButcherState() {
            ExtraResults = new List<ThingProperties>();
            HeChanges = new HeDiffTracker();
        }

        public void ReturnPart(Hediff diff, HediffSet set) {
            ExtraResults.Add(new ThingProperties(diff.def.spawnThingOnRemoved, Util.HitpointsFactor(diff.Part, set)));
            HeChanges.RemovePart(diff.Part, set);
        }

        public void ExtractPart(ThingDef extracted, HediffSet set, BodyPartRecord part) {
            ExtraResults.Add(new ThingProperties(extracted, Util.HitpointsFactor(part, set)));
            HeChanges.RemovePart(part, set);
        }
    }

    class ThingProperties {
        public ThingProperties(ThingDef def, float healthPercentage) {
            Def = def;
            HealthPercentage = healthPercentage;
        }

        public ThingDef Def { get; private set; }
        public float HealthPercentage { get; private set; }
    }


    public class HeDiffTracker {
        public ICollection<BodyPartRecord> PartsToRemove { get; private set; }
        public ICollection<Hediff> HiddenHeDiffs { get; private set; }

        public HeDiffTracker() {
            PartsToRemove = new List<BodyPartRecord>();
            HiddenHeDiffs = new List<Hediff>();           
        }

        public void RemovePart(BodyPartRecord part, HediffSet set) {
            // removing a part will kill all injuries/hediffs on this part or descendants
            IEnumerable<BodyPartRecord> partAndDescendants = part.GetDescendants(true);

            foreach (var hediff in set.hediffs.Join(partAndDescendants, d => d.Part, p => p, (d, p) => d).Where(d => !(d is Hediff_MissingPart))) {
                HiddenHeDiffs.Add(hediff);     
            }

            PartsToRemove.Add(part);
        }

    }
}
