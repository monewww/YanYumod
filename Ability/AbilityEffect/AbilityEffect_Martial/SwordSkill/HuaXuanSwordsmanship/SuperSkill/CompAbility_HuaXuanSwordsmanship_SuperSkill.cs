

using UnityEngine;
using Verse;
using YanYu.Utilities;

namespace YanYu
{
    public class CompAbility_HuaXuanSwordsmanship_SuperSkill : CompAbilityEffect_SwordBase
    {
        public new CompProperties_HuaXuanSwordsmanship_SuperSkill Props => (CompProperties_HuaXuanSwordsmanship_SuperSkill)props;
        public Pawn GetPawn => this.parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            
        }
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);

            IntVec3 center = target.Cell;

            AreaAttactEffectPromptUtility.DrawCircleFeildEdge(
                GetPawn,
                target,
                8f,
                center: center,
                color: Color.HSVToRGB(0.61f, 0.7f, 0.9f)
            );

            AreaAttactEffectPromptUtility.DrawEllipticalFieldEdges(
                GetPawn,
                target,
                4f,
                0.7f,
                center: target.Cell,
                halfElliptical: false,
                color: Color.HSVToRGB(0.61f, 0.7f, 0.9f)
            );
        }


    }
}
