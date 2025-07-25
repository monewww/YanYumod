
using Verse;

namespace YanYu
{
    public class HediffComp_AttackTrigger: HediffComp
    {

        public virtual void doEffect(ref DamageInfo dinfo, Pawn pawn)
        {
            Log.Message($"there is no doeffect function in HediffComp_AttackTrigger triggered on {pawn.Name} with damage {dinfo.Amount}");
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
        }

    }
}
