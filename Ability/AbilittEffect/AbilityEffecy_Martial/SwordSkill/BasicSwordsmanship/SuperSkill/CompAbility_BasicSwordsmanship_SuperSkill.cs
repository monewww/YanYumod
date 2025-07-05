using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace YanYu
{
    public class CompAbility_BasicSwordsmanship_SuperSkill : CompAbilityEffect_SwordBase
    {
        public new CompProperties_BasicSwordsmanship_SuperSkill Props => (CompProperties_BasicSwordsmanship_SuperSkill)this.props;
        public Pawn GetPawn => this.parent.pawn;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            List<IntVec3> explosedPosition = new List<IntVec3>();
            List<Thing> ignoredThings = new List<Thing>
            {
                GetPawn
            };

            foreach (Pawn mapPawn in GetPawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (mapPawn.Faction == GetPawn.Faction) ignoredThings.Add(mapPawn);
            }

            foreach (var item in GenSight.PointsOnLineOfSight(GetPawn.Position, GetLeftPoint(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, 10.0f).ToIntVec3()))
            {
                if (!GenSight.LineOfSight(GetPawn.Position, item, GetPawn.Map)) continue;
                if (!explosedPosition.Contains(item))
                {
                    GenExplosion.DoExplosion(
                        item, GetPawn.Map, parent.verb.EffectiveRange, DamageDefOf.Cut, GetPawn, 20, 2.0f, ignoredThings: ignoredThings,
                        overrideCells: GenSight.PointsOnLineOfSight(item, GenSight.LastPointOnLineOfSight(item, GetSpecifyDirectionVector3(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, item.ToVector3Shifted(), parent.verb.EffectiveRange).ToIntVec3(), intv3 => GenSight.LineOfSight(item, intv3, GetPawn.Map))).ToList(),
                        propagationSpeed: 2.0f
                        );
                    explosedPosition.Add(item);
                }
            }

            //右侧
            foreach (var item in GenSight.PointsOnLineOfSight(GetPawn.Position, GetRightPoint(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, 10.0f).ToIntVec3()))
            {
                if (!GenSight.LineOfSight(GetPawn.Position, item, GetPawn.Map)) continue;
                if (!explosedPosition.Contains(item))
                {
                    GenExplosion.DoExplosion(
                        item, GetPawn.Map, parent.verb.EffectiveRange, DamageDefOf.Cut, GetPawn, 20, 2.0f, ignoredThings: ignoredThings,
                        overrideCells: GenSight.PointsOnLineOfSight(item, GenSight.LastPointOnLineOfSight(item, GetSpecifyDirectionVector3(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, item.ToVector3Shifted(), parent.verb.EffectiveRange).ToIntVec3(), intv3 => GenSight.LineOfSight(item, intv3, GetPawn.Map))).ToList(),
                        propagationSpeed: 2.0f
                        );
                    explosedPosition.Add(item);
                }
            }

            ////获取指定方向的矩形范围
            //List<IntVec3> targetPositions = GetIntVec3s(target, parent.verb.EffectiveRange);

            ////获取范围内的敌方
            ////不对机械族生效？
            //List<Pawn> mapPawnInRange = new List<Pawn>();
            //foreach (Pawn mapPawn in GetPawn.Map.mapPawns.AllPawnsSpawned)
            //{
            //    //范围内
            //    if (!targetPositions.Contains(mapPawn.Position)) continue;
            //    //自己
            //    if (mapPawn.Equals(GetPawn)) continue;
            //    //友伤保护
            //    if (mapPawn.Faction == GetPawn.Faction) continue;
            //    mapPawnInRange.Add(mapPawn);
            //}


        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);

            if (target.Cell == GetPawn.Position) return;

            // 1 / 3
            GenDraw.DrawFieldEdges(GetIntVec3s(target, parent.verb.EffectiveRange / 3 * 1), Color.red);

            // 2 / 3
            GenDraw.DrawFieldEdges(GetIntVec3s(target, parent.verb.EffectiveRange / 3 * 2), Color.yellow);

            // 3 / 3
            GenDraw.DrawFieldEdges(GetIntVec3s(target, parent.verb.EffectiveRange), Color.green);
        }


        public List<IntVec3> GetIntVec3s(LocalTargetInfo target, float range)
        {
            List<IntVec3> targetPosition = new List<IntVec3>();

            //左侧
            foreach (var item in GenSight.PointsOnLineOfSight(GetPawn.Position, GetLeftPoint(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, 10.0f).ToIntVec3()))
            {
                if (!GenSight.LineOfSight(GetPawn.Position, item, GetPawn.Map)) continue;
                foreach (var item1 in GenSight.PointsOnLineOfSight(item, GetSpecifyDirectionVector3(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, item.ToVector3Shifted(), range).ToIntVec3()))
                {
                    if (!targetPosition.Contains(item1))
                    {
                        targetPosition.Add(item1);
                    }
                }
            }

            //右侧
            foreach (var item in GenSight.PointsOnLineOfSight(GetPawn.Position, GetRightPoint(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, 10.0f).ToIntVec3()))
            {
                if (!GenSight.LineOfSight(GetPawn.Position, item, GetPawn.Map)) continue;
                foreach (var item1 in GenSight.PointsOnLineOfSight(item, GetSpecifyDirectionVector3(GetPawn.Position.ToVector3Shifted(), target.CenterVector3, item.ToVector3Shifted(), range).ToIntVec3()))
                {
                    if (!targetPosition.Contains(item1))
                    {
                        targetPosition.Add(item1);
                    }
                }
            }

            //范围内 && 可视
            return targetPosition.Where(intv3 => intv3.InHorDistOf(GetPawn.Position, parent.verb.EffectiveRange) && GenSight.LineOfSight(GetPawn.Position, intv3, GetPawn.Map)).ToList();
        }

        //获取左点
        public Vector3 GetLeftPoint(Vector3 p1, Vector3 p2, float range)
        {
            Vector3 dir = p2 - p1;
            Vector3 horizontalDir = new Vector3(dir.x, 0, dir.z);

            if (horizontalDir.sqrMagnitude < 0.0001f)
            {
                return p1;
            }

            horizontalDir.Normalize();
            Vector3 rightDir = Vector3.Cross(Vector3.up, horizontalDir);
            Vector3 leftDir = -rightDir;
            return p1 + leftDir * range;
        }

        //获取右点
        public Vector3 GetRightPoint(Vector3 p1, Vector3 p2, float range)
        {
            Vector3 dir = p2 - p1;
            Vector3 horizontalDir = new Vector3(dir.x, 0, dir.z);

            if (horizontalDir.sqrMagnitude < 0.0001f)
            {
                return p1;
            }

            horizontalDir.Normalize();
            Vector3 rightDir = Vector3.Cross(Vector3.up, horizontalDir);
            return p1 + rightDir * range;
        }

        public Vector3 GetSpecifyDirectionVector3(Vector3 p1, Vector3 p2, Vector3 p3, float range)
        {
            // 计算起点和终点之间的方向向量
            Vector3 direction = (p1 - p2).normalized;

            // 偏移向量
            Vector3 offset = direction * -range;

            // 计算范围内的最靠近终点的点
            Vector3 closestPoint = p3 + offset;

            return closestPoint;
        }

    }
}
