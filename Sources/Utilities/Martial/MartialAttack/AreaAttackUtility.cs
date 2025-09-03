
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace YanYu
{
    public class AreaAttackUtility
    {
        //椭圆
        public static void DoEllipticalDamage(
            Pawn attacker,
            LocalTargetInfo target,
            float radiusX,
            float radiusZ,
            float damageAmount,
            DamageDef damageDef = null,
            IntVec3 center = default(IntVec3),
            float startAngle = 0f,
            float endAngle = 360f,
            float armorPenetration = 1.0f,
            SoundDef hitSound = null,
            Thing instigator = null,
            List<Thing> ignoredThings = null
            )
        {
            if (damageDef == null)
            {
                damageDef = DamageDefOf.Cut;
            }
            Map map = attacker.Map;

            // 方向向量（XZ 平面单位向量）
            Vector3 dir = (target.Cell.ToVector3Shifted() - attacker.Position.ToVector3Shifted());
            dir.y = 0f; // 确保只在 XZ 平面上
            dir = dir.normalized;
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3Shifted();

            // 构建本地坐标系：forward = dir, right = dir.Cross()
            Vector3 right = new Vector3(dir.z, 0f, -dir.x); // 90° 右手

            int cellRadX = Mathf.CeilToInt(radiusX);
            int cellRadZ = Mathf.CeilToInt(radiusZ);

            var damaged = new HashSet<Pawn>();
            int maxCellRad = Mathf.Max(cellRadZ, cellRadX);
            for (int dx = -maxCellRad; dx <= maxCellRad; dx++)
            {
                for (int dz = -maxCellRad; dz <= maxCellRad; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map)) continue;

                    // 将 cell → 本地坐标 (forward, right)
                    Vector3 offset = (cell.ToVector3Shifted() - center.ToVector3Shifted());
                    //角度判定
                    float angleToTarget = Vector3.Angle(offset, dir);
                    if (angleToTarget < startAngle  || angleToTarget > endAngle ) continue;
                    float localX = Vector3.Dot(offset, dir);   // 前后
                    float localZ = Vector3.Dot(offset, right); // 左右


                    // 椭圆判定公式 (x/rx)^2 + (z/rz)^2 ≤ 1
                    if ((localX * localX) / (radiusX * radiusX) + (localZ * localZ) / (radiusZ * radiusZ) > 1f)
                        continue;

                    // 伤害 Pawns
                    List<Thing> thingList = cell.GetThingList(map);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        Thing thing = thingList[i];
                        if (thing is Pawn pawn && !ignoredThings.Contains(pawn) && !damaged.Contains(pawn))
                        {
                            var dinfo = new DamageInfo(damageDef, damageAmount, armorPenetration, -1, instigator ?? attacker);
                            pawn.TakeDamage(dinfo);
                            damaged.Add(pawn);
                        }
                    }
                }
            }
            // 播放音效
            hitSound?.PlayOneShot(new TargetInfo(center, map));
        }

        //圆
        public static void DoCircleDamage(
            Pawn attacker,
            LocalTargetInfo target,
            float radius,
            float damageAmount,
            float armorPenetration = 1.0f,
            DamageDef damageDef = null,
            bool targetCenter = false,
            IntVec3 center = default(IntVec3),
            float angle = 360f,
            SoundDef soundDef = null,
            Thing instigator = null,
            List<Thing> ignoredThings = null
            )
        {
            if (damageDef == null)
            {
                damageDef = DamageDefOf.Cut;
            }
            Map map = attacker.Map;
            if (center == default(IntVec3))
            {
                if (targetCenter)
                {
                    center = target.Cell;
                }
                else
                {
                    center = attacker.Position;
                }
            }
            angle = angle > 360f ? 360f : angle < 0f ? 0f : angle; // 限制角度范围
            int cellRad = Mathf.CeilToInt(radius);
            var damaged = new HashSet<Pawn>();
            for (int dx = -cellRad; dx <= cellRad; dx++)
            {
                for (int dz = -cellRad; dz <= cellRad; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map)) continue;
                    // 距离判定
                    float dist = (cell - center).LengthHorizontalSquared;
                    if (dist > radius * radius) continue;
                    // 角度判定
                    Vector3 offset = (cell.ToVector3Shifted() - target.Cell.ToVector3Shifted());
                    float angleToTarget = Vector3.Angle(offset, attacker.DrawPos - center.ToVector3Shifted());
                    if (angleToTarget > angle / 2f) continue;
                    // 伤害 Pawns
                    List<Thing> thingList = cell.GetThingList(map);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        Thing thing = thingList[i];
                        if (thing is Pawn pawn && !ignoredThings.Contains(pawn) && !damaged.Contains(pawn))
                        {
                            var dinfo = new DamageInfo(damageDef, damageAmount, armorPenetration, -1, instigator ?? attacker);
                            pawn.TakeDamage(dinfo);
                            damaged.Add(pawn);
                        }
                    }
                }
            }
            // 播放音效
            soundDef?.PlayOneShot(new TargetInfo(center, map));

        }

        //菱形
        public static void DoDiamondDamage(
            Pawn attacker,
            LocalTargetInfo target,
            float radiusX,
            float radiusZ,
            float damageAmount,
            DamageDef damageDef = null,
            bool targetCenter = false,
            IntVec3 center = default(IntVec3),
            bool halfDiamond = true,
            float armorPenetration = 1.0f,
            SoundDef hitSound = null,
            Thing instigator = null,
            List<Thing> ignoredThings = null
            )
        {
            if (damageDef == null)
            {
                damageDef = DamageDefOf.Cut;
            }
            Map map = attacker.Map;
            if (center == default(IntVec3))
            {
                if (targetCenter)
                {
                    center = target.Cell;
                }
                else
                {
                    center = attacker.Position;
                }
            }
            int cellRadX = Mathf.CeilToInt(radiusX);
            int cellRadZ = Mathf.CeilToInt(radiusZ);
            var damaged = new HashSet<Pawn>();
            int maxCellRad = Mathf.Max(cellRadZ, cellRadX);
            for (int dx = -maxCellRad; dx <= maxCellRad; dx++)
            {
                for (int dz = -maxCellRad; dz <= maxCellRad; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map)) continue;
                    // 将 cell → 本地坐标 (forward, right)
                    Vector3 offset = (cell.ToVector3Shifted() - center.ToVector3Shifted());
                    float localX = Mathf.Abs(offset.x); // 前后
                    float localZ = Mathf.Abs(offset.z); // 左右
                    // 半菱形：只取面向目标的半边
                    if (halfDiamond && localX < 0) continue;
                    // 菱形判定公式 |x/rx| + |z/rz| ≤ 1
                    if ((localX / radiusX) + (localZ / radiusZ) > 1f)
                        continue;
                    // 伤害 Pawns
                    List<Thing> thingList = cell.GetThingList(map);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        Thing thing = thingList[i];
                        if (thing is Pawn pawn && !ignoredThings.Contains(pawn) && !damaged.Contains(pawn))
                        {
                            var dinfo = new DamageInfo(damageDef, damageAmount, armorPenetration, -1, instigator ?? attacker);
                            pawn.TakeDamage(dinfo);
                            damaged.Add(pawn);
                        }
                    }
                }
            }
            // 播放音效
            hitSound?.PlayOneShot(new TargetInfo(center, map));

        }
    }

}
