using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace YanYu
{
    public class CompAbility_BasicFistTechnique_SuperSkill : CompAbilityEffect_FistBase
    {
        public new CompProperties_BasicFistTechnique_SuperSkill Props => (CompProperties_BasicFistTechnique_SuperSkill)this.props;
        public Pawn GetPawn => this.parent.pawn;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            SimpleCurve accCurve = new SimpleCurve
            {
                new CurvePoint(0f, 30f),
                new CurvePoint(0.35f, 30f),
                new CurvePoint(0.4f, 450f),   
                new CurvePoint(0.45f, 0f),
                new CurvePoint(0.75f, -200f),
                new CurvePoint(0.750001f, 0f),
            };

            float start_rot = (target.Cell - GetPawn.Position).AngleFlat;
            var mote = (MoteUtil.Mote_MoveGrow)ThingMaker.MakeThing(ThingDef.Named("YanYu_Mote_BasicFistTechnique_MoveGrow"));
            mote.Setup(
                startPos: GetPawn.Position.ToVector3Shifted(),
                endPos: target.CenterVector3,
                startScale: new Vector3(5f, 1f, 5f),
                endScale: new Vector3(20f, 1f, 20f),
                startSpeed: 0f,
                acc: accCurve,
                lastTime: 1f,
                scaleUpTime:0.5f,
                startRot: start_rot
            );

            GenSpawn.Spawn(mote, GetPawn.Position, GetPawn.Map);

            Vector3 centerPos = (target.Cell.ToVector3Shifted() - GetPawn.Position.ToVector3Shifted()).normalized * 16f + GetPawn.Position.ToVector3Shifted();

            DelayedActionManager.Register(() =>
            {
                Pawn attacker = GetPawn;
                if (!attacker.Destroyed && attacker.Spawned)
                {
                    List<Thing> DelayignoredThings = new List<Thing> { attacker };
                    foreach (Pawn mapPawn in attacker.Map.mapPawns.AllPawnsSpawned)
                    {
                        if (mapPawn.Faction == attacker.Faction)
                            DelayignoredThings.Add(mapPawn);
                    }

                    AreaAttackUtility.DoEllipticalDamage(
                        GetPawn,
                        target,
                        16f,
                        3.8f,
                        Props.damage,
                        center: centerPos.ToIntVec3(),
                        startAngle: 120f,
                        endAngle: 240f,
                        damageDef: DamageDefOf.Blunt,
                        ignoredThings: DelayignoredThings
                    );
                }
            }, Find.TickManager.TicksGame+45);

        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);
            Vector3 centerPos = (target.Cell.ToVector3Shifted()-GetPawn.Position.ToVector3Shifted()).normalized * 16f + GetPawn.Position.ToVector3Shifted();
            AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                GetPawn,
                target.Cell,
                16f,
                3.8f,
                center: centerPos.ToIntVec3(),
                startAngle: 120f,
                endAngle: 240f,
                color: Color.HSVToRGB(0f, 1f, 0.4f)
            );
        }
    }
}
