using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace YanYu
{
    [StaticConstructorOnStartup]
    public static class DebugTools_YanYu
    {
        [DebugAction("YanYu", "保存当前地图Pawn", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SaveCurrentMapPawns()
        {
            Map map = Find.CurrentMap;
            if (map != null)
            {
                MapDataUtility.SavePawns(map, "LingYueGongPawns");
                Messages.Message("保存了 " + map.mapPawns.AllPawnsSpawned.Count + " 个Pawn", MessageTypeDefOf.TaskCompletion, false);
            }
        }

        [DebugAction("YanYu", "加载Pawn到当前地图", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void LoadPawnsToCurrentMap()
        {
            Map map = Find.CurrentMap;
            if (map != null)
            {
                MapDataUtility.LoadPawns(map, "LingYueGongPawns");
                Messages.Message("已加载Pawn到当前地图", MessageTypeDefOf.TaskCompletion, false);
            }
        }

        [DebugAction("YanYu", "保存当前地图", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SaveCurrentMap()
        {
            Map map = Find.CurrentMap;
            if (map != null)
            {
                MapDataUtility.SaveMapData(map, "LingYueGongMap");
                Messages.Message("已保存当前地图数据", MessageTypeDefOf.TaskCompletion, false);
            }
        }

        [DebugAction("YanYu", "加载地图数据", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void LoadMapData()
        {
            Map map = Find.CurrentMap;
            if (map != null)
            {
                MapDataUtility.LoadMapData(map, "LingYueGongMap");
                Messages.Message("已加载地图数据", MessageTypeDefOf.TaskCompletion, false);
            }
        }
        [DebugAction("YanYu", "Spawn YanYu_NPC With Faction", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static List<DebugActionNode> SpawnNPCWithFaction()
        {
            List<DebugActionNode> list = new List<DebugActionNode>();
            foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
            {
                if (!(faction2.def.defName.StartsWith("YanYu"))) continue;
                Faction faction = faction2;
                list.Add(new DebugActionNode(faction.Name, DebugActionType.Action, null, null)
                {
                    action = delegate ()
                    {
                        //Log.Error("Spawn YanYu_NPC With Faction: " + faction.Name+"Pawndef:"+ YanYu_PawnDefOf.YanYu_NPCNormal.defName);
                        PawnGenerationRequest request = new PawnGenerationRequest(YanYu_PawnDefOf.YanYu_NPCNormal, faction, PawnGenerationContext.NonPlayer, null, false, false, false, false, true, 1f, false, false, false, false, false, false);
                        Pawn pawn = PawnGenerator.GeneratePawn(request);
                        GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap);
                        pawn.needs?.AddOrRemoveNeedsAsAppropriate();
                    }
                });
            }
            return list;
        }
    }
}
