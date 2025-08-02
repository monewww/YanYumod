

using System.Collections.Generic;
using System;
using Verse;
using System.Linq;

namespace YanYu
{
    [StaticConstructorOnStartup]
    public class GameComponentTickManager : GameComponent
    {
        private static List<Action> tickActions;
        static GameComponentTickManager()
        {
            tickActions = new List<Action>();
        }

        public GameComponentTickManager(Game game) : base()
        {
            if (!game.components.Any(c => c is GameComponentTickManager))
            {
                game.components.Add(this);
            }
        }

        public override void GameComponentTick()
        {
            foreach (var act in tickActions.ToList())
            {
                act?.Invoke();
            }
        }

        public static void RegisterTickAction(Action act)
        {
            if (!tickActions.Contains(act))
            {
                tickActions.Add(act);
                //Log.Message($"[GameComponentTickManager] Registered tick action. Total now: {tickActions.Count}");
            }
        }
    }
    public static class DelayedActionManager
    {
        private static List<(int tick, Action action)> actions;
        static DelayedActionManager()
        {
            actions = new List<(int tick, Action action)>();
            GameComponentTickManager.RegisterTickAction(Tick);
        }


        public static void Register(Action action, int triggerTick)
        {
            actions.Add((triggerTick, action));
        }

        private static void Tick()
        {
            int now = Find.TickManager.TicksGame;
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                var (tick, act) = actions[i];
                if (now >= tick)
                {
                    act();
                    actions.RemoveAt(i);
                }
            }
        }
    }
    public static class PeriodicActionManager
    {
        private static List<(Action action, int interval, int nextTick)> actions;

        static PeriodicActionManager()
        {
            Init();
        }

        private static void Init()
        {
            if (actions == null)
            {
                actions = new List<(Action action, int interval, int nextTick)>();
                GameComponentTickManager.RegisterTickAction(Tick);
            }
        }

        public static void Register(Action action, int interval)
        {
            Init();
            int now = Find.TickManager.TicksGame;
            actions.Add((action, interval, now + interval));
        }

        private static void Tick()
        {
            Init();
            int now = Find.TickManager.TicksGame;

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                var (act, interval, nextTick) = actions[i];
                if (now >= nextTick)
                {
                    try
                    {
                        act();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"[YanYu] PeriodicAction Error:\n{e}");
                    }
                    actions[i] = (act, interval, now + interval); // reset next tick
                }
            }
        }
    }


}
