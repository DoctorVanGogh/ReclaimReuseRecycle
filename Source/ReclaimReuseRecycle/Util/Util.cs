using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public static class Util {

        static Util() {
            _giveShortHash = Create_GiveShortHashInvocator();
        }

        /// <summary>
        /// Get's a <see cref="BodyPartRecord"/>'s descendants
        /// </summary>
        /// <param name="record">Part to get descendants for.</param>
        /// <param name="includeSelf">Include <paramref name="record"/> in enumeration.</param>
        public static IEnumerable<BodyPartRecord> GetDescendants(this BodyPartRecord record, bool includeSelf = false) {
            if (includeSelf)
                yield return record;

            foreach (var descendant in record.parts.SelectMany(p => p.GetDescendants(true))) {
                yield return descendant;
            }

        }

        /// <summary>
        /// Creates a <see cref="Thing"/> from a <paramref name="def"/>.
        /// </summary>
        /// <param name="def">Definition for the <see cref="Thing"/></param>
        /// <param name="healthPercentage">Health percentage of the created <see cref="Thing"/></param>
        /// <returns>Will have between one and <see cref="Thing.MaxHitPoints"/> Hitpoints.</returns>
        public static Thing CreateThing(ThingDef def, float? healthPercentage = null) {
            Thing thing = ThingMaker.MakeThing(def);

            if (healthPercentage != null) {
                thing.HitPoints = Math.Max(1, Math.Min(thing.MaxHitPoints, (int)Math.Round(thing.MaxHitPoints * healthPercentage.Value )));
            }
            return thing;
        }

        /// <summary>
        /// Get's a <see cref="BodyPartRecord"/>'s health percentage.
        /// </summary>
        /// <param name="part">Part to get the health percentage for.</param>
        /// <param name="hediffSet"><see cref="HediffSet"/> tracking the owning <see cref="Pawn"/>'s injuries.</param>
        /// <returns>Body part's health percentage.</returns>
        public static float HitpointsFactor(BodyPartRecord part, HediffSet hediffSet) {
            return hediffSet.GetPartHealth(part)/part.def.GetMaxHealth(hediffSet.pawn);
        }

        public static void Error(string message) {
            Verse.Log.Error($"[R³] {message}");
        }
        public static void Warning(string message) {
            Verse.Log.Warning($"[R³] {message}");
        }

        public static void Log(string message) {
            Verse.Log.Message($"[R³] {message}");
        }

        [Conditional("TRACE")]
        public static void Trace(string message) {
            Log(message);
        }

        private static readonly Action<Def, Type> _giveShortHash;

        public static void GiveShortHash<TDef>(this TDef value) where TDef : Def {
            _giveShortHash(value, typeof(TDef));
        }

        internal static Action<Def, Type> Create_GiveShortHashInvocator() {
            Type[] parametersSignature = new[] {typeof(Def), typeof(Type)};

            MethodInfo miGiveShortHashInternal = typeof(ShortHashGiver).GetMethod(
                "GiveShortHash",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                parametersSignature,
                null);

            if (miGiveShortHashInternal == null) {
                throw new InvalidOperationException("R³: Cannot retrieve ShortHashGiver.GiveShortHash method...");
            }

            DynamicMethod dm = new DynamicMethod("__GiveShortHash_Dynamic", null, parametersSignature, typeof(ShortHashGiver));
            ILGenerator IL = dm.GetILGenerator();
            IL.Emit(OpCodes.Ldarg_0);                               // 'Def' argument
            IL.Emit(OpCodes.Ldarg_1);                               // 'Type' argument
            IL.Emit(OpCodes.Call, miGiveShortHashInternal);         // call 'ShortHashGiver.GiveShortHash'
            IL.Emit(OpCodes.Ret);                                   // return

            return (Action<Def, Type>) dm.CreateDelegate(typeof(Action<Def, Type>));
        }
    }
}
