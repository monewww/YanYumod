
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace YanYu
{
    public class HediffCompProperties_MartialHediffWithAbility : HediffCompProperties
    {
        public List<AbilityDef> YanYu_Martials;

        public bool uniqueMartial = false;

        public HediffCompProperties_MartialHediffWithAbility()
        {
            this.compClass = typeof(HediffComp_MartialHediffWithAbility);
        }
    }
}
