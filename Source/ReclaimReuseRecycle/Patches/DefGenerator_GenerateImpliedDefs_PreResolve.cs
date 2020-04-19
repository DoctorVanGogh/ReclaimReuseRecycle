using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
    class DefGenerator_GenerateImpliedDefs_PreResolve {

        public static void Prefix() {
            foreach (var def in ThingDefGenerator_Reclaimed.ImpliedReclaimableDefs()) {
                DefGenerator.AddImpliedDef(def);
            }
        }
    }
}
