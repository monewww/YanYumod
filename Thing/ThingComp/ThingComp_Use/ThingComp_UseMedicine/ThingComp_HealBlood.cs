using RimWorld;
using Verse;

namespace YanYu
{
    public class ThingComp_HealBlood: CompUseEffect
    {
        public ThingCompProperties_HealBlood Props
        {
            get
            {
                return (ThingCompProperties_HealBlood)this.props;
            }
        }

        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);
            HealUtil.TryHeal(user, Props.healAmount);
        }

    }
}
