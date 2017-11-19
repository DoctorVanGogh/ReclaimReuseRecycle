using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle {

    
    [StaticConstructorOnStartup]
    // ReSharper disable once UnusedMember.Global
    public class R3Mod : Mod {

        private static string[] _tagLines;
        private Texture2D _logo;

        private string TagLine;
        private const int tagDelay = 10;

        private DateTime lastChange = DateTime.Now;


        public R3Mod(ModContentPack content) : base(content) {
            HarmonyInstance harmony = HarmonyInstance.Create("DoctorVanGogh.ReclaimReuseRecycle");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Util.Log($"Initialized Harmony patches {Assembly.GetExecutingAssembly().GetName().Version}");


            foreach (var t in typeof(Def).AllSubclasses()) {
                Util.Log($"{t}"); 
            }


            GetSettings<Settings>();
        }

        public static string[] TagLines => _tagLines ?? (_tagLines = LanguageKeys.r3.R3_Tagline.Translate().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));

        public Texture2D Logo => _logo ?? (_logo = ContentFinder<Texture2D>.Get("UI/Recycle", true));

        public override string SettingsCategory() {
            return LanguageKeys.r3.R3_SettingsCategory.Translate();
        }

        //[Reloader.ReloadMethod]
        public override void DoSettingsWindowContents(Rect inRect) {

            var rect = new Rect(inRect);

            const float imageSize = 196f;

            GUI.BeginGroup(rect);

            GUI.DrawTexture(new Rect(16f, 16f, imageSize, imageSize), Logo);

            Text.Font = GameFont.Medium;
            var tagStyle = new GUIStyle(Text.CurTextAreaReadOnlyStyle) {
                               richText = true,
                               alignment = TextAnchor.MiddleCenter
                           };

            // flip tagline every 'tagDelay' seconds
            if (TagLine == null || (DateTime.Now - lastChange  ).TotalSeconds > tagDelay) {
                TagLine = TagLines.RandomElement();
                lastChange = DateTime.Now;
            }

            GUI.Label(
                new Rect(imageSize + 16f*2, 16f, inRect.width - imageSize - 16f*3, imageSize),
                $"<size=48><i>{TagLine}</i></size>",
                tagStyle);

            Text.Font = GameFont.Small;

            var y = imageSize + 16f;

            var s = new GUIStyle(Text.CurTextAreaReadOnlyStyle) {
                        richText = true
                    };

            var content = new GUIContent(LanguageKeys.r3.R3_RangeExplanation.Translate());
            var sz = s.CalcSize(content);

            GUI.Label(new Rect(rect.x, y, rect.width, sz.y),content, s);

            y += sz.y;

            const float sliderHeight = 26f;

            Text.Anchor = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(rect.x, y, rect.width*0.5f, sliderHeight), LanguageKeys.r3.R3_Settings_Recoverable.Translate(ThingDefGenerator_Reclaimed.NonSterileColorHex, ReclamationType.NonSterile.Translate()));
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.FloatRange(new Rect(rect.width*0.5f, y, rect.width*0.5f, sliderHeight), 1, ref Settings.NonSterileRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
            y += sliderHeight + 5f;

            Settings.MangledRange.max = Math.Min(Settings.MangledRange.max, Settings.NonSterileRange.min);
            Settings.MangledRange.min = Math.Min(Settings.MangledRange.max, Settings.MangledRange.min);

            Text.Anchor = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(rect.x, y, rect.width * 0.5f, sliderHeight), LanguageKeys.r3.R3_Settings_Recoverable.Translate(ThingDefGenerator_Reclaimed.MangledColorHex, ReclamationType.Mangled.Translate()));
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.FloatRange(new Rect(rect.width * 0.5f , y, rect.width * 0.5f, sliderHeight), 2, ref Settings.MangledRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
            y += sliderHeight + 5f;

            GUI.EndGroup();           
        }
    }
}
