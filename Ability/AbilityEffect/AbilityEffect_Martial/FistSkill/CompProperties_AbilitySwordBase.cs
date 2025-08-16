using RimWorld;

namespace YanYu
{
    public class CompProperties_AbilityFistBase : CompProperties_AbilityEffect
    {
        public string keyword = "sword";
        public CompProperties_AbilityFistBase()
        {
            compClass = typeof(CompAbilityEffect_FistBase);
        }
    }
}
