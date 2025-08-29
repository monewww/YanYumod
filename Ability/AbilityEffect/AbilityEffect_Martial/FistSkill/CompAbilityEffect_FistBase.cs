
using RimWorld;
using System.Linq;
using Verse;


namespace YanYu
{
    public class CompAbilityEffect_FistBase : CompAbilityEffect
    {
        public CompProperties_AbilitySwordBase PropsSword => (CompProperties_AbilitySwordBase)props;
        public override bool GizmoDisabled(out string reason)
        {
            if (!PawnHasSword(parent.pawn))
            {
                reason = "MustEquipSword".Translate();
                return true;
            }

            return base.GizmoDisabled(out reason);
        }

        // 判断装备的名字是否包含"sword"关键字
        private bool PawnHasSword(Pawn pawn)
        {
            if (pawn?.equipment?.Primary == null) return false;

            string equipedWeapom = pawn.equipment.Primary.def.label;
            return YanYu_Mod.settings.glovesList.Contains(equipedWeapom);

        }
    }
}
