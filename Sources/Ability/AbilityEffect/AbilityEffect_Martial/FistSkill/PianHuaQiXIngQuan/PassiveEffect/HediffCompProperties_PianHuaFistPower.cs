

using Verse;
namespace YanYu
{
    public class HediffCompProperties_PianHuaFistPower : HediffCompProperties_AttackTrigger
    {
        public float damagePerStack = 0.03f;
        public int maxStack = 3;
        public int ticksToDisappear = 3600; 
        public HediffCompProperties_PianHuaFistPower()
        {
            this.compClass = typeof(HediffComp_PianHuaFistPower);
        }

    }
}
