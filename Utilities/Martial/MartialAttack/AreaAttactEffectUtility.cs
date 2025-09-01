
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace YanYu
{
  
    public class AreaAttactEffectUtility
    {
        public static void DoEffect(
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
            float baseAngle = dir.ToVector3Shifted().AngleFlat();
            float angleRad = baseAngle * Mathf.Deg2Rad;

            Vector3 forward = new Vector3(Mathf.Sin(angleRad), 0f, Mathf.Cos(angleRad));
            Vector3 right = new Vector3(Mathf.Cos(angleRad), 0f, -Mathf.Sin(angleRad));

            Vector3 offset = right * offsetRight + forward * offsetForward;
            fleckData.rotation = baseAngle + rotationAngle*Mathf.Rad2Deg;
            fleckData.spawnPosition = attacker.Position.ToVector3Shifted() + offset;
            attacker.Map.flecks.CreateFleck(fleckData);
        }

    }

    public static class MoteUtil
    {
        public class Mote_MoveGrow : Mote
        {
            private Vector3 endPos;
            private Vector3 direction;
            private float currentSpeed;
            private SimpleCurve acceleration;
            private float rotationAcceleration;
            private Vector3 scaleRate;
            private float duration;
            private float elapsedTime;
            private float scaleUpTime;

            public void Setup(
                Vector3 startPos,
                Vector3 endPos,
                Vector3 startScale = default,
                Vector3 endScale = default,
                float startSpeed = 1f,
                SimpleCurve acc = default,
                float scaleUpTime = 1f,
                float startRot = 0f,
                float rotSpeed = 0f,
                float lastTime = 2f)
            {
                this.exactPosition = startPos;
                this.endPos = endPos;
                this.direction = (endPos - startPos).normalized;
                this.curvedScale = (startScale == default ? Vector3.one : startScale);
                this.scaleUpTime = scaleUpTime;
                this.scaleRate = ((endScale == default ? Vector3.one : endScale) - this.curvedScale) / scaleUpTime / 60;
                this.exactRotation = startRot;
                this.currentSpeed = startSpeed;
                this.acceleration = acc;
                this.rotationRate = rotSpeed;
                this.duration = lastTime;
                this.elapsedTime = 0f;

                Log.Message($"ScaleRate:{scaleRate}");
            }
            protected override void TimeInterval(float deltaTime)
            {
                base.TimeInterval(deltaTime);

                elapsedTime += deltaTime;
                if (elapsedTime >= duration)
                {
                    Destroy();
                    return;
                }

                // --- 位置更新 (速度 + 加速度) ---
                currentSpeed += acceleration.Evaluate(elapsedTime) * deltaTime;
                exactPosition += direction * currentSpeed * deltaTime;
                //Log.Message($"dir:{direction},exactPosition:{exactPosition}");

                // --- 缩放更新 (线性插值) ---
                if (elapsedTime < scaleUpTime)
                {
                    curvedScale = curvedScale + scaleRate;
                }

                // --- 旋转更新 ---
                rotationRate += rotationAcceleration * deltaTime;
                exactRotation += rotationRate * deltaTime;
            }
        }


    }
}
