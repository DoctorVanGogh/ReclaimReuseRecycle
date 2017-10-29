using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    class Building_R3WorkTable : Building_WorkTable {
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats {
            get {
                foreach (var displayStat in base.SpecialDisplayStats) {
                    yield return displayStat;
                }


                yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.MedicalSurgerySuccessChance);
                yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.MechanoidOperationSuccessChance);
            }

        }

    }
}
