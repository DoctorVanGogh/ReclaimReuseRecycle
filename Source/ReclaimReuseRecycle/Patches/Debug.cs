using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle.Patches
{
    /*
    [HarmonyPatch(typeof(ThingMaker), nameof(ThingMaker.MakeThing))]
    class Debug {
        public static void Prefix(ThingDef def, ThingDef stuff) {
            Log.Message($"Make {def?.defName} from {stuff?.defName}");
        }
    }
    */
}
