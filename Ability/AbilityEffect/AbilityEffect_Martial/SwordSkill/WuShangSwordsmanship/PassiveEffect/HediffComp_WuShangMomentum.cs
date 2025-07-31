using RimWorld;
using Verse;

namespace YanYu
{
    public class HediffComp_WuShangMomentum : HediffComp_AttackTrigger
    {
        private int lastAttackTick = 0;

        public HediffCompProperties_WuShangMomentum Props => (HediffCompProperties_WuShangMomentum)this.props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (Find.TickManager.TicksGame - lastAttackTick >= Props.ticksToDisappear)
            {
                // 输出日志
                Log.Message($"[舞殇剑势] {Pawn?.LabelShort ?? "？？？"} 的剑势已结束，移除被动效果。");

                // 清除被动效果 Hediff
                var passiveHediff = Pawn?.health?.hediffSet?.GetFirstHediffOfDef(
                    DefDatabase<HediffDef>.GetNamed("YanYu_MartialPassiveEffect_WuShangMomentum", true));

                if (passiveHediff != null)
                {
                    Pawn.health.RemoveHediff(passiveHediff);
                }

            }
        }

        public override void doEffect(ref DamageInfo dinfo, Pawn victim)
        {
            if (Pawn == null) return;

            lastAttackTick = Find.TickManager.TicksGame;

            var passiveHediffDef = HediffDef.Named("YanYu_MartialPassiveEffect_WuShangMomentum");
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
            float armorPenBonus = Props.apPerStack * currentSeverity;

            dinfo.SetAmount(dinfo.Amount * (1f + damageBonus));
            DamageInfoUtil.AddArmorPenetration(ref dinfo, armorPenBonus);

            //Log.Message($"[舞殇剑势] {Pawn.LabelShort} 触发剑势，第 {currentSeverity} 层，增加伤害{damageBonus:P0} 穿甲{armorPenBonus:P0}");
        }


        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref lastAttackTick, "lastAttackTick", 0);
        }
    }
}
