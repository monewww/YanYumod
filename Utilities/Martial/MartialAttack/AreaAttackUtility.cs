
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace YanYu.Utilities
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
            bool selfCenter = true,
            bool halfElliptical = true,
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
            Vector3 centerVec = selfCenter ? attacker.DrawPos : target.CenterVector3;
            IntVec3 center = centerVec.ToIntVec3();

            // 方向向量（XZ 平面单位向量）
            Vector3 dir = (target.CenterVector3 - attacker.DrawPos);
            dir.y = 0f; // 确保只在 XZ 平面上
            dir = dir.normalized;
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3();

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
                    Vector3 offset = (cell.ToVector3Shifted() - centerVec);
                    float localX = Vector3.Dot(offset, dir);   // 前后
                    float localZ = Vector3.Dot(offset, right); // 左右

                    // 半椭圆：只取面向目标的半边
                    if (halfElliptical && localX < 0) continue;

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
        }
    }
}
