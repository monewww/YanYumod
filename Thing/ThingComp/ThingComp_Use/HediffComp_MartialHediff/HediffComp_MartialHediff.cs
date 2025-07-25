using RimWorld;
using System;
using Verse;



namespace YanYu
{
    public class HediffComp_MartialHediff : HediffComp
    {
        public HediffCompProperties_MartialHediff Props
        {
            get
            {
                return (HediffCompProperties_MartialHediff)this.props;
            }
        }
        public Pawn GetPawn
        {
            get
            {
                return this.Pawn;
            }
        }

        public bool IsUniqueMartial
        {
            get
            {
                return Props.uniqueMartial;
            }
        }

        public override void CompPostPostRemoved()
        {   
            base.CompPostPostRemoved();
            //待定
            try
            {

                if (Props.YanYu_Martials != null)
                {
                    foreach (var martial in Props.YanYu_Martials)
                    {
                        if (GetPawn.abilities.GetAbility(martial) != null)
                        {
                            GetPawn.abilities.RemoveAbility(martial);
                        }
                    }
                }

                //if (Props.YanYu_PassiveEffects != null)
                //{
                //    foreach (var passiveEffect in Props.YanYu_PassiveEffects)
                //    {
                //        if (GetPawn.TryGetComp<ThingComp_UseMartialbook>() is ThingComp_UseMartialbook comp)
                //        {
                //            comp.RemovePassiveEffect(passiveEffect);
                //        }
                //    }
                //}

            }
            catch (Exception e) { }
        }
    }
}
