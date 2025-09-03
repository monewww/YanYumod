using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace YanYu
{
    [HarmonyPatch(typeof(Pawn), "GetFloatMenuOptions")]
    public static class Pawn_GetFloatMenuOptions_Patch
    {
        public static void Postfix(Pawn __instance, Pawn selPawn, ref IEnumerable<FloatMenuOption> __result)
        {
            var list = new List<FloatMenuOption>(__result);

            list.Add(new FloatMenuOption("get quest", delegate
            {
                var ext = __instance.kindDef.GetModExtension<QuestGiver>();
                if (ext != null)
                {
                    Job job = JobMaker.MakeJob(YanYu_JobDefOf.GoAndTalkWithLeader, __instance);
                    selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);

                }
            }));
            __result = list;

        }
    }
}

