
using RimWorld;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;

namespace YanYu
{
    public static class MapDataUtility
    {
        public static bool CanWriteThingDeep(Thing t)
        {
            // 只保存存活且不是临时的 Thing
            if (t == null || t.Destroyed) return false;
            if (t.def == null) return false;


            return true;
        }
        private static string MapDataFolder => Path.Combine(GenFilePaths.SaveDataFolderPath, "YanYuMapData");

        public static void SavePawns(Map map, string mapFileName)
        {
            if (!Directory.Exists(MapDataFolder))
            {
                Directory.CreateDirectory(MapDataFolder);
            }

            string fullPath = Path.Combine(MapDataFolder, mapFileName + ".xml");

            List<Pawn> pawnsToSave = new List<Pawn>();
            foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
            {
                pawnsToSave.Add(pawn);
            }

            SafeSaver.Save(fullPath, "pawns", delegate
            {
                Scribe_Collections.Look(ref pawnsToSave, "pawns", LookMode.Deep);
            });
        }

        public static void LoadPawns(Map targetMap, string mapFileName)
        {
            string fullPath = Path.Combine(MapDataFolder, mapFileName + ".xml");
            if (!File.Exists(fullPath))
            {
                Log.Warning("Map data file not found: " + fullPath);
                return;
            }

            List<Pawn> loadedPawns = null;

            Scribe.loader.InitLoading(fullPath);
            try
            {
                Scribe_Collections.Look(ref loadedPawns, "pawns", LookMode.Deep);
            }
            finally
            {
                Scribe.loader.FinalizeLoading();
            }

            if (loadedPawns != null)
            {
                foreach (Pawn pawn in loadedPawns)
                {
                    // 找一个可用的位置生成
                    IntVec3 spawnCell = CellFinder.RandomClosewalkCellNear(targetMap.Center, targetMap, 5);
                    GenSpawn.Spawn(pawn, spawnCell, targetMap);
                }
            }
        }

        public static void SaveMapData(Map map, string mapFileName)
        {
            if (!Directory.Exists(MapDataFolder))
                Directory.CreateDirectory(MapDataFolder);

            string fullPath = Path.Combine(MapDataFolder, mapFileName + ".xml");

            //List<Pawn> pawnsToSave = map.mapPawns.AllPawnsSpawned.ToList();
            List<Thing> thingsToSave = map.listerThings.AllThings.Where(t=>CanWriteThingDeep(t)).ToList();
            Log.Message($"Saving {thingsToSave.Count} pawns to map data file: {fullPath}");

            List<TerrainDef> terrainGrid = new List<TerrainDef>();
            for (int i = 0; i < map.cellIndices.NumGridCells; i++)
                terrainGrid.Add(map.terrainGrid.TerrainAt(map.cellIndices.IndexToCell(i)));

            SafeSaver.Save(fullPath, "mapData", delegate
            {
                //Scribe_Collections.Look(ref pawnsToSave, "pawns", LookMode.Deep);
                Scribe_Collections.Look(ref thingsToSave, "things", LookMode.Deep);
                Scribe_Collections.Look(ref terrainGrid, "terrain", LookMode.Def);
            });
        }

        public static void LoadMapData(Map map, string mapFileName)
        {
            string fullPath = Path.Combine(MapDataFolder, mapFileName + ".xml");
            if (!File.Exists(fullPath))
            {
                Log.Warning("Map data file not found: " + fullPath);
                return;
            }

            foreach (Thing thing in map.listerThings.AllThings.ToList())
                thing.Destroy(DestroyMode.Vanish);

            // 初始化加载
            List<Pawn> loadedPawns = null;
            List<Thing> loadedThings = null;
            List<TerrainDef> loadedTerrain = null;

            Scribe.loader.InitLoading(fullPath);
            try
            {
                Scribe_Collections.Look(ref loadedPawns, "pawns", LookMode.Deep);
                Scribe_Collections.Look(ref loadedThings, "buildings", LookMode.Deep);
                Scribe_Collections.Look(ref loadedTerrain, "terrain", LookMode.Def);
            }
            finally
            {
                Scribe.loader.FinalizeLoading();
            }

            // 恢复地形
            if (loadedTerrain != null)
            {
                for (int i = 0; i < loadedTerrain.Count && i < map.cellIndices.NumGridCells; i++)
                {
                    map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i), loadedTerrain[i]);
                }
            }

            // 恢复建筑
            if (loadedThings != null)
            {
                foreach (Thing t in loadedThings)
                {
                    GenSpawn.Spawn(t, t.Position, map);
                }
            }

            // 恢复 Pawn
            if (loadedPawns != null)
            {
                foreach (Pawn p in loadedPawns)
                {
                    GenSpawn.Spawn(p, p.Position, map);
                }
            }

            Messages.Message("地图数据加载完成", MessageTypeDefOf.TaskCompletion, false);
        }

    }
}