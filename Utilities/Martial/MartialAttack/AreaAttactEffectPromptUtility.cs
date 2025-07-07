

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
            Vector3 centerVec = selfCenter ? attacker.DrawPos : target.CenterVector3;
            IntVec3 center = centerVec.ToIntVec3();

            Vector3 dir = (target.CenterVector3 - attacker.DrawPos);
            dir.y= 0f; // 确保只在 XZ 平面上
            dir.Normalize(); // 转换为单位向量
            if (dir == Vector3.zero) dir = attacker.Rotation.FacingCell.ToVector3();

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

    }
}
