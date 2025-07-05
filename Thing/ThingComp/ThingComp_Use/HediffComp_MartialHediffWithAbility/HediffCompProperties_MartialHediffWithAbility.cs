
using Verse;
using RimWorld;

namespace YanYu
{
    public class HediffCompProperties_MartialHediffWithAbility : HediffCompProperties
    {
        public AbilityDef YanYu_Martial;

        public bool uniqueMartial = false;

        public HediffCompProperties_MartialHediffWithAbility()
        {
            this.compClass = typeof(HediffComp_MartialHediffWithAbility);
        }
    }
}
