using RimWorld;
using Verse;

namespace YanYu
{
    public class HediffComp_PianHuaFistPower : HediffComp
    {
        private int lastAttackTick = 0;

        public HediffCompProperties_PianHuaFistPower Props => (HediffCompProperties_PianHuaFistPower)this.props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (Find.TickManager.TicksGame - lastAttackTick >= Props.ticksToDisappear)
            {
                var passiveHediff = Pawn?.health?.hediffSet?.GetFirstHediffOfDef(
                    DefDatabase<HediffDef>.GetNamed("YanYu_MartialPassiveEffect_PianHuaFistPower", true));

                if (passiveHediff != null)
                {
                    Pawn.health.RemoveHediff(passiveHediff);
                }

            }
        }

        public void doEffect()
        {
            if (Pawn == null) return;

            lastAttackTick = Find.TickManager.TicksGame;

            var passiveHediffDef = HediffDef.Named("YanYu_MartialPassiveEffect_PianHuaFistPower");
            Hediff existingHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(passiveHediffDef);

            float currentSeverity = 0f;

            if (existingHediff == null)
            {
                // 添加新 hediff 并设置初始层数为 1
                existingHediff = Pawn.health.AddHediff(passiveHediffDef);
                existingHediff.Severity = 1f;
                currentSeverity = 1f;
            }
            else
            {
                // 若未达最大层数，叠层
                if (existingHediff.Severity < Props.maxStack)
                {
                    existingHediff.Severity += 1f;
                }

                currentSeverity = existingHediff.Severity;
            }

            float damageBonus = Props.damagePerStack * currentSeverity;
        }


        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref lastAttackTick, "lastAttackTick", 0);
        }
    }
}
