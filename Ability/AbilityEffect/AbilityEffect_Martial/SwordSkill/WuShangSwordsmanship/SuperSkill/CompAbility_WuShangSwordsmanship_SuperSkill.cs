

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using YanYu.Utilities;

namespace YanYu
{
    public class CompAbility_WuShangSwordsmanship_SuperSkill : CompAbilityEffect_SwordBase
    {
        public new CompProperties_WuShangSwordsmanship_SuperSkill Props => (CompProperties_WuShangSwordsmanship_SuperSkill)this.props;
        public Pawn GetPawn => this.parent.pawn;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            List<IntVec3> explosedPosition = new List<IntVec3>();
            var damagedPawns = new HashSet<Pawn>();
            var tickToCombo = Find.TickManager.TicksGame + 60;
            var tickToDamage1 = Find.TickManager.TicksGame;
            var tickToDamage2 = Find.TickManager.TicksGame + 30;
            var tickToDamage3 = Find.TickManager.TicksGame + 60;
            var tickToDamage4 = Find.TickManager.TicksGame + 130;

            float scale = 5f;
            List<Thing> ignoredThings = new List<Thing>
            {
                GetPawn
            };

            foreach (Pawn mapPawn in GetPawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (mapPawn.Faction == GetPawn.Faction) ignoredThings.Add(mapPawn);
            }
            //喊口号！
            MoteMaker.ThrowText(
                GetPawn.DrawPos + new Vector3(0, 0, 0.5f),
                map: GetPawn.Map,
                text: "GracefulDance".Translate()
            );
            //第一段特效 3个
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck1, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage1
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck2, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage1+2
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck3, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage1 + 4
            );
            //第一段伤害 
            IntVec3 center = (GetPawn.Position.ToVector3Shifted() + (target.Cell.ToVector3Shifted() - GetPawn.Position.ToVector3Shifted()).normalized * 8).ToIntVec3();
            AreaAttackUtility.DoCircleDamage(
                GetPawn,
                target,
                6f,
                Props.damage * 0.1f,
                damageDef:DamageDefOf.Cut,
                center:center,
                ignoredThings: ignoredThings

            );
            //第二段特效 5个
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck4, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage2 
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck5, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage2 + 2
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck6, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage2 + 4
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck7, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage2 + 6
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck8, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage2 + 8
            );

            //第二段伤害
            AreaAttackUtility.DoCircleDamage(
                GetPawn,
                target,
                6f,
                Props.damage * 0.2f,
                damageDef: DamageDefOf.Cut,
                center: center,
                ignoredThings: ignoredThings
            );

            //第三段特效 8个
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck9, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck10, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 2
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck11, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 4
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck12, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 6
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck13, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 8
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck14, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 10
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck15, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 12
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_WuShangSwordsmanship_fleck16, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 0f
                ),
                tickToDamage3 + 14
            );


            //第三段伤害
            AreaAttackUtility.DoCircleDamage(
                GetPawn,
                target,
                6f,
                Props.damage * 0.7f,
                damageDef: DamageDefOf.Cut,
                center: center,
                ignoredThings: ignoredThings
            );

            MoteMaker.ThrowText(
                GetPawn.DrawPos + new Vector3(0, 0, 1f),
                map: GetPawn.Map,
                text: "MoonDance".Translate()
            );
            DelayedActionManager.Register(() =>
            {
                Pawn attacker = GetPawn;
                var comboFleck1Time = tickToCombo + 3;
                var comboFleck2Time = tickToCombo + 6;
                var comboFleck3Time = tickToCombo + 9;
                if (!attacker.Destroyed && attacker.Spawned)
                {
                    List<Thing> DelayignoredThings = new List<Thing> { attacker };
                    foreach (Pawn mapPawn in attacker.Map.mapPawns.AllPawnsSpawned)
                    {
                        if (mapPawn.Faction == attacker.Faction)
                            DelayignoredThings.Add(mapPawn);
                    }

                    AreaAttackUtility.DoDiamondDamage(
                        attacker,
                        target,
                        14,
                        3,
                        Props.damage * 0.7f,
                        damageDef: DamageDefOf.Cut,
                        center: center,
                        ignoredThings: DelayignoredThings
                    );
                }
            }, tickToCombo);

        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);

            IntVec3 center = (GetPawn.Position.ToVector3Shifted() + (target.Cell.ToVector3Shifted() - GetPawn.Position.ToVector3Shifted()).normalized * 8).ToIntVec3();

            AreaAttactEffectPromptUtility.DrawCircleFeildEdge(
                GetPawn,
                target,
                6f,
                center: center,
                color: Color.HSVToRGB(0.95f, 0.6f, 0.9f) 
            );

            
            AreaAttactEffectPromptUtility.DrawDiamondFeildEdge(
                GetPawn,
                target,
                9.3f,
                1.8f,
                center: center,
                color: Color.HSVToRGB(0.95f, 0.2f, 0.9f)
            );
        }
    }
}
