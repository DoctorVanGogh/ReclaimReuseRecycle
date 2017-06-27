using System.Collections.Generic;
using System.Linq;
using Harmony;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    public class GenRecipe_MakeRecipeProducts {

        private static Thing[] Empty = new Thing[0];

        public static void Prefix(ref object __state, RecipeDef recipeDef, ref Pawn worker, List<Thing> ingredients, Thing dominantIngredient) {
            if (recipeDef.specialProducts?.Any(p => p == SpecialProductType.Butchery) == true) {
                Corpse corpse = dominantIngredient as Corpse;
                __state = corpse;
            }
        }

        public static void Postfix(object __state, ref IEnumerable<Thing> __result, RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient) {
            var corpse = __state as Corpse;

            if (corpse != null) {
                List<Thing> result = new List<Thing>(__result ?? Empty);

                RaceProperties race = corpse.InnerPawn.RaceProps;
                Pawn_HealthTracker healthTracker = corpse.InnerPawn.health;
                HediffSet diffSet = healthTracker.hediffSet;

                if (race.IsFlesh) {
                    foreach (Hediff hediff in diffSet.hediffs.Where(d => d is Hediff_Implant || d is Hediff_AddedPart)) {
                        result.Add(Util.CreateThing(hediff.def.spawnThingOnRemoved, Util.HitpointsFactor(hediff.Part, diffSet)));                        
                    }
                }

                if (race.IsMechanoid) {
                    foreach (var t in diffSet.GetNotMissingParts().Select(bpr => new {
                        Part = bpr, Spawn = bpr.def.spawnThingOnRemoved
                    }).Where(t => t.Spawn != null)) {
                        result.Add(Util.CreateThing(t.Spawn, Util.HitpointsFactor(t.Part, diffSet)));                        
                    }
                }

                __result = result;
            }
        }
    }
}
