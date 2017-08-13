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
            foreach (var filter in new [] { DefReferences.SpecialThingFilter_R3_AllowUnharvested_Primitive, DefReferences.SpecialThingFilter_R3_AllowUnharvested_Advanced, DefReferences.SpecialThingFilter_R3_AllowUnharvested_Glittertech}) {
                DefReferences.Recipe_ButcherCorpseFlesh.defaultIngredientFilter.SetAllow(filter, false);
                DefReferences.Recipe_ButcherCorpseMechanoid.defaultIngredientFilter.SetAllow(filter, false);
            }

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
