using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(Corpse))]
    [HarmonyPatch(nameof(Corpse.SpecialDisplayStats))]
    class Corpse_SpecialDisplayStats {
        public static void Postfix(Corpse __instance, ref IEnumerable<StatDrawEntry> __result) {
            var complexities = HarvestUtility.GetExtractableThings(__instance).Select(pd => pd.Complexity).ToArray();

            if (complexities.Length == 0)
                return;

            var maxComplexity = complexities.Max();

            __result = __result.Concat(new[] {
                                                 new StatDrawEntry(
                                                     R3DefOf.ReclaimedItem,
                                                     LanguageKeys.r3.R3_MaxComplexity.Translate(),
                                                     maxComplexity.ToString(),
                                                     null,
                                                     0)
                                             });
        }
    }
}
