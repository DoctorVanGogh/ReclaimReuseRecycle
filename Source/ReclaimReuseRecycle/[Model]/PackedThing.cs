using System;
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

        public ThingDef SpawnOnUnpack => packedDef.SpawnOnUnpack;

        public PackedThingDef packedDef;

        public override void SpawnSetup(Map map, bool respawningAfterLoad) {
            base.SpawnSetup(map, respawningAfterLoad);
            packedDef = (def as PackedThingDef);
            if (packedDef == null)
                Util.Error($"{nameof(PackedThing)}: {nameof(packedDef)} is null - missing a class definition in the xml files?");
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats() {
            yield return new StatDrawEntry(
                R3DefOf.ReclaimedItem,
                LanguageKeys.r3.R3_OriginalThing.Translate(),
                SpawnOnUnpack.LabelCap,
                SpawnOnUnpack.description,
                0
            );
            yield return new StatDrawEntry(
                R3DefOf.ReclaimedItem, 
                LanguageKeys.r3.R3_Complexity.Translate(), 
                this.packedDef.Complexity.ToString(),
                null,
                0);           
        }

    }
}
