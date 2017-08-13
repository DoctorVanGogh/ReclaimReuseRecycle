# Features for Alpha release

## Code/Defs

### Injection

[x] Think about creating [multiple(?)] `ThingDefGenerator_[Stuff]` and injecting into `DefGenerator.GenerateImpliedDefs_PreResolve`

### Harvesting

[x] RecipeDefs with fitting `workerClass`  (Base `RecipeWorker`) - Override `ConsumeIngredient` to not destroy corpse
[x] `GenRecipe.MakeRecipeProducts` patch
    [x] results modification
    [x] corpse hediffset modification
    [x] failure modes (no result, part(s) lost, corpse destroyed + butcher human mood debuff?)
        [x] damage on failure
[x] Implement hp scale mapping x%-y% health of part lead to 50%-100% of reclaimed item
    [x] Settings for thresholds
    [x] `HarvestUtility.TryExtractPart`
    [x] `R3Mod.GetDef`
    [x] `GenMAth.LerpDouble` !!

### part categorization

[x] 'Complexity' for reclaimed parts (Low/Average/High)
    [x] Mapping: techlevel -> research project.techlevel -> price?
    [-] define `<StatDef>` for complexity
      [-] 'parts': by techlevel, by research, by price
    [-] StatWorker
      [-] custom `StatWorker.GetStatDrawEntryLabel` implementation

### Refurbishment

#### simple

[x] design ingredients curve
[x] tiered Recipes: part + medicine

#### mangled

[x] design ingredients curve
[x] tiered recipes

## Gfx

[~] reclaimed part
[~] damaged part