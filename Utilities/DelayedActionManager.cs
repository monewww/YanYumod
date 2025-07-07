

using System.Collections.Generic;
using System;
using Verse;
using System.Linq;

namespace YanYu.Utilities
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
                tickActions.Add(act);
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
}
