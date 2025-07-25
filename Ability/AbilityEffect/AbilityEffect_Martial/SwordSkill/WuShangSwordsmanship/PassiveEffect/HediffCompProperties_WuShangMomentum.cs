

using Verse;
namespace YanYu
{
    public class HediffCompProperties_WuShangMomentum : HediffCompProperties_AttackTrigger
    {
        public float damagePerStack = 0.04f;
        public float apPerStack = 0.02f; 
        public int maxStack = 10;
        public int ticksToDisappear = 3600; 
        public HediffCompProperties_WuShangMomentum()
        {
            this.compClass = typeof(HediffComp_WuShangMomentum);
        }

    }
}
