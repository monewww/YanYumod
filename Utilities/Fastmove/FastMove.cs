using Verse;
using UnityEngine;
using RimWorld;
using System.Collections;
using HarmonyLib;
using System.Collections.Generic;

namespace YanYu
{
    public class FastMove
    {
        public static void MoveToCell(Pawn pawn, IntVec3 targetCell, float moveDuration)
        {
            if (!pawn.Spawned || !targetCell.IsValid || pawn.Destroyed)
                return;

            var map = pawn.Map;
            if (map == null)
                return;

            pawn.Map.GetComponent<FastMoveController>().StartFastMove(pawn, targetCell, moveDuration);
        }
    }

    public class FastMoveController : MapComponent
    {
        private struct MovingPawn
        {
            public Pawn pawn;
            public Vector3 startPos;
            public Vector3 endPos;
            public float startTime;
            public float duration;
        }

        private List<MovingPawn> movingPawns = new List<MovingPawn>();

        public FastMoveController(Map map) : base(map) { }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            if (movingPawns.Count == 0) return;

            float currentTime = Time.realtimeSinceStartup;
            for (int i = movingPawns.Count - 1; i >= 0; i--)
            {
                var mp = movingPawns[i];
                float t = (currentTime - mp.startTime) / mp.duration;
                if (t >= 1f)
                {
                    mp.pawn.Position = mp.endPos.ToIntVec3();
                    mp.pawn.Drawer.tweener.ResetTweenedPosToRoot();
                    movingPawns.RemoveAt(i);
                }
                else
                {
                    Vector3 pos = Vector3.Lerp(mp.startPos, mp.endPos, t);
                    Traverse.Create(mp.pawn.Drawer.tweener).Field("tweenedPos").SetValue(pos);
                    mp.pawn.Rotation = RotFromVector((mp.endPos - mp.startPos).normalized);
                }
            }
        }

        public void StartFastMove(Pawn pawn, IntVec3 cell, float duration)
        {
            var entry = new MovingPawn()
            {
                pawn = pawn,
                startPos = pawn.DrawPos,
                endPos = cell.ToVector3Shifted(),
                startTime = Time.realtimeSinceStartup,
                duration = duration
            };
            movingPawns.Add(entry);
        }

        private Rot4 RotFromVector(Vector3 dir)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
                return dir.x > 0 ? Rot4.East : Rot4.West;
            else
                return dir.z > 0 ? Rot4.North : Rot4.South;
        }
    }
}
