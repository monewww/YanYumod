//using HarmonyLib;
//using System.Collections.Generic;
//using Verse;

//public class GameComponent_FastMover : GameComponent
//{
//    private static List<YanYu.FastMove> activeMotions = new List<YanYu.FastMove>();

//    public GameComponent_FastMover(Game game) : base() { }

//    public override void GameComponentTick()
//    {
//        for (int i = activeMotions.Count - 1; i >= 0; i--)
//        {
//            var motion = activeMotions[i];
//            motion.Tick();

//            // 移除已完成
//            if (!motionActive(motion))
//            {
//                activeMotions.RemoveAt(i);
//            }
//        }
//    }

//    private bool motionActive(YanYu.FastMove move) => move != null && Traverse.Create(move).Field("active").GetValue<bool>();

//    public static void Register(YanYu.FastMove move)
//    {
//        activeMotions.Add(move);
//    }
//}
