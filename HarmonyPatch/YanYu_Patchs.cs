
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace YanYu
{
    [HarmonyPatch(typeof(DamageWorker_AddInjury), "ApplyToPawn")]
    public static class Patch_DamageWorker_AttackTrigger
    {
        [HarmonyPrefix]
        public static void Prefix(ref DamageInfo dinfo, Pawn pawn)
        {
            //Log.Message($"Institator: {dinfo.Instigator?.LabelShort}, Target: {pawn.LabelShort}");
            if (!(dinfo.Instigator is Pawn attacker)) return;
            //总觉得所有pawn都要检测的话太消耗资源，只管同派系的吧
            if (attacker.Faction != Faction.OfPlayer) return;
            //Log.Message("going on");
            var hediffs = attacker.health.hediffSet.hediffs.OfType<HediffWithComps>().ToList();

            foreach (var hediff in hediffs)
            {
                foreach (var comp in hediff.comps.OfType<HediffComp_AttackTrigger>())
                {
                    Log.Message($"[舞殇剑势] {attacker.LabelShort} 触发了剑势被动效果。");
                    comp.doEffect(ref dinfo, pawn);
                }
            }


        }
    }



    [HarmonyPatch(typeof(ApparelGraphicRecordGetter), nameof(ApparelGraphicRecordGetter.TryGetGraphicApparel))]
    public static class Patch_GenderedApparelPath_Prefix
    {
        // TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, bool forStatue, out ApparelGraphicRecord rec)
        static bool Prefix(Apparel apparel, BodyTypeDef bodyType, bool forStatue, ref bool __result, ref ApparelGraphicRecord rec)
        {
            // 仅处理我们打标的衣服
            if (apparel == null || bodyType == null) return true;
            if (apparel.Wearer == null) return true;
            if (apparel.GetComp<CompRenameByGender>() == null) return true;

            string basePath = apparel.def?.apparel?.wornGraphicPath;
            if (basePath.NullOrEmpty()) return true;

            string genderStr = apparel.Wearer.gender == Gender.Female ? "Female"
                              : apparel.Wearer.gender == Gender.Male ? "Male"
                              : "None";

            // 候选路径前缀列表（不含 _south/_east/_north）
            string[] candidates = new[]
            {
                $"{basePath}_{genderStr}_{bodyType.defName}",
                $"{basePath}_{genderStr}",
                $"{basePath}_{bodyType.defName}",
                $"{basePath}"
            };

            foreach (var prefix in candidates)
            {
                // 先探测 south 是否存在，避免报错
                var south = ContentFinder<Texture2D>.Get(prefix + "_south", reportFailure: false);
                if (south == null) continue;

                // 找到了；构建 Graphic 并接管
                var shader = ShaderDatabase.CutoutComplex;
                var col1 = apparel.DrawColor;
                var col2 = apparel.DrawColorTwo;
                var g = GraphicDatabase.Get<Graphic_Multi>(prefix, shader, Vector2.one, col1, col2);
                rec = new ApparelGraphicRecord(g, apparel);
                __result = true;

                // 记录下究竟匹配了哪个前缀，便于调试
                Log.Message($"[YanYu] Matched apparel graphic: {prefix} (gender={genderStr}, bodyType={bodyType.defName})");

                return false; // 阻止后续原逻辑与其他 mod 的补丁
            }

            Log.Message($"[YanYu] No gender/bodyType path matched for: base={basePath}, gender={genderStr}, bodyType={bodyType.defName}");
            return true;
        }
    }



}
