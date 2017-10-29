using System.Diagnostics.CodeAnalysis;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    [DefOf]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class R3DefOf {
        #region harvest recipes
        public static RecipeDef R3_HarvestCorpseFlesh_Primitive;
        public static RecipeDef R3_HarvestCorpseFlesh_Advanced;
        public static RecipeDef R3_HarvestCorpseFlesh_Glittertech;
        public static RecipeDef R3_HarvestCorpseMechanoid_Primitive;
        public static RecipeDef R3_HarvestCorpseMechanoid_Advanced;
        public static RecipeDef R3_HarvestCorpseMechanoid_Glittertech;
        #endregion

        #region refurbishment recipes
        public static RecipeDef R3_Sterilize_Primitive;
        public static RecipeDef R3_Sterilize_Advanced;
        public static RecipeDef R3_Sterilize_Glittertech;
        public static RecipeDef R3_Refurbish_Primitive;
        public static RecipeDef R3_Refurbish_Advanced;
        public static RecipeDef R3_Refurbish_Glittertech;
        #endregion

        #region other recipes
        public static RecipeDef ButcherCorpseFlesh;
        public static RecipeDef ButcherCorpseMechanoid;
        #endregion

        #region ThingCategoryDefs
        public static ThingCategoryDef BodyPartsNatural;
        public static ThingCategoryDef BodyPartsReclaimed;
        public static ThingCategoryDef BodyPartsNonSterile;
        public static ThingCategoryDef BodyPartsNonSterile_Primitive;
        public static ThingCategoryDef BodyPartsNonSterile_Advanced;
        public static ThingCategoryDef BodyPartsNonSterile_Glittertech;
        public static ThingCategoryDef BodyPartsMangled;
        public static ThingCategoryDef BodyPartsMangled_Primitive;
        public static ThingCategoryDef BodyPartsMangled_Advanced;
        public static ThingCategoryDef BodyPartsMangled_Glittertech;
        #endregion

        #region SpecialThingFilterDefs
        public static SpecialThingFilterDef R3_AllowUnharvested_Primitive;
        public static SpecialThingFilterDef R3_AllowUnharvested_Advanced;
        public static SpecialThingFilterDef R3_AllowUnharvested_Glittertech;
        #endregion

        #region StatCategoryDef
        public static StatCategoryDef ReclaimedItem;
        #endregion


        #region ThingDef
        public static ThingDef R3_TableHarvesting;
        #endregion
    }
}
