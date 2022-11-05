using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using System;
using CrunchyDuck.SetSeedRandom.Patches;

// Just tossing this all into one file because it's not complex enough to call for better structure.
namespace CrunchyDuck.SetSeedRandom {
    [StaticConstructorOnStartup]
    public class Main {
        static Main() {
            var harmony = new Harmony("CrunchyDuck.SetSeedRandom");
            AddPatch(harmony, typeof(Patch_TickManager_DoSingleTick));
        }

        private static void AddPatch(Harmony harmony, Type type) {
            // TODO: Sometime make a patch interface.
            var prefix = type.GetMethod("Prefix") != null ? new HarmonyMethod(type, "Prefix") : null;
            var postfix = type.GetMethod("Postfix") != null ? new HarmonyMethod(type, "Postfix") : null;
            var trans = type.GetMethod("Transpiler") != null ? new HarmonyMethod(type, "Transpiler") : null;
            harmony.Patch((MethodBase)type.GetField("Target").GetValue(null), prefix: prefix, postfix: postfix, transpiler: trans);
        }
    }

    public class ModMenuThing : Mod {
        public ModMenuThing(ModContentPack content) : base(content) {}

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect) {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            //listingStandard.CheckboxForIntInput("Custom seed on new game", ref Settings.isSeedRandom, ref Settings.currentStartingSeed);
            if (Find.World == null)
                listingStandard.Label("Current world seed: Not in world");
            else
				listingStandard.Label("Current world seed: " + SeedManager.currentSeed);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() {
            return "Set Seed Random";
        }
    }

    public class SeedManager : GameComponent {
        public static int seedFromString;
        public static string currentSeed;

        public SeedManager(Game game) {}

		public override void FinalizeInit() {
			base.FinalizeInit();
            var w = Find.World;
            currentSeed = w.info.seedString;
            seedFromString = w.info.Seed;
        }
	}
}
