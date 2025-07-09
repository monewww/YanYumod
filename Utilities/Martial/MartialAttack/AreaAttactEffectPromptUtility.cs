

using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace YanYu.Utilities
{
    public class AreaAttactEffectPromptUtility
    {
        public static void DrawEllipticalFieldEdges(
            Pawn attacker,
            LocalTargetInfo target,
            float radiusX,
            float radiusZ,
            bool selfCenter = true,
            bool halfElliptical = true,
            Color? color = null
        )
        {
            var map = attacker.Map;
            Vector3 centerVec = selfCenter ? attacker.Position.ToVector3Shifted() : target.CenterVector3;
            IntVec3 center = centerVec.ToIntVec3();

            Vector3 dir = (target.Cell.ToVector3Shifted() - attacker.Position.ToVector3Shifted());
            dir.y = 0f; // 确保只在 XZ 平面上
            dir.Normalize(); // 转换为单位向量
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3Shifted();

            Vector3 right = new Vector3(dir.z, 0f, -dir.x);

            int cellRadX = Mathf.CeilToInt(radiusX);
            int cellRadZ = Mathf.CeilToInt(radiusZ);

            var cellsInEllipse = new List<IntVec3>();
            int cellradmax = Mathf.Max(cellRadZ, cellRadX);
            for (int dx = -cellradmax; dx <= cellradmax; dx++)
            {
                for (int dz = -cellradmax; dz <= cellradmax; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map))
                        continue;

                    Vector3 offset = (cell.ToVector3Shifted() - centerVec);
                    float localX = Vector3.Dot(offset, dir);
                    float localZ = Vector3.Dot(offset, right);

                    if (halfElliptical && localX < 0)
                        continue;

                    float ellipseValue = (localX * localX) / (radiusX * radiusX) + (localZ * localZ) / (radiusZ * radiusZ);
                    if (ellipseValue <= 1f)
                    {
                        cellsInEllipse.Add(cell);
                    }
                }
            }

            // 用红色或传入颜色画边缘
            GenDraw.DrawFieldEdges(cellsInEllipse, color ?? Color.red);
        }

        //类比DoCircleDamage
        public static void DrawCircleFeildEdge(
            Pawn attacker,
            LocalTargetInfo target,
            float radius,
            bool targetCenter = false,
            IntVec3 center = default(IntVec3),
            float angle = 360f,
            Color? color = null
        )
        {
            var map = attacker.Map;
            Vector3 centerVec = targetCenter ? target.Cell.ToVector3Shifted() : attacker.Position.ToVector3Shifted();
            if (center == default(IntVec3)) center = centerVec.ToIntVec3();
            Vector3 dir = (target.Cell.ToVector3Shifted() - attacker.Position.ToVector3Shifted());
            dir.y = 0f; // 确保只在 XZ 平面上
            dir.Normalize(); // 转换为单位向量
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3Shifted();
            Vector3 right = new Vector3(dir.z, 0f, -dir.x);
            var cellsInCircle = new List<IntVec3>();
            int cellRad = Mathf.CeilToInt(radius);
            for (int dx = -cellRad; dx <= cellRad; dx++)
            {
                for (int dz = -cellRad; dz <= cellRad; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map))
                        continue;
                    Vector3 offset = (cell.ToVector3Shifted() - center.ToVector3Shifted());
                    float localX = Vector3.Dot(offset, dir);
                    float localZ = Vector3.Dot(offset, right);
                    if ((localX * localX) / (radius * radius) + (localZ * localZ) / (radius * radius) <= 1f)
                    {
                        cellsInCircle.Add(cell);
                    }
                }
            }
            // 用红色或传入颜色画边缘
            GenDraw.DrawFieldEdges(cellsInCircle, color ?? Color.red);
        }


        public static void DrawDiamondFeildEdge(
            Pawn attacker,
            LocalTargetInfo target,
            float radiusX,
            float radiusZ,
            bool targetCenter = false,
            IntVec3 center = default(IntVec3),
            bool halfDiamond = false,
            Color? color = null
            )
        {
            var map = attacker.Map;
            Vector3 centerVec = targetCenter ? target.Cell.ToVector3Shifted() : attacker.Position.ToVector3Shifted();
            if (center == default(IntVec3)) center = centerVec.ToIntVec3();
            Vector3 dir = (target.Cell.ToVector3Shifted() - attacker.Position.ToVector3Shifted());
            dir.y = 0f; // 确保只在 XZ 平面上
            dir.Normalize(); // 转换为单位向量
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3Shifted();
            Vector3 right = new Vector3(dir.z, 0f, -dir.x);
            var cellsInDiamond = new List<IntVec3>();
            int cellRadX = Mathf.CeilToInt(radiusX);
            int cellRadZ = Mathf.CeilToInt(radiusZ);
            int cellRadMax = Mathf.Max(cellRadZ, cellRadX);
            for (int dx = -cellRadMax; dx <= cellRadMax; dx++)
            {
                for (int dz = -cellRadMax; dz <= cellRadMax; dz++)
                {
                    IntVec3 cell = new IntVec3(center.x + dx, 0, center.z + dz);
                    if (!cell.InBounds(map))
                        continue;
                    Vector3 offset = (cell.ToVector3Shifted() - center.ToVector3Shifted());
                    float localX = Vector3.Dot(offset, dir);
                    if (halfDiamond && localX < 0) continue; // 如果是半菱形，且在负方向则跳过
                    float localZ = Vector3.Dot(offset, right);
                    if (Mathf.Abs(localX) / radiusX + Mathf.Abs(localZ) / radiusZ <= 1f)
                    {
                        cellsInDiamond.Add(cell);
                    }
                }
            }
            // 用红色或传入颜色画边缘
            GenDraw.DrawFieldEdges(cellsInDiamond, color ?? Color.red);

        }
    }
}
