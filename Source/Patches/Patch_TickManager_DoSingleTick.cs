using Verse;
using System.Reflection;
using HarmonyLib;

namespace CrunchyDuck.SetSeedRandom.Patches {
    public class Patch_TickManager_DoSingleTick {
        public static MethodInfo Target = AccessTools.Method(typeof(TickManager), nameof(TickManager.DoSingleTick));

        public static void Prefix(TickManager __instance) {
            Rand.PushState(SeedManager.seedFromString + __instance.TicksGame);
        }

        public static void Postfix() {
            Rand.PopState();
		}
    }
}
