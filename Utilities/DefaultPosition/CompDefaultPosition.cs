using Verse;
using Verse.AI;
using RimWorld;


namespace YanYu
{
    public class CompDefaultPosition : ThingComp
    {
        public CompProperties_DefaultPosition Props => (CompProperties_DefaultPosition)this.props;
        public IntVec3? defaultPosition;
        private int nextCheckTick = 0;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!defaultPosition.HasValue && parent is Pawn pawn)
            {
                defaultPosition = pawn.Position;
            }
            resetNextTick();
        }
        public override void CompTick()
        {
            base.CompTick();
            if (!(parent is Pawn pawn)) return;

            // 这些状态下不要干活，避免和 UI/Overlay 或换图过程打架
            if (!pawn.Spawned || pawn.Map == null || pawn.Dead || pawn.Downed || pawn.Suspended)
                return;

            // jobs/PathEndMode 等对象有时会在极少数帧里未就绪，先判空
            if (Find.TickManager.TicksGame <= nextCheckTick)
                return;

            try
            {
                if (defaultPosition.HasValue && pawn.Position != defaultPosition.Value)
                {
                    // 不要在同一 tick 里重复发 Goto
                    if (pawn.jobs != null && (pawn.jobs.curJob == null || pawn.jobs.curJob.def != JobDefOf.Goto))
                    {
                        // 再次确认能到达，TargetInfo 用 LocalTargetInfo 包一层更稳妥
                        var target = (LocalTargetInfo)defaultPosition.Value;
                        if (pawn.CanReach(target, PathEndMode.OnCell, Danger.Deadly))
                        {
                            Job job = JobMaker.MakeJob(JobDefOf.Goto, target);
                            // OrderedJob 可能因为各种原因失败，判一下返回值
                            pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                        else
                        {
                            // Name 可能为 null，用 LabelShortCap 更安全
                            Log.Message($"{pawn.LabelShortCap} cannot reach default position {defaultPosition.Value}");
                        }
                    }
                }
            }
            finally
            {
                resetNextTick(); // 无论如何重置下次检查，防止连环重试
            }

        }
        private void resetNextTick()
        {
            nextCheckTick = Find.TickManager.TicksGame + Rand.RangeInclusive(30*60,60*60);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            IntVec3 tempPos = defaultPosition ?? IntVec3.Invalid;
            Scribe_Values.Look(ref tempPos, "defaultPos", IntVec3.Invalid);
            if (tempPos != IntVec3.Invalid)
            {
                defaultPosition = tempPos;
            }
            Scribe_Values.Look(ref nextCheckTick, "nextCheckTick", 0);
        }
    }
}