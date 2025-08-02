//using System;
//using System.Linq;
//using Verse;

//namespace YanYu
//{
//    public class CompHealWhenDamage : ThingComp
//    {
//        public CompProperties_HealWhenDamage Props => (CompProperties_HealWhenDamage)this.props;

//        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
//        {
//            base.PostPreApplyDamage(ref dinfo, out absorbed);
//            absorbed = false;
//        }

//        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
//        {
//            base.PostPostApplyDamage(dinfo, totalDamageDealt);

//            // 如果不是攻击者就返回
//            if (parent == null || !(parent is ThingWithComps weapon)) return;

//            // 找到攻击者
//            Pawn attacker = dinfo.Instigator as Pawn;
//            if (attacker == null) return;

//            // 检查是否是这个武器造成的
//            if (attacker.equipment?.Primary != weapon) return;

//            // 开始回血
//            float healAmount = totalDamageDealt * Props.healRatio;
//            HealPawn(attacker, healAmount);
//        }

//        private void HealPawn(Pawn pawn, float amount)
//        {
//            if (pawn.health == null || pawn.Dead) return;

//            // 随便选几个受伤部位来治疗
//            var injuredParts = pawn.health.hediffSet.GetInjuredParts();
//            foreach (var part in injuredParts.InRandomOrder())
//            {
//                var hediffs = pawn.health.hediffSet.hediffs
//                    .Where(h => h.Part == part && h is Hediff_Injury injury && injury.CanHealNaturally())
//                    .Cast<Hediff_Injury>();

//                foreach (var injury in hediffs)
//                {
//                    float toHeal = Math.Min(injury.Severity, amount);
//                    injury.Heal(toHeal);
//                    amount -= toHeal;
//                    if (amount <= 0f) return;
//                }
//            }
//        }
//    }

//}
