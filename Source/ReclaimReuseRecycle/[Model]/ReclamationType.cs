using System;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {
    public enum ReclamationType {
        NonSterile,
        Mangled
    }

    public static class FormattingUtility {

        public static string Translate(this ReclamationType t) {
            switch (t) {
                case ReclamationType.NonSterile:
                    return LanguageKeys.r3.R3_Reclamation_NonSterile.Translate();
                case ReclamationType.Mangled:
                    return LanguageKeys.r3.R3_Reclamation_Mangled.Translate();
                default:
                    return $"{t}";
            }
        }

        public static string ToHexColor(this Color c) {
            return $"#{(byte) (c.r*255):X2}{(byte) (c.g*255):X2}{(byte) (c.b*255):X2}{(byte) (c.a*255):X2}";
        }
    }
}