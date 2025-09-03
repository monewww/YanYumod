

using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace YanYu
{
    public class AreaAttactEffectPromptUtility
    {
        public static void DrawEllipticalFieldEdges(
            Pawn attacker,
            IntVec3 target,
            float radiusX,
            float radiusZ,
            IntVec3 center = default(IntVec3),
            float startAngle = 0f,
            float endAngle = 360f,
            Color? color = null
        )
        {
            var map = attacker.Map;
            Vector3 centerVec = center == default(IntVec3) ? attacker.Position.ToVector3Shifted() : center.ToVector3Shifted();
            Vector3 dir = (target.ToVector3Shifted() - attacker.Position.ToVector3Shifted());
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
                    //角度判定
                    float angleToTarget = Vector3.Angle(offset, dir);
                    if (startAngle>0 && (angleToTarget < startAngle || angleToTarget > endAngle)) continue;
                    if (startAngle<0 && (angleToTarget < 360+startAngle && angleToTarget > endAngle)) continue;

                    float localX = Vector3.Dot(offset, dir);
                    float localZ = Vector3.Dot(offset, right);

                    float ellipseValue = (localX * localX) / (radiusX * radiusX) + (localZ * localZ) / (radiusZ * radiusZ);
                    if (ellipseValue <= 1f)
                    {
                        cellsInEllipse.Add(cell);
                    }
                }
            }

            GenDraw.DrawFieldEdges(cellsInEllipse, color ?? Color.red);
        }

        public static void DrawCircleFeildEdge(
            Pawn attacker,
            LocalTargetInfo target,
            float radius,
            IntVec3 center = default(IntVec3),
            float angle = 360f,
            Color? color = null
        )
        {
            var map = attacker.Map;
            Vector3 centerVec = center == default(IntVec3) ? attacker.Position.ToVector3Shifted() : center.ToVector3Shifted();
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
            GenDraw.DrawFieldEdges(cellsInCircle, color ?? Color.red);
        }


        public static void DrawDiamondFeildEdge(
            Pawn attacker,
            LocalTargetInfo target,
            float radiusX,
            float radiusZ,
            IntVec3 center = default(IntVec3),
            bool halfDiamond = false,
            Color? color = null
            )
        {
            var map = attacker.Map;
            Vector3 centerVec = center == default(IntVec3) ? attacker.Position.ToVector3Shifted() : center.ToVector3Shifted();
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
            GenDraw.DrawFieldEdges(cellsInDiamond, color ?? Color.red);

        }
    }
}
