using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public static class Util {
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
    }
}
