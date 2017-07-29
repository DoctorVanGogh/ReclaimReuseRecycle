using System.Collections.Generic;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    class DefGenerator_GenerateImpliedDefs_PostResolve {

        [HarmonyPostfix]
        public static void Postfix() {
            // "patch" butcher creature recipe to disallow unharvested corpses by default
            DefReferences.Recipe_ButcherCorpseFlesh.defaultIngredientFilter.SetAllow(DefReferences.SpecialThingFilter_R3_AllowUnHarvested, false);
            DefReferences.Recipe_ButcherCorpseMechanoid.defaultIngredientFilter.SetAllow(DefReferences.SpecialThingFilter_R3_AllowUnHarvested, false);

            // "patch" market value stats to account for 'reclaimed' status
            DefReferences.Stat_MarketValue.parts.Add(new StatPart_Reclaimed());


            // setup lookup Cache
            ThingDefGenerator_Reclaimed.LookupCache = DefDatabase<ThingDef>.AllDefsListForReading
                                                                           .OfType<PackedThingDef>()
                                                                           .GroupBy(p => p.SpawnOnUnpack)
                                                                           .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
