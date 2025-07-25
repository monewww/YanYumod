
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace YanYu
{
    public class HediffCompProperties_MartialHediff : HediffCompProperties
    {
        public List<AbilityDef> YanYu_Martials;
        //public List<HediffCompProperties> YanYu_PassiveEffects;

        public bool uniqueMartial = false;

        public HediffCompProperties_MartialHediff()
        {
            this.compClass = typeof(HediffComp_MartialHediff);
        }
    }
}
