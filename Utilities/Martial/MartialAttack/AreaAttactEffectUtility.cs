
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace YanYu.Utilities
{
  
    public class AreaAttactEffectUtility
    {
        public static void DoEffectRotation(
            Pawn attacker,
            LocalTargetInfo target,
            FleckCreationData fleckData,
            float rotationAngle = 0f,   
            float offsetRight = 0f,
            float offsetForward = 0f
        )
        {
            IntVec3 dir = (target.Cell - attacker.Position);
            dir.y = 0;
            float baseAngle = dir.ToVector3().AngleFlat();
            float angleRad = baseAngle * Mathf.Deg2Rad;

            Vector3 forward = new Vector3(Mathf.Sin(angleRad), 0f, Mathf.Cos(angleRad));
            Vector3 right = new Vector3(Mathf.Cos(angleRad), 0f, -Mathf.Sin(angleRad));

            Vector3 offset = right * offsetRight + forward * offsetForward;
            fleckData.rotation = baseAngle + rotationAngle*Mathf.Rad2Deg;
            fleckData.spawnPosition = attacker.Position.ToVector3() + offset;
            attacker.Map.flecks.CreateFleck(fleckData);
        }
    }
}
