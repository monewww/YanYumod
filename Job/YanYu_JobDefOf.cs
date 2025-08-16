
using RimWorld;
using Verse;

namespace YanYu
{
    [DefOf]
    public static class YanYu_JobDefOf
    {
        public static JobDef GoAndTalkWithLeader;
        
        static YanYu_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(YanYu_JobDefOf));
        }
    }
}
