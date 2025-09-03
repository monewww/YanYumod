using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace YanYu
{
    public class CompAbility_PianHuaQiXingQuan_SuperSkill : CompAbilityEffect_FistBase
    {
        public new CompProperties_PianHuaQiXingQuan_SuperSkill Props => (CompProperties_PianHuaQiXingQuan_SuperSkill)this.props;
        public Pawn GetPawn => this.parent.pawn;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            int gaptime = 15;
            base.Apply(target, dest);
            Hediff fistPowerHediff = GetPawn.health.hediffSet.GetFirstHediffOfDef(
                DefDatabase<HediffDef>.GetNamed("YanYu_MartialPassiveEffect_PianHuaFistPower", true));
            HediffComp_PianHuaFistPower comp= GetPawn.health.hediffSet.GetFirstHediffOfDef(
                DefDatabase<HediffDef>.GetNamed("YanYu_MartialHediff_PianHuaQiXingQuan", true))?.TryGetComp<HediffComp_PianHuaFistPower>();
            if (comp != null)
            {
                comp.doEffect();
            }
            else
            {
                Log.Error(ToString() + " could not find HediffComp_PianHuaFistPower on " + GetPawn.LabelShort);
            }

            if (fistPowerHediff?.Severity > 0 && UnityEngine.Random.value<=0.4+0.2* fistPowerHediff.Severity)
            {
                gaptime = 55;
                float[] angles = { 30f, 90f, 150f, 210f, 270f, 330f };
                float scale = 8f;
                MoteMaker.ThrowText(
                    GetPawn.DrawPos + new Vector3(0, 0, 0.5f),
                    map: GetPawn.Map,
                    text: "SeizingtheMountain".Translate()
                );
                foreach (float angle in angles)
                {
                    Vector3 dir = (target.Cell.ToVector3Shifted() - GetPawn.Position.ToVector3Shifted()).normalized;
                    //扩大一点防止被整数
                    Vector3 rotatedDir = Quaternion.Euler(0, angle, 0) * dir * 16;
                    IntVec3 targetPos = rotatedDir.ToIntVec3() + GetPawn.Position;
                    
                    DelayedActionManager.Register(() =>
                    {
                        if (!GetPawn.Destroyed && GetPawn.Spawned)
                        {
                            AreaAttactEffectUtility.DoEffect(
                                GetPawn,
                                targetPos,
                                FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_PianHuaQiXingQuan_fleck1, scale),
                                rotationAngle: 0f,
                                offsetRight: 0f,
                                offsetForward: 4f
                            );
                            List<Thing> DelayignoredThings = new List<Thing> { GetPawn };
                            foreach (Pawn mapPawn in GetPawn.Map.mapPawns.AllPawnsSpawned)
                            {
                                if (mapPawn.Faction == GetPawn.Faction)
                                    DelayignoredThings.Add(mapPawn);
                            }
                            AreaAttackUtility.DoEllipticalDamage(
                                GetPawn,
                                targetPos,
                                8f,
                                3f,
                                Props.damage*0.1f*fistPowerHediff.Severity,
                                center: GetPawn.Position,
                                startAngle: -90f,
                                endAngle: 90f,
                                damageDef: DamageDefOf.Blunt,
                                ignoredThings: DelayignoredThings
                            );

                        }

                    }, Find.TickManager.TicksGame + (int)((angle - 30) / 60 * 7));
                }
                GetPawn.health.RemoveHediff(fistPowerHediff);
            }
            DelayedActionManager.Register(() => {
                MoteMaker.ThrowText(
                    GetPawn.DrawPos + new Vector3(0, 0, 1f),
                    map: GetPawn.Map,
                    text: "SevenStarsConvergence".Translate()
                );
                if (!GetPawn.Destroyed && GetPawn.Spawned)
                {
                    AreaAttactEffectUtility.DoEffect(
                        GetPawn,
                        target.Cell,
                        FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_PianHuaQiXingQuan_fleck2, 20f),
                        rotationAngle: 0f,
                        offsetRight: 0f,
                        offsetForward: 4f
                    );
                    List<Thing> DelayignoredThings = new List<Thing> { GetPawn };
                    foreach (Pawn mapPawn in GetPawn.Map.mapPawns.AllPawnsSpawned)
                    {
                        if (mapPawn.Faction == GetPawn.Faction)
                            DelayignoredThings.Add(mapPawn);
                    }
                    AreaAttackUtility.DoEllipticalDamage(
                        GetPawn,
                        target.Cell,
                        12f,
                        4.5f,
                        Props.damage*0.6f,
                        center: GetPawn.Position,
                        startAngle: -60f,
                        endAngle: 60f,
                        damageDef: DamageDefOf.Blunt,
                        ignoredThings: DelayignoredThings
                    );

                }
            },Find.TickManager.TicksGame + gaptime);
        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);
            Hediff fistPowerHediff = GetPawn.health.hediffSet.GetFirstHediffOfDef(
                DefDatabase<HediffDef>.GetNamed("YanYu_MartialPassiveEffect_PianHuaFistPower", true));
            if (fistPowerHediff?.Severity > 0) {
                Vector3 dir = (target.Cell.ToVector3Shifted() - GetPawn.Position.ToVector3Shifted()).normalized;
                float[] angles = { 30f, 90f, 150f, 210f, 270f, 330f };
                foreach (float angle in angles)
                {
                    //扩大一点防止被整数
                    Vector3 rotatedDir = Quaternion.Euler(0, angle, 0) * dir * 16;
                    IntVec3 targetPos = rotatedDir.ToIntVec3() + GetPawn.Position;
                    AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                        GetPawn,
                        targetPos,
                        8f,
                        3f,
                        center: GetPawn.Position,
                        startAngle: -90f,
                        endAngle: 90f,
                        color: Color.HSVToRGB(0.12f, 0.85f, 0.6f + angle / 360 * 0.4f)
                    );
                }
            }
            AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                GetPawn,
                target.Cell,
                12f,
                4.5f,
                center: GetPawn.Position,
                startAngle: -60f,
                endAngle: 60f,
                color: Color.HSVToRGB(0.12f, 0.85f, 1f)
            );

        }
    }
}
