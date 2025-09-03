using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace YanYu
{
    public class GoAndTalkWithLeader:JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            // 移动到目标
            Toil goToTarget = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            yield return goToTarget;

            // 到达后弹窗
            Toil talk = new Toil();
            talk.initAction = () =>
            {
                Pawn pawn = this.pawn;
                Pawn target = (Pawn)this.TargetThingA;
                Find.WindowStack.Add(new Dialog_QuestInteraction(pawn, target));
            };
            yield return talk;
        }
    }
}
