using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public class RecipeWorker_Harvest : RecipeWorker {
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map) {
            if ((HarvestFleshRecipes.Contains(recipe)  || HarvestMechanoidRecipes.Contains(recipe)) && ingredient is Corpse)
                return;

            base.ConsumeIngredient(ingredient, recipe, map);

        }

        public static RecipeDef[] HarvestFleshRecipes = new[] {
                                                             R3DefOf.R3_HarvestCorpseFlesh_Primitive,
                                                             R3DefOf.R3_HarvestCorpseFlesh_Advanced,
                                                             R3DefOf.R3_HarvestCorpseFlesh_Glittertech
                                                         };

        public static RecipeDef[] HarvestMechanoidRecipes = new[] {
                                                                 R3DefOf.R3_HarvestCorpseMechanoid_Primitive,
                                                                 R3DefOf.R3_HarvestCorpseMechanoid_Advanced,
                                                                 R3DefOf.R3_HarvestCorpseMechanoid_Glittertech
                                                             };
    }
}
