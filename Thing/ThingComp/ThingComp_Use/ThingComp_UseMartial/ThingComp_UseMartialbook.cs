


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
            if (isGetAbility)
            {
                parent.Destroy();
            }
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

            /* ───────────── 1. 检查并卸载旧武学 ───────────── */
            var existingMartialHediff =
                user.health.hediffSet.hediffs.FirstOrDefault(h => h.def.defName.StartsWith("YanYu_Martial"));

            if (existingMartialHediff != null)
            {
                bool unique = existingMartialHediff.TryGetComp<HediffComp_MartialHediff>()?.IsUniqueMartial ?? false;
                if (unique)
                {
                    Messages.Message("YanYu_Martialbook_AlreadyHasUniqueMartial", user,
                        MessageTypeDefOf.NeutralEvent, false);
                    return;
                }

                /* 移除旧能力 */
                var oldComp = existingMartialHediff.TryGetComp<HediffComp_MartialHediff>();
                if (oldComp?.Props.YanYu_Martials != null)
                {
                    foreach (var abl in oldComp.Props.YanYu_Martials)
                        user.abilities.RemoveAbility(abl);
                }

                /* 删除旧 Hediff */
                user.health.RemoveHediff(existingMartialHediff);
            }

            HediffDef targetDef = Props.giveHediffDef;
            if (targetDef == null)
            {
                Log.Error("ThingComp_UseMartialbook targetHediffDef is null");
                return;
            }

            var newProps = targetDef.CompProps<HediffCompProperties_MartialHediff>();

            if (newProps.YanYu_Martials != null)
            {
                foreach (var abl in newProps.YanYu_Martials)
                    user.abilities.GainAbility(abl);
            }
            Hediff newHediff = HediffMaker.MakeHediff(targetDef, user);
            user.health.AddHediff(newHediff);

            isGetAbility = true;
        }


        public void SendLetter(Pawn user, bool isGetAbility)
        {
            //正常情况
            string title = "UseBook_LetterTilie".Translate();
            string text = "UseBook_LetterText_PawnGetNewLevel".Translate();

            //学习了新的技能
            if (isGetAbility)
            {
                text += "UseBook_LetterText_PawnGetAbility".Translate();
            }

            Find.LetterStack.ReceiveLetter(title, text, LetterDefOf.PositiveEvent, user, null, null, null, null, 0, true);
        }
    }
}
