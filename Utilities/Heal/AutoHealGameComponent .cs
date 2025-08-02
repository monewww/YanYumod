using RimWorld;
using Verse;

namespace YanYu
{
    public class AutoHealGameComponent : GameComponent
    {
        private bool initialized = false;

        public AutoHealGameComponent(Game game) : base() { }

        public override void GameComponentUpdate()
        {
            if (!initialized && Current.Game != null && Find.TickManager != null)
            {
                PeriodicActionManager.Register(AutoHealInitializer.HealAllPawns, 60);
                initialized = true;
            }
        }
    }

    public static class AutoHealInitializer
    {
        public static void HealAllPawns()
        {
            foreach (var pawn in PawnsFinder.AllMaps_FreeColonistsAndPrisoners)
            {
                if (pawn.Dead || pawn.health == null) continue;

                float healPerSecond = pawn.GetStatValue(StatDef.Named("AutoHealPerSecond"), true);
                if (healPerSecond > 0f)
                {
                    HealUtil.TryHeal(pawn, healPerSecond);
                }
            }
        }
    }

}
