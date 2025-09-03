using RimWorld;
using Verse;


namespace YanYu
{
    public class ThingCompProperties_HealBlood: CompProperties_UseEffect
    {
        public float healAmount = 0.1f;
        public ThingCompProperties_HealBlood()
        {
            compClass = typeof(ThingComp_HealBlood);
        }
    }
}
