using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public class RecipeWorker_Harvest : RecipeWorker {
        public RecipeDef[] HarvestingRecipes = new[] {DefReferences.Recipe_R3_HarvestCorpseFlesh, DefReferences.Recipe_R3_HarvestCorpseMechanoid};


        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map) {
            if (HarvestingRecipes.Contains(recipe) && ingredient is Corpse)
                return;

            base.ConsumeIngredient(ingredient, recipe, map);
        }
    }
}
