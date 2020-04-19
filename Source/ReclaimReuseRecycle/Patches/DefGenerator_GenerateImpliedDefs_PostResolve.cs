using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    class DefGenerator_GenerateImpliedDefs_PostResolve {

        [HarmonyPostfix]
        public static void Postfix() {
            // "patch" butcher creature recipe to disallow unharvested corpses by default
            foreach (var filter in new [] { R3DefOf.R3_AllowUnharvested_Primitive, R3DefOf.R3_AllowUnharvested_Advanced, R3DefOf.R3_AllowUnharvested_Glittertech }) {
                R3DefOf.ButcherCorpseFlesh.defaultIngredientFilter.SetAllow(filter, false);
                R3DefOf.ButcherCorpseMechanoid.defaultIngredientFilter.SetAllow(filter, false);
            }

            // "patch" market value stats to account for 'reclaimed' status
            StatDefOf.MarketValue.parts.Add(new StatPart_Reclaimed());


            // setup lookup Cache
            var customElements = DefDatabase<ThingDef>.AllDefsListForReading
                                                      .OfType<PackedThingDef>()
                                                      .ToArray();

            ThingDefGenerator_Reclaimed.LookupCache = customElements.GroupBy(p => p.SpawnOnUnpack)
                                                                    .ToDictionary(g => g.Key, g => g.ToArray());
            
            // work around for sound issues
            foreach (var packedThingDef in customElements) {
                packedThingDef.ResolveReferences();
            }
        }
    }
}
