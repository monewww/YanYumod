
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace YanYu
{
    [HarmonyPatch(typeof(DamageWorker_AddInjury), "ApplyToPawn")]
    public static class Patch_DamageWorker_ModifySwordMomentum
    {
        [HarmonyPrefix]
        public static void Prefix(ref DamageInfo dinfo, Pawn pawn)
        {
            //Log.Message($"Institator: {dinfo.Instigator?.LabelShort}, Target: {pawn.LabelShort}");
            if (!(dinfo.Instigator is Pawn attacker)) return;
            //总觉得所有pawn都要检测的话太消耗资源，只管同派系的吧
            if (attacker.Faction != Faction.OfPlayer) return;
            //Log.Message("going on");
            var hediffs = attacker.health.hediffSet.hediffs.OfType<HediffWithComps>().ToList();

            foreach (var hediff in hediffs)
            {
                foreach (var comp in hediff.comps.OfType<HediffComp_AttackTrigger>())
                {
                    Log.Message($"[舞殇剑势] {attacker.LabelShort} 触发了剑势被动效果。");
                    comp.doEffect(ref dinfo, pawn);
                }
            }


        }
    }




}
