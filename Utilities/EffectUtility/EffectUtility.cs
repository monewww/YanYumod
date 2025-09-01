using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace YanYu
{
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
                this.scaleRate = ((endScale == default ? Vector3.one : endScale) - this.curvedScale) / scaleUpTime / 60 ;
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
                if (elapsedTime< scaleUpTime)
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
