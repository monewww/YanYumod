using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
using YanYu.Utilities;

namespace YanYu
{
    public class CompAbility_BasicSwordsmanship_SuperSkill : CompAbilityEffect_SwordBase
    {
        public new CompProperties_BasicSwordsmanship_SuperSkill Props => (CompProperties_BasicSwordsmanship_SuperSkill)this.props;
        public Pawn GetPawn => this.parent.pawn;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            List<IntVec3> explosedPosition = new List<IntVec3>();
            var damagedPawns = new HashSet<Pawn>();
            var tickToCombo = Find.TickManager.TicksGame + 60;
            var tickToFleck1 = Find.TickManager.TicksGame ; // 第一段特效立即触发
            var tickToFleck2 = Find.TickManager.TicksGame + 3;
            var tickToFleck3 = Find.TickManager.TicksGame + 6;
            //技能后摇
            GetPawn.stances.stagger.StaggerFor(60);

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
                text: "SubstanceWithinFeint".Translate()  
            );
            //三段特效
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck1,scale),
                    rotationAngle: 0f,
                    offsetRight: 2f,
                    offsetForward: 5f
                ),
                tickToFleck1
            );

            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck2, scale),
                    rotationAngle: 0f,
                    offsetRight: 0f,
                    offsetForward: 5f
                ),
                tickToFleck2
            );
            DelayedActionManager.Register(() =>
                AreaAttactEffectUtility.DoEffect(
                    GetPawn,
                    target,
                    FleckMaker.GetDataStatic(GetPawn.Position.ToVector3Shifted(), GetPawn.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck3, scale),
                    rotationAngle: 0f,
                    offsetRight: -2.8f,
                    offsetForward: 3f
                ),
                tickToFleck3
            );

            AreaAttackUtility.DoEllipticalDamage(
                GetPawn,
                target,
                10,
                5,
                Props.damage,
                DamageDefOf.Cut,
                ignoredThings: ignoredThings
            );

            if (Random.value < Props.comboChance)
            {
                MoteMaker.ThrowText(
                    GetPawn.DrawPos + new Vector3(0, 0, 1f), 
                    map: GetPawn.Map,
                    text: "ShadowInPursuit".Translate()
                );
                //延迟造成二段伤害
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
                        //三段特效
                        DelayedActionManager.Register(() =>
                            AreaAttactEffectUtility.DoEffect(
                                attacker,
                                target,
                                FleckMaker.GetDataStatic(attacker.Position.ToVector3Shifted(), attacker.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck2_1, scale),
                                rotationAngle: 0f,
                                offsetRight: 0f,
                                offsetForward: 10f
                            ),
                            comboFleck1Time
                        );
                        DelayedActionManager.Register(() =>
                            AreaAttactEffectUtility.DoEffect(
                                attacker,
                                target,
                                FleckMaker.GetDataStatic(attacker.Position.ToVector3Shifted(), attacker.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck2_2, scale),
                                rotationAngle: 0f,
                                offsetRight: 2f,
                                offsetForward: 10f
                            ),
                            comboFleck2Time
                        );
                        DelayedActionManager.Register(() =>
                            AreaAttactEffectUtility.DoEffect(
                                attacker,
                                target,
                                FleckMaker.GetDataStatic(attacker.Position.ToVector3Shifted(), attacker.Map, YanYuFleckDefOf.YanYu_MartialEffect_BasicSwordsmanship_fleck2_3, scale),
                                rotationAngle: 0f,
                                offsetRight: 2f,
                                offsetForward: 9f
                            ),
                            comboFleck3Time
                        );
                        //二段伤害
                        AreaAttackUtility.DoEllipticalDamage(
                            attacker,
                            target,
                            20,
                            3,
                            Props.damage,
                            DamageDefOf.Cut,
                            ignoredThings: DelayignoredThings
                        );
                    }
                }, tickToCombo);
            }

        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);

            AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                GetPawn,
                target,
                10,
                5
            );

            AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                GetPawn,
                target,
                20,
                3,
                color: Color.yellow
            );
        }
    }
}
