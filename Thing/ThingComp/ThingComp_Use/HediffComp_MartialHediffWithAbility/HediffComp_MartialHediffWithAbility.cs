using RimWorld;
using System;
using Verse;



namespace YanYu
{
    public class HediffComp_MartialHediffWithAbility : HediffComp
    {
        public HediffCompProperties_MartialHediffWithAbility Props
        {
            get
            {
                return (HediffCompProperties_MartialHediffWithAbility)this.props;
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
                     
            }
            catch (Exception e) { }
        }
    }
}
