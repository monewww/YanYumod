using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YanYu
{
    public class CompProperties_WuShangSwordsmanship_SuperSkill : CompProperties_AbilitySwordBase
    {
        public int damage;
        public CompProperties_WuShangSwordsmanship_SuperSkill()
        {
            compClass = typeof(CompAbility_WuShangSwordsmanship_SuperSkill);
        }
    }
}
