using Verse;
using RimWorld;
using UnityEngine;

namespace YanYu
{
    public class QuestGiver : DefModExtension
    {

    }

    public class Dialog_QuestInteraction : Window
    {
        private Pawn questGiver;
        private Pawn playerPawn;

        public Dialog_QuestInteraction(Pawn playerPawn, Pawn questGiver)
        {
            this.playerPawn = playerPawn;
            this.questGiver = questGiver;
            this.doCloseButton = true;
            this.closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Widgets.Label(new Rect(0, 0, inRect.width, 30), $"你正在与 {questGiver.LabelShort} 交谈。");

            if (Widgets.ButtonText(new Rect(0, 40, inRect.width, 30), "接取任务"))
            {
                Messages.Message("你接取了任务！", MessageTypeDefOf.TaskCompletion);
                this.Close();
            }

            if (Widgets.ButtonText(new Rect(0, 80, inRect.width, 30), "购买物资"))
            {
                Messages.Message("打开商店界面！", MessageTypeDefOf.TaskCompletion);
                this.Close();
            }
        }

    }
}
