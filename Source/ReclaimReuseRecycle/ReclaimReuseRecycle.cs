using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    // ReSharper disable once UnusedMember.Global
    public class ReclaimReuseRecycle : Mod {
       
        public ReclaimReuseRecycle(ModContentPack content) : base(content) {
            HarmonyInstance harmony = HarmonyInstance.Create("DoctorVanGogh.ReclaimReuseRecycle");
            harmony.PatchAll(Assembly.GetExecutingAssembly());           

            Log.Message("Initialized Reclaim Reuse Recycle...");
        }
    }
}
