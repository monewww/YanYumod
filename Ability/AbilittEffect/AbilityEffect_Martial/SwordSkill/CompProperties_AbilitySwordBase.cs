using RimWorld;

namespace YanYu
{
    public class CompProperties_AbilitySwordBase : CompProperties_AbilityEffect
    {
        public string keyword = "sword";
        public CompProperties_AbilitySwordBase()
        {
            compClass = typeof(CompAbilityEffect_SwordBase);
        }
    }
}
