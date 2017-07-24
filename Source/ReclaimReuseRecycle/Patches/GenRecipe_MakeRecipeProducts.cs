using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    public class GenRecipe_MakeRecipeProducts {

        private static Thing[] Empty = new Thing[0];

        private static RecipeDef[] ReclamationRecipes = new[] {
                                                            DefReferences.Recipe_R3_Refurbish_Primitive,
                                                            DefReferences.Recipe_R3_Refurbish_Advanced,
                                                            DefReferences.Recipe_R3_Refurbish_Glittertech,
                                                            DefReferences.Recipe_R3_Sterilize_Primitive,
                                                            DefReferences.Recipe_R3_Sterilize_Advanced,
                                                            DefReferences.Recipe_R3_Sterilize_Glittertech
                                                        };

        public static void Postfix(ref IEnumerable<Thing> __result, RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient) {
            if (recipeDef == DefReferences.Recipe_R3_HarvestCorpseFlesh) {
                List<Thing> result = new List<Thing>(__result ?? Empty);

                var corpse = dominantIngredient as Corpse;

                if (corpse == null) {
                    Log.Warning("Harvesting without a corpse???");
                    return;
                }

                RaceProperties race = corpse.InnerPawn.RaceProps;
                Pawn_HealthTracker healthTracker = corpse.InnerPawn.health;
                HediffSet diffSet = healthTracker.hediffSet;

                foreach (Hediff hediff in diffSet.hediffs.Where(d => d is Hediff_Implant || d is Hediff_AddedPart).Where(d => d.def.spawnThingOnRemoved != null).ToArray()) {
                    Thing thing = HarvestUtility.TryExtractPart(worker, corpse, race, diffSet, hediff.Label, hediff.Part, hediff.def.spawnThingOnRemoved);
                    if (thing != null)
                        result.Add(thing);
                }

                __result = result;
            } else if (recipeDef == DefReferences.Recipe_R3_HarvestCorpseMechanoid) {
                List<Thing> result = new List<Thing>(__result ?? Empty);

                var corpse = dominantIngredient as Corpse;

                if (corpse == null) {
                    Log.Warning("Harvesting without a corpse???");
                    return;
                }

                RaceProperties race = corpse.InnerPawn.RaceProps;
                Pawn_HealthTracker healthTracker = corpse.InnerPawn.health;
                HediffSet diffSet = healthTracker.hediffSet;

                foreach (BodyPartRecord bpr in diffSet.GetNotMissingParts().Where(bpr => bpr.def.spawnThingOnRemoved != null)) {
                    Thing thing = HarvestUtility.TryExtractPart(worker, corpse, race, diffSet, bpr.def.LabelCap, bpr, bpr.def.spawnThingOnRemoved);
                    if (thing != null)
                        result.Add(thing);
                }

                __result = result;
            } else if (ReclamationRecipes.Contains(recipeDef)) {
                List<Thing> result = new List<Thing>(__result ?? Empty);

                PackedThing reclaimedThing = ingredients.OfType<PackedThing>().First();

                result.Add(ThingMaker.MakeThing(reclaimedThing.packedDef.SpawnOnUnpack));

                __result = result;
            }
        }
    }
}
