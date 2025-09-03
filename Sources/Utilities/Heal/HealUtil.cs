using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace YanYu
{
    public static class HealUtil
    {
        public static void TryHeal(Pawn pawn, float amount)
        {
            if (pawn.health == null || pawn.Dead) return;

            var injuredParts = pawn.health.hediffSet.GetInjuredParts();
            foreach (var part in injuredParts.InRandomOrder())
            {
                var hediffs = pawn.health.hediffSet.hediffs
                    .Where(h => h.Part == part && h is Hediff_Injury injury && injury.CanHealNaturally())
                    .Cast<Hediff_Injury>();

                foreach (var injury in hediffs)
                {
                    float toHeal = Math.Min(injury.Severity, amount);
                    injury.Heal(toHeal);
                    amount -= toHeal;
                    if (amount <= 0f) return;
                }
            }
        }
    }
}
