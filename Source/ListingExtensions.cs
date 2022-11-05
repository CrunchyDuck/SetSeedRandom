using RimWorld;
using Verse;
using Verse.Sound;
using System;
using UnityEngine;

namespace CrunchyDuck.SetSeedRandom {
    public static class ListingExtensions {
        // Stolen heartlessly from https://github.com/PeteTimesSix/ResearchReinvented/blob/main/ResearchReinvented/Source/Utilities/ListingExtensions.cs
        public static void SliderLabeled(this Listing_Standard instance, string label, ref float value, float min, float max, float roundTo = -1, float displayMult = 1, int decimalPlaces = 0, string valueSuffix = "", string tooltip = null, Action onChange = null) {
            if (!string.IsNullOrEmpty(label)) {
                Rect r = instance.GetRect(22f);
                var anchor = Text.Anchor;
                // Min
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(r, min * displayMult + valueSuffix);
                // Readout
                Text.Anchor = TextAnchor.MiddleCenter;
                TooltipHandler.TipRegion(r, tooltip);
                Widgets.Label(r, $"{label}: {(value * displayMult).ToString($"F{decimalPlaces}")}{valueSuffix}");
                // Max
                Text.Anchor = TextAnchor.MiddleRight;
                Widgets.Label(r, max * displayMult + valueSuffix);

                Text.Anchor = anchor;
            }
            var valueBefore = value;
            value = instance.FullSlider(value, min, max, roundTo: roundTo);
            if (value != valueBefore) {
                onChange?.Invoke();
            }
        }

        public static float FullSlider(this Listing_Standard instance, float val, float min, float max, float roundTo = -1f, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null) {
            float newVal = Widgets.HorizontalSlider(instance.GetRect(22f), val, min, max, middleAlignment, label, leftAlignedLabel, rightAlignedLabel, roundTo);
            if (newVal != val) {
                SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
            }
            instance.Gap(instance.verticalSpacing);
            return newVal;
        }

        public static void CheckboxForIntInput(this Listing_Standard instance, string check_label, ref bool check_on, ref int val) {
            Listing_Standard listing_standard = new Listing_Standard();
            listing_standard.Begin(instance.GetRect(24f));
            listing_standard.ColumnWidth = listing_standard.ColumnWidth / 2 - 10;

            listing_standard.CheckboxLabeled(check_label, ref check_on);

            listing_standard.NewColumn();
			if (check_on) {
				string edit_buffer = val.ToString();
				listing_standard.TextFieldNumeric<int>(ref val, ref edit_buffer, -2147483648, 2147483647);
			}
            listing_standard.End();
        }

        public static void LabelledInput(this Listing_Standard instance, string label, ref int val) {
            Listing_Standard listing_standard = new Listing_Standard();
            listing_standard.Begin(instance.GetRect(24f));
            listing_standard.ColumnWidth = listing_standard.ColumnWidth / 2 - 10;

			listing_standard.Label(label);

            listing_standard.NewColumn();
			string edit_buffer = val.ToString();
			listing_standard.TextFieldNumeric<int>(ref val, ref edit_buffer, int.MinValue, int.MaxValue);

            listing_standard.End();
        }

        public static void LabelledInput(this Listing_Standard instance, string label, ref string val) {
            Listing_Standard listing_standard = new Listing_Standard();
            listing_standard.Begin(instance.GetRect(24f));
            listing_standard.ColumnWidth = listing_standard.ColumnWidth / 2 - 10;

			listing_standard.Label(label);

            listing_standard.NewColumn();
			val = listing_standard.TextEntry(val);

            listing_standard.End();
        }
    }
}
