using HarmonyLib;
using RimWorld;
using Verse;

namespace YanYu
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            var harmony = new Harmony("com.yanyu");
            harmony.PatchAll();
        }
    }
}
