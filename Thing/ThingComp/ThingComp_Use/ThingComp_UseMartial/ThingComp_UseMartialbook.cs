



using RimWorld;
using Verse;
using Verse.Sound;

namespace YanYu
{
    public class ThingComp_UseMartialbook: CompUseEffect
    {
        public ThingCompProperties_UseMartialbook Props
        {
            get
            {
                return (ThingCompProperties_UseMartialbook)this.props;
            }
        }

        public override void DoEffect(Pawn user)
        {
            //特效
            ExtraEffect(user);

            UseEffect(user, out bool isGetAbility);

            SendLetter(user, isGetAbility);
        }

        private void ExtraEffect(Pawn user)
        {
            FleckMaker.Static(user.Position, user.Map, FleckDefOf.PsycastAreaEffect, 0.1f);
            SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(user.Position, user.Map, false));
        }

        private void UseEffect(Pawn user, out bool isGetAbility)
        {
            isGetAbility = false;
            var Props = this.Props as ThingCompProperties_UseMartialbook;
            if (Props == null)
            {
                Log.Error("ThingComp_UseMartialbook Props is null");
                return;
            }

            var existingMartialHediff = user.health.hediffSet.hediffs.FirstOrDefault(h => h.def.defName.StartsWith("YanYu_"));

            if (existingMartialHediff != null)
            {
                bool exsistingMartialIsUnique = existingMartialHediff.TryGetComp<HediffComp_MartialHediffWithAbility>()?.IsUniqueMartial ?? false;
                if (exsistingMartialIsUnique)
                {
                    Messages.Message("YanYu_Martialbook_AlreadyHasUniqueMartial", user, MessageTypeDefOf.NeutralEvent, false);
                    return;
                }
                //删除已有武学和hidiff
                foreach (var ability in existingMartialHediff.TryGetComp<HediffComp_MartialHediffWithAbility>().Props.YanYu_Martials)
                {
                    if (user.abilities.GetAbility(ability) != null)
                    {
                        user.abilities.RemoveAbility(ability);
                    }
                }
                user.health.RemoveHediff(existingMartialHediff);
            }
            //获取目标HediffDef
            HediffDef targetHediffDef = Props.giveHediffDef;
            if (targetHediffDef == null)
            {
                Log.Error("ThingComp_UseMartialbook targetHediffDef is null");
                return;
            }
            //添加新的武学
            HediffCompProperties_MartialHediffWithAbility hediffcomp = targetHediffDef.CompProps<HediffCompProperties_MartialHediffWithAbility>();
            foreach (var martial in hediffcomp.YanYu_Martials)
            {
                user.abilities.GainAbility(martial);
            }
            //添加新的Hediff
            Hediff newHediff = HediffMaker.MakeHediff(targetHediffDef, user);
            user.health.AddHediff(newHediff);


            isGetAbility = true;



        }

        public void SendLetter(Pawn user, bool isGetAbility)
        {
            //正常情况
            string title = "UseBook_LetterTilie";
            string text = "UseBook_LetterText_PawnGetNewLevel";

            //学习了新的技能
            if (isGetAbility)
            {
                text += "UseBook_LetterText_PawnGetAbility";
            }

            Find.LetterStack.ReceiveLetter(title, text, LetterDefOf.PositiveEvent, user, null, null, null, null, 0, true);
        }
    }
}
