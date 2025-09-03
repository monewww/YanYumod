
namespace YanYu
{
    public class CompProperties_BasicSwordsmanship_SuperSkill : CompProperties_AbilitySwordBase
    {
        public int damage;
        public float comboChance;

        public CompProperties_BasicSwordsmanship_SuperSkill()
        {
            compClass = typeof(CompAbility_BasicSwordsmanship_SuperSkill);
            //damage = 50;
            //comboChance = 0.7f; 
        }
    }
}
