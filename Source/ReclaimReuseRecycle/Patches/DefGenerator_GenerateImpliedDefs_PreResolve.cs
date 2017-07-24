using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
    class DefGenerator_GenerateImpliedDefs_PreResolve {

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr) {
            var instructions = new List<CodeInstruction>(instr);

            var idx = instructions.FirstIndexOf(ci => ci.opcode == OpCodes.Stloc_0);
            if (idx == -1) {
                Util.Error("Cannot find implied def storage - not injecting our generated defs.");
                return instructions;
            }

            instructions.Insert(idx,new CodeInstruction(instructions[idx -1]));
            instructions.Insert(idx, new CodeInstruction(OpCodes.Call, typeof(ThingDefGenerator_Reclaimed).GetMethod(nameof(ThingDefGenerator_Reclaimed.ImpliedReclaimableDefs))));

            /*
             * Change:
             * 
             * IEnumerable<ThingDef> enumerable = ....Concat(...);
             * 
             * => 
             * 
             * IEnumerable<ThingDef> enumerable = ....Concat(...).Concat(ThingDefGenerator_Reclaimed.ImpliedReclaimableDefs());
             * 
             */

            return instructions;
        }
    }
}
