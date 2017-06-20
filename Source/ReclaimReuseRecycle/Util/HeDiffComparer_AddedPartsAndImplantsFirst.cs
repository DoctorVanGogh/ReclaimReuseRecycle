using System.Collections.Generic;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public class HeDiffComparer_AddedPartsThenImplants : IComparer<Hediff> {
        public int Compare(Hediff x, Hediff y) {
            return Rank(x).CompareTo(Rank(y));
        }

        private static int Rank(Hediff value) {
            if (value == null)
                return -2;

            if (value is Hediff_AddedPart)
                return -1;
            if (value is Hediff_Implant)
                return 0;

            return 1;
        }
    }
}