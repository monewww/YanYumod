using RimWorld;
using Verse;

namespace YanYu
{
    public class ThingCompProperties_UseMartialbook: CompProperties_UseEffect
    {
        public HediffDef giveHediffDef;

        public string martialName;

        public bool uniqueMartial;
        public ThingCompProperties_UseMartialbook()
        {
            compClass = typeof(ThingComp_UseMartialbook);
        }

    }
}
