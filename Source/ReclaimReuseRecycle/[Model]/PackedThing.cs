using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public class PackedThingDef : ThingDef {
        public ThingDef SpawnOnUnpack { get; set; }

        public ReclamationType? ReclamationType { get; set; }

        public Complexity Complexity { get; set;}
    }


    public class PackedThing : ThingWithComps {

        public ThingDef SpawnOnUnpack => packedDef?.SpawnOnUnpack;

        public PackedThingDef packedDef;

        public override void SpawnSetup(Map map, bool respawningAfterLoad) {
            base.SpawnSetup(map, respawningAfterLoad);
            packedDef = (def as PackedThingDef);
            if (packedDef == null)
                Util.Error($"{nameof(PackedThing)}: {nameof(packedDef)} is null - missing a class definition in the xml files?");
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats {
            get {

                yield return new StatDrawEntry(DefReferences.StatCategory_ReclaimedItem, LanguageKeys.r3.R3_OriginalThing.Translate(), packedDef.SpawnOnUnpack.LabelCap) {
                                 overrideReportText = packedDef.SpawnOnUnpack.description
                             };
                yield return new StatDrawEntry(DefReferences.StatCategory_ReclaimedItem, "Complexity", this.packedDef.Complexity.ToString());
            }
        }

    }
}
