#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using BoredomAndDungeons.Environment.NaturalMap;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools
{
    public static partial class BDCreateCleanMazePrototypeScene
    {
        private const string ScenesFolder = "Assets/_Project/Scenes";
        private const string ScenePath = "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity";

        private const int Width = 7;
        private const int Height = 7;

        private const float RoomSize = 34f;
        private const float WallThickness = 0.7f;
        private const float WallHeight = 5.5f;
        private const float DoorWidth = 10.5f;
        private const float ExitPressureDepth = 17.0f;
        private const float ExitPressureWidthBonus = 10.0f;

        private const float CorridorEncounterChance = 0.12f;
        private const float CorridorSecondEnemyChance = 0.18f;

        private static readonly System.Random Rng = new System.Random();

        [MenuItem("Boredom And Dungeons/Create Clean Maze Prototype Scene")]
        public static void CreateCleanMazePrototypeScene()
        {
            
            LogSceneBuilderStageInfo();
if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("CreateCleanMazePrototypeScene was skipped because editor scene creation cannot run during Play Mode. Stop Play Mode, then run the scene builder again.");
                return;
            }

            EnsureScenesFolder();
            SafeNewEditorScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            MazeCell[,] maze = GenerateMaze(Width, Height);

            int entranceX = Rng.Next(0, Width);
            int exitX = Rng.Next(0, Width);

            Vector2Int entranceCell = new Vector2Int(entranceX, 0);
            Vector2Int exitCell = new Vector2Int(exitX, Height - 1);

            GameObject mazeRoot = new GameObject("BD_Maze_Root");

            BuildMazeGeometry(mazeRoot.transform, maze, entranceCell, exitCell);
            CreateMinimapRoomsForMaze(maze, entranceCell);

            Vector3 entrancePosition = CellCenter(entranceCell.x, entranceCell.y) + new Vector3(0f, 0f, -RoomSize * 0.28f);
            Vector3 exitPosition = CellCenter(exitCell.x, exitCell.y) + new Vector3(0f, 0f, RoomSize * 0.48f);

            GameObject player = CreatePlayer(entrancePosition + Vector3.up * 1.05f);
            CreateWhiteLightSphere(player.transform);

            Transform horseSafeSpot = CreateHorseSafeSpot(entrancePosition + new Vector3(-10.5f, 0f, 8.5f));
            GameObject horse = CreateHorse(entrancePosition + new Vector3(9.0f, 1.05f, 3.2f), horseSafeSpot);

            Dictionary<Vector2Int, List<BDHealth>> enemiesByCell = SpawnEnemiesAcrossMaze(maze, entranceCell, exitCell);
            CreateCombatRoomsForMaze(maze, enemiesByCell, entranceCell, exitCell);

            CreateMazeExit(exitPosition + Vector3.up * 0.55f);
            CreateGameBoyCollectiblesAndCinematic(entrancePosition, exitPosition);
            CreateCamera(player.transform);
            CreateDirectionalLight();
            CreateGameHud();

            ApplyNaturalMapShapeVisualPass();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
            Selection.activeGameObject = player;

            Debug.Log($"Created clean maze prototype scene: {ScenePath}");
            Debug.Log($"Maze entrance bottom edge cell x={entranceX}, exit top edge cell x={exitX}. The maze is solvable.");
        }


        private static void EnsureScenesFolder()
        {
            if (!Directory.Exists("Assets/_Project"))
                Directory.CreateDirectory("Assets/_Project");

            if (!Directory.Exists(ScenesFolder))
                Directory.CreateDirectory(ScenesFolder);

            AssetDatabase.Refresh();
        }

        private sealed class MazeCell
        {
            public bool Visited;
            public bool NorthWall = true;
            public bool SouthWall = true;
            public bool EastWall = true;
            public bool WestWall = true;
        }

        private static MazeCell[,] GenerateMaze(int width, int height)
        {
            MazeCell[,] cells = new MazeCell[width, height];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                cells[x, y] = new MazeCell();

            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            Vector2Int current = new Vector2Int(Rng.Next(0, width), 0);
            cells[current.x, current.y].Visited = true;
            stack.Push(current);

            while (stack.Count > 0)
            {
                current = stack.Peek();
                List<Vector2Int> neighbors = GetUnvisitedNeighbors(cells, current, width, height);

                if (neighbors.Count == 0)
                {
                    stack.Pop();
                    continue;
                }

                Vector2Int next = neighbors[Rng.Next(neighbors.Count)];
                RemoveWallBetween(cells, current, next);
                cells[next.x, next.y].Visited = true;
                stack.Push(next);
            }

            return cells;
        }

        private static List<Vector2Int> GetUnvisitedNeighbors(MazeCell[,] cells, Vector2Int current, int width, int height)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            TryAdd(current.x, current.y + 1);
            TryAdd(current.x, current.y - 1);
            TryAdd(current.x + 1, current.y);
            TryAdd(current.x - 1, current.y);

            return result;

            void TryAdd(int x, int y)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                    return;

                if (!cells[x, y].Visited)
                    result.Add(new Vector2Int(x, y));
            }
        }

        private static void RemoveWallBetween(MazeCell[,] cells, Vector2Int a, Vector2Int b)
        {
            Vector2Int delta = b - a;

            if (delta == Vector2Int.up)
            {
                cells[a.x, a.y].NorthWall = false;
                cells[b.x, b.y].SouthWall = false;
            }
            else if (delta == Vector2Int.down)
            {
                cells[a.x, a.y].SouthWall = false;
                cells[b.x, b.y].NorthWall = false;
            }
            else if (delta == Vector2Int.right)
            {
                cells[a.x, a.y].EastWall = false;
                cells[b.x, b.y].WestWall = false;
            }
            else if (delta == Vector2Int.left)
            {
                cells[a.x, a.y].WestWall = false;
                cells[b.x, b.y].EastWall = false;
            }
        }

        private static void BuildMazeGeometry(Transform root, MazeCell[,] maze, Vector2Int entranceCell, Vector2Int exitCell)
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "BD_Maze_Ground";
            ground.transform.SetParent(root);
            ground.transform.position = new Vector3(
                (Width - 1) * RoomSize * 0.5f,
                -0.08f,
                (Height - 1) * RoomSize * 0.5f
            );
            ground.transform.localScale = new Vector3(Width * RoomSize + 2f, 0.16f, Height * RoomSize + 2f);
            ground.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.23f, 0.32f, 0.23f, 1f));

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector3 center = CellCenter(x, y);

                    CreateRoomFloorMarker(root, x, y, center);

                    MazeCell cell = maze[x, y];

                    if (cell.NorthWall)
                    {
                        bool isTopExit = x == exitCell.x && y == Height - 1;
                        CreateHorizontalWallWithOptionalDoor(root, $"Wall_N_{x}_{y}", center + new Vector3(0f, WallHeight * 0.5f, RoomSize * 0.5f), isTopExit);
                    }

                    if (cell.SouthWall)
                    {
                        bool isBottomEntrance = x == entranceCell.x && y == 0;
                        CreateHorizontalWallWithOptionalDoor(root, $"Wall_S_{x}_{y}", center + new Vector3(0f, WallHeight * 0.5f, -RoomSize * 0.5f), isBottomEntrance);
                    }

                    if (cell.EastWall)
                    {
                        CreateVerticalWallWithDoorIfNeeded(root, $"Wall_E_{x}_{y}", center + new Vector3(RoomSize * 0.5f, WallHeight * 0.5f, 0f), false);
                    }

                    if (cell.WestWall)
                    {
                        CreateVerticalWallWithDoorIfNeeded(root, $"Wall_W_{x}_{y}", center + new Vector3(-RoomSize * 0.5f, WallHeight * 0.5f, 0f), false);
                    }
                }
            }

            CreateMarker("BD_Maze_Entrance_Marker", CellCenter(entranceCell.x, entranceCell.y) + new Vector3(0f, 0.1f, -RoomSize * 0.48f), new Vector3(DoorWidth, 0.18f, 1.25f), new Color(0.1f, 0.9f, 1f, 1f), root);
            CreateMarker("BD_Maze_Exit_Marker", CellCenter(exitCell.x, exitCell.y) + new Vector3(0f, 0.1f, RoomSize * 0.48f), new Vector3(DoorWidth, 0.18f, 1.25f), new Color(1f, 0.85f, 0.1f, 1f), root);
        }

        private static void CreateRoomFloorMarker(Transform root, int x, int y, Vector3 center)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = $"Room_{x}_{y}_SoftFloorTint";
            marker.transform.SetParent(root);
            marker.transform.position = center + Vector3.up * 0.015f;
            marker.transform.localScale = new Vector3(RoomSize - 2.4f, 0.03f, RoomSize - 2.4f);

            float t = y / Mathf.Max(1f, Height - 1f);
            Color grass = new Color(0.18f, 0.36f, 0.18f, 0.18f);
            Color sand = new Color(0.52f, 0.44f, 0.28f, 0.18f);
            Color color = Color.Lerp(grass, sand, Mathf.Sin(t * Mathf.PI));
            marker.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_RoomTint", color);

            Collider col = marker.GetComponent<Collider>();
            if (col != null)
                UnityEngine.Object.DestroyImmediate(col);
        }

        private static void CreateHorizontalWallWithOptionalDoor(Transform root, string name, Vector3 position, bool hasDoor)
        {
            if (!hasDoor)
            {
                CreateWall(root, name, position, new Vector3(RoomSize + WallThickness, WallHeight, WallThickness));
                return;
            }

            float segmentLength = (RoomSize - DoorWidth) * 0.5f;

            CreateWall(root, name + "_Left", position + new Vector3(-(DoorWidth + segmentLength) * 0.5f, 0f, 0f), new Vector3(segmentLength, WallHeight, WallThickness));
            CreateWall(root, name + "_Right", position + new Vector3((DoorWidth + segmentLength) * 0.5f, 0f, 0f), new Vector3(segmentLength, WallHeight, WallThickness));
        }

        private static void CreateVerticalWallWithDoorIfNeeded(Transform root, string name, Vector3 position, bool hasDoor)
        {
            if (!hasDoor)
            {
                CreateWall(root, name, position, new Vector3(WallThickness, WallHeight, RoomSize + WallThickness));
                return;
            }

            float segmentLength = (RoomSize - DoorWidth) * 0.5f;

            CreateWall(root, name + "_Bottom", position + new Vector3(0f, 0f, -(DoorWidth + segmentLength) * 0.5f), new Vector3(WallThickness, WallHeight, segmentLength));
            CreateWall(root, name + "_Top", position + new Vector3(0f, 0f, (DoorWidth + segmentLength) * 0.5f), new Vector3(WallThickness, WallHeight, segmentLength));
        }

        private static void CreateWall(Transform root, string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(root);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.10f, 0.11f, 0.12f, 1f));
            wall.AddComponent<BDOccludingWall>();
        }

        private static Vector3 CellCenter(int x, int y)
        {
            return new Vector3(x * RoomSize, 0f, y * RoomSize);
        }


        private static bool IsProtectedEntranceArea(Vector2Int cell, Vector2Int entranceCell, int protectedDistance = 1)
        {
            int distance = Mathf.Abs(cell.x - entranceCell.x) + Mathf.Abs(cell.y - entranceCell.y);
            return distance <= protectedDistance;
        }

        private static Dictionary<Vector2Int, List<BDHealth>> SpawnEnemiesAcrossMaze(MazeCell[,] maze, Vector2Int entranceCell, Vector2Int exitCell)
        {
            Dictionary<Vector2Int, List<BDHealth>> enemiesByCell = new Dictionary<Vector2Int, List<BDHealth>>();
            List<Vector2Int> candidates = new List<Vector2Int>();

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);

                if (IsProtectedEntranceArea(cell, entranceCell, 1))
                    continue;

                int manhattanFromEntrance = Mathf.Abs(cell.x - entranceCell.x) + Mathf.Abs(cell.y - entranceCell.y);
                if (manhattanFromEntrance < 2)
                    continue;

                candidates.Add(cell);
            }

            Shuffle(candidates);

            foreach (Vector2Int cell in candidates)
            {
                float depth01 = cell.y / Mathf.Max(1f, Height - 1f);
                bool isExitCell = cell == exitCell;

                // Strategic density: less spam, more intent.
                double spawnChance = 0.38 + depth01 * 0.32;

                if (cell.y >= Height - 2)
                    spawnChance = 0.82;

                if (isExitCell)
                    spawnChance = 1.0;

                if (Rng.NextDouble() > spawnChance)
                    continue;

                int enemyCount = 1;

                if (cell.y >= 2)
                    enemyCount = 2;

                if (cell.y >= Height - 2)
                    enemyCount = 3;

                if (isExitCell)
                    enemyCount = 4;

                if (Rng.NextDouble() < 0.18 && !isExitCell)
                    enemyCount++;

                enemyCount = Mathf.Clamp(enemyCount, 1, 4);

                if (!enemiesByCell.ContainsKey(cell))
                    enemiesByCell[cell] = new List<BDHealth>();

                Vector3 center = CellCenter(cell.x, cell.y);

                // Every combat room gets exactly one dedicated exit blocker.
                Vector3 patrolPos = center + new Vector3(RngRange(-8.5f, 8.5f), 1.05f, RngRange(-8.5f, 8.5f));
                AddEnemyToCell(enemiesByCell, cell, CreatePatrolEnemy(patrolPos));

                for (int localIndex = 1; localIndex < enemyCount; localIndex++)
                {
                    Vector3 pos = center + new Vector3(RngRange(-11.5f, 11.5f), 1.05f, RngRange(-11.5f, 11.5f));
                    GameObject enemy;

                    if (depth01 < 0.35f)
                    {
                        enemy = (localIndex % 2 == 0) ? CreateChargerEnemy(pos) : CreateSwordEnemy(pos);
                    }
                    else if (depth01 < 0.70f)
                    {
                        int type = (localIndex + cell.x) % 3;
                        enemy = type == 0 ? CreateRangedEnemy(pos) :
                                type == 1 ? CreateTrapEnemy(pos) :
                                            CreateChargerEnemy(pos);
                    }
                    else
                    {
                        int type = (localIndex + cell.x + cell.y) % 4;
                        enemy = type == 0 ? CreateRangedEnemy(pos) :
                                type == 1 ? CreateTrapEnemy(pos) :
                                type == 2 ? CreateJumperEnemy(pos) :
                                            CreateChargerEnemy(pos);
                    }

                    AddEnemyToCell(enemiesByCell, cell, enemy);
                }
            }

            SpawnOccasionalCorridorEnemies(maze, entranceCell, exitCell, enemiesByCell);

            Debug.Log($"B&D Maze tactical density V33: combat rooms={enemiesByCell.Count}");

            return enemiesByCell;
        }


        private static void SpawnOccasionalCorridorEnemies(
            MazeCell[,] maze,
            Vector2Int entranceCell,
            Vector2Int exitCell,
            Dictionary<Vector2Int, List<BDHealth>> enemiesByCell)
        {
            int corridorEncounterCount = 0;
            int corridorEnemyCount = 0;

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                MazeCell mazeCell = maze[x, y];

                if (IsProtectedEntranceArea(cell, entranceCell, 1))
                    continue;

                int manhattanFromEntrance = Mathf.Abs(cell.x - entranceCell.x) + Mathf.Abs(cell.y - entranceCell.y);
                if (manhattanFromEntrance < 3)
                    continue;

                // Only check East/North so each corridor connection is considered once.
                if (!mazeCell.EastWall && x + 1 < Width)
                {
                    Vector2Int neighbor = new Vector2Int(x + 1, y);
                    TrySpawnCorridorEncounter(cell, neighbor, true, entranceCell, exitCell, enemiesByCell, ref corridorEncounterCount, ref corridorEnemyCount);
                }

                if (!mazeCell.NorthWall && y + 1 < Height)
                {
                    Vector2Int neighbor = new Vector2Int(x, y + 1);
                    TrySpawnCorridorEncounter(cell, neighbor, false, entranceCell, exitCell, enemiesByCell, ref corridorEncounterCount, ref corridorEnemyCount);
                }
            }

            Debug.Log($"B&D Corridor encounters V33: encounters={corridorEncounterCount}, enemies={corridorEnemyCount}");
        }

        private static void TrySpawnCorridorEncounter(
            Vector2Int cell,
            Vector2Int neighbor,
            bool eastWestConnection,
            Vector2Int entranceCell,
            Vector2Int exitCell,
            Dictionary<Vector2Int, List<BDHealth>> enemiesByCell,
            ref int corridorEncounterCount,
            ref int corridorEnemyCount)
        {
            if (cell == exitCell)
                return;

            if (IsProtectedEntranceArea(cell, entranceCell, 1) || IsProtectedEntranceArea(neighbor, entranceCell, 1))
                return;

            if (Rng.NextDouble() > CorridorEncounterChance)
                return;

            corridorEncounterCount++;

            int count = Rng.NextDouble() < CorridorSecondEnemyChance ? 2 : 1;
            Vector3 corridorCenter = (CellCenter(cell.x, cell.y) + CellCenter(neighbor.x, neighbor.y)) * 0.5f;

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = corridorCenter;
                pos.y = 1.05f;

                if (eastWestConnection)
                {
                    pos.x += RngRange(-1.1f, 1.1f);
                    pos.z += RngRange(-DoorWidth * 0.35f, DoorWidth * 0.35f);
                }
                else
                {
                    pos.x += RngRange(-DoorWidth * 0.35f, DoorWidth * 0.35f);
                    pos.z += RngRange(-1.1f, 1.1f);
                }

                GameObject enemy = CreateCorridorEnemy(pos, cell.y);
                AddEnemyToCell(enemiesByCell, cell, enemy);
                corridorEnemyCount++;
            }
        }

        private static GameObject CreateCorridorEnemy(Vector3 position, int depthY)
        {
            int roll = Rng.Next(0, depthY >= Height - 2 ? 4 : 3);

            switch (roll)
            {
                case 0:
                    return CreateSwordEnemy(position);
                case 1:
                    return CreateChargerEnemy(position);
                case 2:
                    return CreateRangedEnemy(position);
                default:
                    return CreateJumperEnemy(position);
            }
        }

        private static void AddEnemyToCell(Dictionary<Vector2Int, List<BDHealth>> enemiesByCell, Vector2Int cell, GameObject enemy)
        {
            if (enemy == null)
                return;

            if (!enemiesByCell.ContainsKey(cell))
                enemiesByCell[cell] = new List<BDHealth>();

            BDHealth health = enemy.GetComponent<BDHealth>();
            if (health != null)
                enemiesByCell[cell].Add(health);
        }

        private static float RngRange(float min, float max)
        {
            return (float)(min + Rng.NextDouble() * (max - min));
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Rng.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }




        private static void CreateMinimapRoomsForMaze(MazeCell[,] maze, Vector2Int entranceCell)
        {
            GameObject root = new GameObject("BD_MinimapRooms");

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                MazeCell mazeCell = maze[x, y];
                Vector3 center = CellCenter(x, y);

                GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
                room.name = $"BD_MinimapRoom_{x}_{y}";
                room.transform.SetParent(root.transform);
                room.transform.position = center + Vector3.up * 1.0f;
                room.transform.localScale = new Vector3(RoomSize - 1.2f, 2.0f, RoomSize - 1.2f);

                Collider collider = room.GetComponent<Collider>();
                collider.isTrigger = true;

                Renderer renderer = room.GetComponent<Renderer>();
                if (renderer != null)
                    UnityEngine.Object.DestroyImmediate(renderer);

                MeshFilter meshFilter = room.GetComponent<MeshFilter>();
                if (meshFilter != null)
                    UnityEngine.Object.DestroyImmediate(meshFilter);

                BDMinimapRoom minimapRoom = room.AddComponent<BDMinimapRoom>();
                minimapRoom.Configure(
                    cell,
                    center,
                    RoomSize,
                    !mazeCell.NorthWall,
                    !mazeCell.SouthWall,
                    !mazeCell.EastWall,
                    !mazeCell.WestWall
                );

                if (cell == entranceCell)
                    minimapRoom.ForceDiscover();
            }

            GameObject minimap = new GameObject("BD_MazeMinimap");
            minimap.AddComponent<BDMazeMinimap>();
        }

        private static void CreateCombatRoomsForMaze(MazeCell[,] maze, Dictionary<Vector2Int, List<BDHealth>> enemiesByCell, Vector2Int entranceCell, Vector2Int exitCell)
        {
            foreach (KeyValuePair<Vector2Int, List<BDHealth>> pair in enemiesByCell)
            {
                Vector2Int cell = pair.Key;
                List<BDHealth> enemies = pair.Value;

                if (enemies == null || enemies.Count == 0)
                    continue;

                GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
                room.name = $"BD_CombatRoom_{cell.x}_{cell.y}";
                room.transform.position = CellCenter(cell.x, cell.y) + Vector3.up * 1.1f;
                room.transform.localScale = new Vector3(RoomSize - 1.0f, 2.2f, RoomSize - 1.0f);

                Collider zone = room.GetComponent<Collider>();
                zone.isTrigger = true;

                Renderer renderer = room.GetComponent<Renderer>();
                if (renderer != null)
                    UnityEngine.Object.DestroyImmediate(renderer);

                BDCombatRoom combatRoom = room.AddComponent<BDCombatRoom>();

                foreach (BDHealth enemy in enemies)
                    combatRoom.RegisterEnemy(enemy);

                MazeCell mazeCell = maze[cell.x, cell.y];

                if (!mazeCell.NorthWall)
                    CreateMazeExitPressureZone($"BD_ExitPressure_N_{cell.x}_{cell.y}", CellCenter(cell.x, cell.y) + new Vector3(0f, 1f, RoomSize * 0.5f - ExitPressureDepth * 0.5f), new Vector3(DoorWidth + ExitPressureWidthBonus, 2f, ExitPressureDepth), combatRoom, CellCenter(cell.x, cell.y) + new Vector3(0f, 0f, RoomSize * 0.5f));

                if (!mazeCell.SouthWall)
                    CreateMazeExitPressureZone($"BD_ExitPressure_S_{cell.x}_{cell.y}", CellCenter(cell.x, cell.y) + new Vector3(0f, 1f, -RoomSize * 0.5f + ExitPressureDepth * 0.5f), new Vector3(DoorWidth + ExitPressureWidthBonus, 2f, ExitPressureDepth), combatRoom, CellCenter(cell.x, cell.y) + new Vector3(0f, 0f, -RoomSize * 0.5f));

                if (!mazeCell.EastWall)
                    CreateMazeExitPressureZone($"BD_ExitPressure_E_{cell.x}_{cell.y}", CellCenter(cell.x, cell.y) + new Vector3(RoomSize * 0.5f - ExitPressureDepth * 0.5f, 1f, 0f), new Vector3(ExitPressureDepth, 2f, DoorWidth + ExitPressureWidthBonus), combatRoom, CellCenter(cell.x, cell.y) + new Vector3(RoomSize * 0.5f, 0f, 0f));

                if (!mazeCell.WestWall)
                    CreateMazeExitPressureZone($"BD_ExitPressure_W_{cell.x}_{cell.y}", CellCenter(cell.x, cell.y) + new Vector3(-RoomSize * 0.5f + ExitPressureDepth * 0.5f, 1f, 0f), new Vector3(ExitPressureDepth, 2f, DoorWidth + ExitPressureWidthBonus), combatRoom, CellCenter(cell.x, cell.y) + new Vector3(-RoomSize * 0.5f, 0f, 0f));
            }
        }

        private static void CreateMazeExitPressureZone(string name, Vector3 position, Vector3 scale, BDCombatRoom room, Vector3 anchorPosition)
        {
            GameObject anchorObject = new GameObject(name + "_Anchor");
            anchorObject.transform.position = anchorPosition;
            room.RegisterExitAnchor(anchorObject.transform);

            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = name;
            zone.transform.position = position;
            zone.transform.localScale = scale;

            Collider collider = zone.GetComponent<Collider>();
            collider.isTrigger = true;

            Renderer zoneRenderer = zone.GetComponent<Renderer>();
            if (zoneRenderer != null)
                UnityEngine.Object.DestroyImmediate(zoneRenderer);

            MeshFilter zoneMesh = zone.GetComponent<MeshFilter>();
            if (zoneMesh != null)
                UnityEngine.Object.DestroyImmediate(zoneMesh);

            BDCombatRoomExitPressureZone pressureZone = zone.AddComponent<BDCombatRoomExitPressureZone>();
            pressureZone.Configure(room, anchorObject.transform);

            BDExitIntentSensor intentSensor = zone.AddComponent<BDExitIntentSensor>();
            intentSensor.Configure(room, anchorObject.transform);

            BDExitPredictionSensor predictionSensor = zone.AddComponent<BDExitPredictionSensor>();
            predictionSensor.Configure(room, anchorObject.transform);
        }

        private static GameObject CreatePlayer(Vector3 position)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "BD_Player";
            player.transform.position = position;

            CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
            if (capsule != null)
                UnityEngine.Object.DestroyImmediate(capsule);

            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.35f;
            cc.center = Vector3.zero;
            cc.stepOffset = 0.3f;

            player.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.08f, 0.10f, 0.13f, 1f));

            player.AddComponent<BDPlayerMarker>();
            player.AddComponent<BDGameBoyInventory>();

            BDHealth health = player.AddComponent<BDHealth>();
            health.SetMaxHealth(100f, true);

            player.AddComponent<BDPlayerController>();
            player.AddComponent<BDPlayerDismountLeap>();
            player.AddComponent<BDPlayerCombat>();
            player.AddComponent<BDPlayerDamageFeedback>();
            player.AddComponent<BDPlayerDodgeIFrameFeedback>();
            player.AddComponent<BDGameResetOnPlayerDeath>();

            return player;
        }

        private static GameObject CreateEnemyBase(string name, Vector3 position, Color color, float hp)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.transform.position = position;

            CapsuleCollider capsule = enemy.GetComponent<CapsuleCollider>();
            if (capsule != null)
                UnityEngine.Object.DestroyImmediate(capsule);

            CharacterController cc = enemy.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.35f;
            cc.center = Vector3.zero;
            cc.stepOffset = 0.25f;

            enemy.GetComponent<Renderer>().sharedMaterial = CreateMaterial(color);

            BDHealth health = enemy.AddComponent<BDHealth>();
            if (enemy.GetComponent<BoredomAndDungeons.BDEnemyBootstrap>() == null) enemy.AddComponent<BoredomAndDungeons.BDEnemyBootstrap>();
            health.SetMaxHealth(hp, true);

            enemy.AddComponent<BDKnockbackReceiver>();
            enemy.AddComponent<BDEnemyGroundStick>();
            if (enemy.GetComponent<BDEnemyCollisionDiscipline>() == null) enemy.AddComponent<BDEnemyCollisionDiscipline>();
            enemy.AddComponent<BDEnemyHorseHarasser>();
            enemy.AddComponent<BDEnemyLootDropper>();
            BDEnemyTacticalRole tacticalRole = enemy.AddComponent<BDEnemyTacticalRole>();
            enemy.AddComponent<BDEnemyTacticalCommand>();
            enemy.AddComponent<BDEnemyExitInterference>();

            CreateDarkSphere(enemy.transform, "DarkSphere");

            return enemy;
        }

        private static GameObject CreateSwordEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_Sword", position, new Color(0.03f, 0.03f, 0.04f, 1f), 100f);
            enemy.AddComponent<BDSwordEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.Pressure);
            return enemy;
        }

        private static GameObject CreateChargerEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_Charger", position, new Color(0.12f, 0.02f, 0.02f, 1f), 120f);
            enemy.AddComponent<BDChargerEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.BurstPunish);
            return enemy;
        }

        private static GameObject CreatePatrolEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_PatrolGuard", position, new Color(0.04f, 0.04f, 0.14f, 1f), 110f);
            enemy.AddComponent<BDPatrolGuardEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.ExitBlocker);
            enemy.AddComponent<BDPatrolExitBlockerBrain>();
            return enemy;
        }

        private static GameObject CreateTrapEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_TrapLayer", position, new Color(0.15f, 0.08f, 0.01f, 1f), 95f);
            enemy.AddComponent<BDTrapLayerEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.AreaControl);
            return enemy;
        }

        private static GameObject CreateRangedEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_RangedShooter", position, new Color(0.10f, 0.02f, 0.18f, 1f), 85f);
            enemy.AddComponent<BDRangedShooterEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.RangedControl);
            return enemy;
        }

        private static GameObject CreateJumperEnemy(Vector3 position)
        {
            GameObject enemy = CreateEnemyBase("BD_Enemy_Jumper", position, new Color(0.02f, 0f, 0.08f, 1f), 115f);
            enemy.AddComponent<BDJumperEnemy>();
            enemy.GetComponent<BDEnemyTacticalRole>().SetRole(BDEnemyTacticalRoleType.AmbushPunish);
            return enemy;
        }

        private static Transform CreateHorseSafeSpot(Vector3 position)
        {
            GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spot.name = "BD_HorseSafeSpot";
            spot.transform.position = position + Vector3.up * 0.04f;
            spot.transform.localScale = new Vector3(1.8f, 0.04f, 1.8f);

            Collider col = spot.GetComponent<Collider>();
            if (col != null)
                UnityEngine.Object.DestroyImmediate(col);

            spot.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_HorseSafeSpot", new Color(0.3f, 0.8f, 1f, 0.18f));
            spot.AddComponent<BDHorseSafeSpot>();

            return spot.transform;
        }

        private static GameObject CreateHorse(Vector3 position, Transform safeSpot)
        {
            GameObject horse = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            horse.name = "BD_Horse";
            horse.transform.position = position;
            horse.transform.localScale = Vector3.one;

            CapsuleCollider capsule = horse.GetComponent<CapsuleCollider>();
            if (capsule != null)
                UnityEngine.Object.DestroyImmediate(capsule);

            CharacterController cc = horse.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.55f;
            cc.center = Vector3.zero;
            cc.stepOffset = 0.25f;

            Renderer rootRenderer = horse.GetComponent<Renderer>();
            if (rootRenderer != null)
                UnityEngine.Object.DestroyImmediate(rootRenderer);

            MeshFilter rootMeshFilter = horse.GetComponent<MeshFilter>();
            if (rootMeshFilter != null)
                UnityEngine.Object.DestroyImmediate(rootMeshFilter);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "BD_Horse_BodyVisual";
            body.transform.SetParent(horse.transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(1.25f, 0.82f, 1.85f);
            body.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.22f, 0.16f, 0.10f, 1f));

            Collider bodyCollider = body.GetComponent<Collider>();
            if (bodyCollider != null)
                UnityEngine.Object.DestroyImmediate(bodyCollider);

            BDHorseHealth health = horse.AddComponent<BDHorseHealth>();
            health.SetMaxHealth(100f, true);

            BDHorseController controller = horse.AddComponent<BDHorseController>();
            controller.SetSafeSpot(safeSpot);
            horse.AddComponent<BDHorseWorldStatusIndicator>();

            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "BD_Horse_Head";
            head.transform.SetParent(horse.transform);
            head.transform.localPosition = new Vector3(0f, 0.48f, 0.62f);
            head.transform.localScale = new Vector3(0.55f, 0.45f, 0.55f);
            head.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.24f, 0.17f, 0.11f, 1f));

            Collider headCollider = head.GetComponent<Collider>();
            if (headCollider != null)
                UnityEngine.Object.DestroyImmediate(headCollider);

            return horse;
        }



        private static void CreateGameBoyCollectiblesAndCinematic(Vector3 entrancePosition, Vector3 exitPosition)
        {
            GameObject root = new GameObject("BD_GameBoy_Collectibles_And_Cinematic_V111");

            // Collectibles must not be in the starting room.
            // They are placed deeper and side-offset, with small hideouts so they feel hidden.
            Vector3 gameBoySpot = Vector3.Lerp(entrancePosition, exitPosition, 0.72f) + new Vector3(-12.0f, 0.15f, 7.5f);
            Vector3 batteryASpot = Vector3.Lerp(entrancePosition, exitPosition, 0.45f) + new Vector3(13.5f, 0.15f, -8.5f);
            Vector3 batteryBSpot = Vector3.Lerp(entrancePosition, exitPosition, 0.88f) + new Vector3(9.5f, 0.15f, -11.5f);

            CreateCollectibleHideout(root.transform, "BD_Hidden_GameBoy_Hideout", gameBoySpot, 0);
            CreateCollectibleHideout(root.transform, "BD_Hidden_BatteryA_Hideout", batteryASpot, 1);
            CreateCollectibleHideout(root.transform, "BD_Hidden_BatteryB_Hideout", batteryBSpot, 2);

            BDGameBoyCollectible gameBoy = BDGameBoyCollectible.SpawnGameBoy(gameBoySpot);
            gameBoy.transform.SetParent(root.transform, worldPositionStays: true);
            AttachCollectibleGuardianSpawner(gameBoy.gameObject, 8.5f, 2, 1, 6.2f);

            BDGameBoyCollectible batteryA = BDGameBoyCollectible.SpawnBattery(batteryASpot);
            batteryA.transform.SetParent(root.transform, worldPositionStays: true);
            AttachCollectibleGuardianSpawner(batteryA.gameObject, 7.25f, 1, 1, 5.6f);

            BDGameBoyCollectible batteryB = BDGameBoyCollectible.SpawnBattery(batteryBSpot);
            batteryB.transform.SetParent(root.transform, worldPositionStays: true);
            AttachCollectibleGuardianSpawner(batteryB.gameObject, 7.75f, 2, 0, 5.8f);

            Vector3 roomCenter = entrancePosition + new Vector3(0f, 0f, -24f);
            GameObject room = CreateDarkGameBoyRoom(roomCenter);
            room.transform.SetParent(root.transform, worldPositionStays: true);
        }

        private static void AttachCollectibleGuardianSpawner(GameObject collectible, float radius, int swordGuardians, int chargerGuardians, float spawnDistance)
        {
            if (collectible == null)
                return;

            BDCollectibleGuardianSpawner spawner = collectible.AddComponent<BDCollectibleGuardianSpawner>();
            spawner.Configure(radius, swordGuardians, chargerGuardians, spawnDistance);
        }

        private static void CreateCollectibleHideout(Transform root, string name, Vector3 center, int variant)
        {
            GameObject hideout = new GameObject(name);
            hideout.transform.SetParent(root, worldPositionStays: true);

            Material wallMaterial = CreateMaterial(variant == 0 ? new Color(0.055f, 0.070f, 0.050f, 1f) : new Color(0.075f, 0.060f, 0.045f, 1f));
            Material patchMaterial = CreateMaterial(variant == 0 ? new Color(0.11f, 0.22f, 0.09f, 1f) : new Color(0.16f, 0.12f, 0.075f, 1f));

            for (int i = 0; i < 5; i++)
            {
                float angle = -110f + i * 55f;
                Vector3 dir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                Vector3 pos = center + dir * 2.15f + Vector3.up * 0.42f;

                GameObject stone = GameObject.CreatePrimitive(i % 2 == 0 ? PrimitiveType.Cube : PrimitiveType.Sphere);
                stone.name = "BD_Collectible_Hideout_Stone";
                stone.transform.SetParent(hideout.transform, worldPositionStays: true);
                stone.transform.position = pos;
                stone.transform.rotation = Quaternion.Euler(0f, angle + 20f, 0f);
                stone.transform.localScale = new Vector3(0.95f + 0.18f * i, 0.55f + 0.08f * (i % 2), 0.42f + 0.05f * i);
                stone.GetComponent<Renderer>().sharedMaterial = wallMaterial;
            }

            GameObject groundPatch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            groundPatch.name = "BD_Collectible_Hideout_GroundPatch";
            groundPatch.transform.SetParent(hideout.transform, worldPositionStays: true);
            groundPatch.transform.position = center + Vector3.down * 0.02f;
            groundPatch.transform.localScale = new Vector3(2.6f, 0.025f, 2.6f);
            groundPatch.GetComponent<Renderer>().sharedMaterial = patchMaterial;

            Collider patchCollider = groundPatch.GetComponent<Collider>();
            if (patchCollider != null)
                UnityEngine.Object.DestroyImmediate(patchCollider);

            GameObject hintLight = new GameObject("BD_Collectible_Hideout_Subtle_Light");
            hintLight.transform.SetParent(hideout.transform, worldPositionStays: true);
            hintLight.transform.position = center + Vector3.up * 1.35f;
            Light light = hintLight.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = variant == 0 ? new Color(0.25f, 0.75f, 0.25f, 1f) : new Color(0.90f, 0.62f, 0.20f, 1f);
            light.intensity = 0.40f;
            light.range = 4.2f;
        }

        private static GameObject CreateDarkGameBoyRoom(Vector3 center)
        {
            GameObject room = new GameObject("BD_Dark_GameBoy_Cinematic_Room");

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "BD_DarkRoom_Floor";
            floor.transform.SetParent(room.transform, worldPositionStays: true);
            floor.transform.position = center + new Vector3(0f, -0.05f, 0f);
            floor.transform.localScale = new Vector3(18f, 0.10f, 18f);
            floor.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.025f, 0.022f, 0.024f, 1f));

            GameObject backWall = CreateDarkRoomWall(room.transform, "BD_DarkRoom_BackWall", center + new Vector3(0f, 2.25f, -9f), new Vector3(18f, 4.5f, 0.45f));
            GameObject leftWall = CreateDarkRoomWall(room.transform, "BD_DarkRoom_LeftWall", center + new Vector3(-9f, 2.25f, 0f), new Vector3(0.45f, 4.5f, 18f));
            GameObject rightWall = CreateDarkRoomWall(room.transform, "BD_DarkRoom_RightWall", center + new Vector3(9f, 2.25f, 0f), new Vector3(0.45f, 4.5f, 18f));
            GameObject frontLeftWall = CreateDarkRoomWall(room.transform, "BD_DarkRoom_FrontWall_Left", center + new Vector3(-6.0f, 2.25f, 9f), new Vector3(6f, 4.5f, 0.45f));
            GameObject frontRightWall = CreateDarkRoomWall(room.transform, "BD_DarkRoom_FrontWall_Right", center + new Vector3(6.0f, 2.25f, 9f), new Vector3(6f, 4.5f, 0.45f));

            Transform seatPoint = CreatePoint("BD_GameBoy_Cinematic_SeatPoint", center + new Vector3(0f, 1.08f, -0.35f), Quaternion.Euler(0f, 180f, 0f), room.transform);
            Transform gameBoyPoint = CreatePoint("BD_GameBoy_Cinematic_GameBoyPoint", center + new Vector3(0f, 1.75f, -1.45f), Quaternion.Euler(0f, 180f, 0f), room.transform);
            Transform lightPoint = CreatePoint("BD_GameBoy_Cinematic_LightPoint", center + new Vector3(0f, 2.05f, -1.35f), Quaternion.identity, room.transform);

            CreateChair(room.transform, center + new Vector3(0f, 0.45f, 0f));

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "BD_GameBoy_Cinematic_Trigger";
            trigger.transform.SetParent(room.transform, worldPositionStays: true);
            trigger.transform.position = center + new Vector3(0f, 1.1f, 0.6f);
            trigger.transform.localScale = new Vector3(5.4f, 2.2f, 5.4f);

            Collider triggerCollider = trigger.GetComponent<Collider>();
            if (triggerCollider != null)
                triggerCollider.isTrigger = true;

            Renderer triggerRenderer = trigger.GetComponent<Renderer>();
            if (triggerRenderer != null)
                triggerRenderer.sharedMaterial = CreateTransparentMaterial("MAT_GameBoyCinematicTrigger", new Color(0.1f, 0.8f, 0.25f, 0.08f));

            BDGameEndingStateController endingStateController = trigger.AddComponent<BDGameEndingStateController>();

            BDGameBoyCinematicRoom cinematic = trigger.AddComponent<BDGameBoyCinematicRoom>();
            cinematic.Configure(seatPoint, gameBoyPoint, lightPoint, endingStateController.RequiredBatteries);

            GameObject darkLightObject = new GameObject("BD_DarkRoom_Dim_Green_Light");
            darkLightObject.transform.SetParent(room.transform, worldPositionStays: true);
            darkLightObject.transform.position = center + new Vector3(0f, 3.2f, -1.5f);
            Light darkLight = darkLightObject.AddComponent<Light>();
            darkLight.type = LightType.Point;
            darkLight.color = new Color(0.18f, 0.70f, 0.30f, 1f);
            darkLight.intensity = 0.72f;
            darkLight.range = 10f;

            return room;
        }

        private static GameObject CreateDarkRoomWall(Transform root, string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(root, worldPositionStays: true);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.035f, 0.033f, 0.038f, 1f));
            wall.AddComponent<BDOccludingWall>();
            return wall;
        }

        private static Transform CreatePoint(string name, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject point = new GameObject(name);
            point.transform.SetParent(parent, worldPositionStays: true);
            point.transform.position = position;
            point.transform.rotation = rotation;
            return point.transform;
        }

        private static void CreateChair(Transform root, Vector3 center)
        {
            Material chairMaterial = CreateMaterial(new Color(0.12f, 0.075f, 0.045f, 1f));

            GameObject seat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            seat.name = "BD_GameBoy_Cinematic_Chair_Seat";
            seat.transform.SetParent(root, worldPositionStays: true);
            seat.transform.position = center + new Vector3(0f, 0.45f, 0f);
            seat.transform.localScale = new Vector3(1.35f, 0.22f, 1.25f);
            seat.GetComponent<Renderer>().sharedMaterial = chairMaterial;

            GameObject back = GameObject.CreatePrimitive(PrimitiveType.Cube);
            back.name = "BD_GameBoy_Cinematic_Chair_Back";
            back.transform.SetParent(root, worldPositionStays: true);
            back.transform.position = center + new Vector3(0f, 1.15f, 0.58f);
            back.transform.localScale = new Vector3(1.35f, 1.35f, 0.20f);
            back.GetComponent<Renderer>().sharedMaterial = chairMaterial;

            for (int i = 0; i < 4; i++)
            {
                float x = (i % 2 == 0) ? -0.52f : 0.52f;
                float z = (i < 2) ? -0.42f : 0.42f;

                GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leg.name = "BD_GameBoy_Cinematic_Chair_Leg";
                leg.transform.SetParent(root, worldPositionStays: true);
                leg.transform.position = center + new Vector3(x, 0.10f, z);
                leg.transform.localScale = new Vector3(0.16f, 0.55f, 0.16f);
                leg.GetComponent<Renderer>().sharedMaterial = chairMaterial;
            }
        }

        private static void CreateMazeExit(Vector3 position)
        {
            GameObject exit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            exit.name = "BD_Maze_TopExitGoal";
            exit.transform.position = position;
            exit.transform.localScale = new Vector3(2.3f, 0.12f, 2.3f);

            Collider col = exit.GetComponent<Collider>();
            col.isTrigger = true;

            exit.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_MazeExitGoal", new Color(1f, 0.85f, 0.05f, 0.42f));
            exit.AddComponent<BDMazeExitGoal>();

            GameObject lightObj = new GameObject("BD_Maze_Exit_Light");
            lightObj.transform.SetParent(exit.transform);
            lightObj.transform.localPosition = Vector3.up * 1.5f;

            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.85f, 0.3f, 1f);
            light.intensity = 2.2f;
            light.range = 5.5f;
        }

        private static void CreateWhiteLightSphere(Transform player)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "BD_Player_White_Light_Sphere";
            sphere.transform.SetParent(player);
            sphere.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            sphere.transform.localScale = Vector3.one * 3f;

            SphereCollider col = sphere.GetComponent<SphereCollider>();
            col.isTrigger = true;

            sphere.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_PlayerWhiteSphere", new Color(1f, 1f, 1f, 0.22f));

            GameObject lightObject = new GameObject("BD_Player_PointLight");
            lightObject.transform.SetParent(player);
            lightObject.transform.localPosition = new Vector3(0f, 1f, 0f);

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = Color.white;
            light.intensity = 2.2f;
            light.range = 4.5f;
        }

        private static void CreateDarkSphere(Transform enemy, string name)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.SetParent(enemy);
            sphere.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            sphere.transform.localScale = Vector3.one * 2.7f;

            SphereCollider col = sphere.GetComponent<SphereCollider>();
            col.isTrigger = true;

            sphere.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_DarkSphere", new Color(0f, 0f, 0f, 0.32f));
        }

        private static void CreateMarker(string name, Vector3 position, Vector3 scale, Color color, Transform parent)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = name;
            marker.transform.SetParent(parent);
            marker.transform.position = position;
            marker.transform.localScale = scale;
            marker.GetComponent<Renderer>().sharedMaterial = CreateMaterial(color);

            Collider col = marker.GetComponent<Collider>();
            if (col != null)
                UnityEngine.Object.DestroyImmediate(col);
        }

        private static void CreateCamera(Transform target)
        {
            GameObject camObject = new GameObject("Main Camera");
            camObject.tag = "MainCamera";

            Camera cam = camObject.AddComponent<Camera>();
            cam.fieldOfView = 58f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 500f;

            BDCameraFollow follow = camObject.AddComponent<BDCameraFollow>();
            follow.SetTarget(target);

            BDCameraOccluderFader occluderFader = camObject.AddComponent<BDCameraOccluderFader>();
            occluderFader.SetTarget(target);
        }


        private static void CreateGameHud()
        {
            GameObject hud = new GameObject("BD_GameHud");
            hud.AddComponent<BDGameHud>();
            hud.AddComponent<BDSecretCollectibleHud>();
        }

        private static void CreateDirectionalLight()
        {
            GameObject obj = new GameObject("Directional Light");
            Light light = obj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.75f;
            light.color = new Color(1f, 0.95f, 0.85f, 1f);
            obj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static Material CreateMaterial(Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            return mat;
        }

        private static Material CreateTransparentMaterial(string name, Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.name = name;
            mat.color = color;
            mat.SetFloat("_Mode", 3f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            return mat;
        }

        private static void ApplyNaturalMapShapeVisualPass()
        {
            const int seed = 102;
            System.Random random = new System.Random(seed);

            GameObject root = new GameObject("BD_Natural_Map_Shape_Pass_V102");

            GameObject[] allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            int roundedCorners = 0;
            int edgeBoulders = 0;
            int groundPatches = 0;

            for (int i = 0; i < allObjects.Length; i++)
            {
                GameObject source = allObjects[i];
                if (source == null)
                    continue;

                string name = source.name;
                if (string.IsNullOrEmpty(name))
                    continue;

                bool likelyWall =
                    name.Contains("Wall") ||
                    name.Contains("Pillar") ||
                    name.Contains("Column") ||
                    name.Contains("Rock") ||
                    name.Contains("Block");

                if (!likelyWall)
                    continue;

                if (source.GetComponentInParent<BDHealth>() != null)
                    continue;

                if (source.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (source.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                Vector3 p = source.transform.position;
                int roll = random.Next(0, 100);

                if (roll < 8 && roundedCorners < 85)
                {
                    Vector3 offset = new Vector3(
                        (float)(random.NextDouble() * 1.15 - 0.575),
                        0.22f,
                        (float)(random.NextDouble() * 1.15 - 0.575)
                    );

                    float radius = 0.42f + (float)random.NextDouble() * 0.30f;
                    float height = 0.16f + (float)random.NextDouble() * 0.20f;
                    Quaternion rotation = Quaternion.Euler(0f, random.Next(0, 360), 0f);
                    bool mossy = random.Next(0, 100) < 42;

                    GameObject visual = BDNaturalMapVisuals.CreateRoundedCornerRock(p + offset, rotation, radius, height, mossy);
                    visual.transform.SetParent(root.transform, worldPositionStays: true);
                    roundedCorners++;
                }
                else if (roll < 17 && edgeBoulders < 120)
                {
                    Vector3 offset = new Vector3(
                        (float)(random.NextDouble() * 1.65 - 0.825),
                        0.12f,
                        (float)(random.NextDouble() * 1.65 - 0.825)
                    );

                    Vector3 scale = new Vector3(
                        0.30f + (float)random.NextDouble() * 0.32f,
                        0.10f + (float)random.NextDouble() * 0.16f,
                        0.20f + (float)random.NextDouble() * 0.28f
                    );

                    Quaternion rotation = Quaternion.Euler(0f, random.Next(0, 360), 0f);
                    bool mossy = random.Next(0, 100) < 30;

                    GameObject visual = BDNaturalMapVisuals.CreateSoftEdgeBoulder(p + offset, scale, rotation, mossy);
                    visual.transform.SetParent(root.transform, worldPositionStays: true);
                    edgeBoulders++;
                }

                if (roll >= 17 && roll < 25 && groundPatches < 110)
                {
                    Vector3 offset = new Vector3(
                        (float)(random.NextDouble() * 1.85 - 0.925),
                        0f,
                        (float)(random.NextDouble() * 1.85 - 0.925)
                    );

                    Vector3 scale = new Vector3(
                        0.52f + (float)random.NextDouble() * 0.82f,
                        1f,
                        0.30f + (float)random.NextDouble() * 0.60f
                    );

                    Quaternion rotation = Quaternion.Euler(0f, random.Next(0, 360), 0f);
                    int biomeHint = random.Next(0, 3);

                    GameObject visual = BDNaturalMapVisuals.CreateGroundPatch(p + offset, scale, rotation, biomeHint);
                    visual.transform.SetParent(root.transform, worldPositionStays: true);
                    groundPatches++;
                }
            }

            Debug.Log($"B&D Natural Map Shape V102: roundedCorners={roundedCorners}, edgeBoulders={edgeBoulders}, groundPatches={groundPatches}");
        }

        private static UnityEngine.SceneManagement.Scene SafeNewEditorScene(UnityEditor.SceneManagement.NewSceneSetup setup, UnityEditor.SceneManagement.NewSceneMode mode)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("Skipped EditorSceneManager.NewScene because Play Mode is active.");
                return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            }

            return UnityEditor.SceneManagement.EditorSceneManager.NewScene(setup, mode);
        }


    }
}
#endif
