#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDCreateCleanPrototypeScene
    {
        private const string ScenesFolder = "Assets/_Project/Scenes";
        private const string ScenePath = "Assets/_Project/Scenes/01_CleanCore_Prototype.unity";

        [MenuItem("Boredom And Dungeons/Create Clean Prototype Scene")]
        public static void CreateCleanPrototypeScene()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("CreateCleanPrototypeScene was skipped because editor scene creation cannot run during Play Mode. Stop Play Mode, then run the scene builder again.");
                return;
            }

            EnsureScenesFolder();
            SafeNewEditorScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject roomRoot = new GameObject("BD_RoomEncounter");
            BDRoomEncounter encounter = roomRoot.AddComponent<BDRoomEncounter>();

            CreateRoomGeometry();

            GameObject player = CreatePlayer(new Vector3(0f, 1.05f, -11.5f));
            CreateWhiteLightSphere(player.transform);

            Transform horseSafeSpot = CreateHorseSafeSpot(new Vector3(-17f, 0f, -12f));
            GameObject horse = CreateHorse(new Vector3(-5f, 1.05f, -12f), horseSafeSpot);

            GameObject sword = CreateEnemyBase("BD_Enemy_Sword", new Vector3(-14f, 1.05f, 6.5f), new Color(0.03f, 0.03f, 0.04f, 1f), 100f);
            sword.AddComponent<BDSwordEnemy>();
            CreateDarkSphere(sword.transform, "DarkSphere_Sword");

            GameObject charger = CreateEnemyBase("BD_Enemy_Charger", new Vector3(-8f, 1.05f, 9f), new Color(0.12f, 0.02f, 0.02f, 1f), 120f);
            charger.AddComponent<BDChargerEnemy>();
            CreateDarkSphere(charger.transform, "DarkSphere_Charger");

            GameObject patrol = CreateEnemyBase("BD_Enemy_PatrolGuard", new Vector3(0f, 1.05f, 8.5f), new Color(0.04f, 0.04f, 0.14f, 1f), 110f);
            patrol.AddComponent<BDPatrolGuardEnemy>();
            CreateDarkSphere(patrol.transform, "DarkSphere_Patrol");

            GameObject trap = CreateEnemyBase("BD_Enemy_TrapLayer", new Vector3(8f, 1.05f, 8f), new Color(0.15f, 0.08f, 0.01f, 1f), 95f);
            trap.AddComponent<BDTrapLayerEnemy>();
            CreateDarkSphere(trap.transform, "DarkSphere_Trap");

            GameObject ranged = CreateEnemyBase("BD_Enemy_RangedShooter", new Vector3(14f, 1.05f, 4f), new Color(0.10f, 0.02f, 0.18f, 1f), 85f);
            ranged.AddComponent<BDRangedShooterEnemy>();
            CreateDarkSphere(ranged.transform, "DarkSphere_Ranged");

            GameObject jumper = CreateEnemyBase("BD_Enemy_Jumper", new Vector3(0f, 1.05f, 0f), new Color(0.02f, 0f, 0.08f, 1f), 115f);
            jumper.AddComponent<BDJumperEnemy>();
            CreateDarkSphere(jumper.transform, "DarkSphere_Jumper");

            GameObject blocker = CreateEnemyBase("BD_Enemy_ExitBlocker", new Vector3(0f, 1.05f, 12.2f), new Color(0.01f, 0.01f, 0.01f, 1f), 130f);
            blocker.AddComponent<BDExitBlockerEnemy>();
            CreateDarkSphere(blocker.transform, "DarkSphere_ExitBlocker");

            encounter.RegisterEnemy(sword.GetComponent<BDHealth>());
            encounter.RegisterEnemy(charger.GetComponent<BDHealth>());
            encounter.RegisterEnemy(patrol.GetComponent<BDHealth>());
            encounter.RegisterEnemy(trap.GetComponent<BDHealth>());
            encounter.RegisterEnemy(ranged.GetComponent<BDHealth>());
            encounter.RegisterEnemy(jumper.GetComponent<BDHealth>());
            encounter.RegisterExitBlocker(blocker.GetComponent<BDExitBlockerEnemy>());

            CreateExitZones(encounter);
            CreateExitWarningMarker(encounter);
            CreateCombatRoomForPrototype(new[]
            {
                sword.GetComponent<BDHealth>(),
                charger.GetComponent<BDHealth>(),
                patrol.GetComponent<BDHealth>(),
                trap.GetComponent<BDHealth>(),
                ranged.GetComponent<BDHealth>(),
                jumper.GetComponent<BDHealth>(),
                blocker.GetComponent<BDHealth>()
            });
            CreateCamera(player.transform);
            CreateDirectionalLight();
            CreateGameHud();

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
            Selection.activeGameObject = player;

            Debug.Log($"Created clean prototype scene: {ScenePath}");
        }

        private static void EnsureScenesFolder()
        {
            if (!Directory.Exists("Assets/_Project"))
                Directory.CreateDirectory("Assets/_Project");

            if (!Directory.Exists(ScenesFolder))
                Directory.CreateDirectory(ScenesFolder);

            AssetDatabase.Refresh();
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

            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0.08f, 0.10f, 0.13f, 1f);
            player.GetComponent<Renderer>().sharedMaterial = mat;

            player.AddComponent<BDPlayerMarker>();

            BDHealth health = player.AddComponent<BDHealth>();
            health.SetMaxHealth(100f, true);

            player.AddComponent<BDPlayerController>();
            player.AddComponent<BDPlayerDismountLeap>();
            player.AddComponent<BDPlayerCombat>();
            player.AddComponent<BDPlayerDamageFeedback>();
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

            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            enemy.GetComponent<Renderer>().sharedMaterial = mat;

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

            return enemy;
        }

        private static void CreateRoomGeometry()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "BD_Room_Ground";
            ground.transform.localScale = new Vector3(3.2f, 1f, 2.35f);
            ground.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.23f, 0.32f, 0.23f, 1f));

            CreateWall("Wall_West", new Vector3(-14f, 1f, 0f), new Vector3(0.7f, 5.5f, 20f));
            CreateWall("Wall_East", new Vector3(14f, 1f, 0f), new Vector3(0.7f, 5.5f, 20f));

            CreateWall("Wall_North_Left", new Vector3(-8f, 1f, 10f), new Vector3(12f, 5.5f, 0.7f));
            CreateWall("Wall_North_Right", new Vector3(8f, 1f, 10f), new Vector3(12f, 5.5f, 0.7f));

            CreateWall("Wall_South_Left", new Vector3(-8f, 1f, -10f), new Vector3(12f, 5.5f, 0.7f));
            CreateWall("Wall_South_Right", new Vector3(8f, 1f, -10f), new Vector3(12f, 5.5f, 0.7f));

            CreateMarker("North_Exit_Visual", new Vector3(0f, 0.08f, 10f), new Vector3(4f, 0.15f, 0.8f), new Color(0.1f, 0.9f, 1f, 1f));
            CreateMarker("South_Exit_Visual", new Vector3(0f, 0.08f, -10f), new Vector3(4f, 0.15f, 0.8f), new Color(0.1f, 0.9f, 1f, 1f));
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;

            // Keep walls grounded even when wall height changes.
            position.y = scale.y * 0.5f;

            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.10f, 0.11f, 0.12f, 1f));
            wall.AddComponent<BDOccludingWall>();
        }

        private static void CreateExitZones(BDRoomEncounter encounter)
        {
            Transform northAnchor = CreateAnchor("NorthExitAnchor", new Vector3(0f, 0f, 10.2f));
            Transform southAnchor = CreateAnchor("SouthExitAnchor", new Vector3(0f, 0f, -10.2f));

            CreateExitZone("NorthExitZone", new Vector3(0f, 1f, 9.45f), new Vector3(4.2f, 5.5f, 1.25f), encounter, northAnchor);
            CreateExitZone("SouthExitZone", new Vector3(0f, 1f, -9.45f), new Vector3(4.2f, 5.5f, 1.25f), encounter, southAnchor);
        }

        private static Transform CreateAnchor(string name, Vector3 position)
        {
            GameObject anchor = new GameObject(name);
            anchor.transform.position = position;
            return anchor.transform;
        }

        private static void CreateExitZone(string name, Vector3 position, Vector3 scale, BDRoomEncounter encounter, Transform anchor)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = name;
            zone.transform.position = position;
            zone.transform.localScale = scale;

            Collider col = zone.GetComponent<Collider>();
            col.isTrigger = true;

            zone.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_" + name, new Color(1f, 0.1f, 0.05f, 0.08f));

            BDRoomExitZone exitZone = zone.AddComponent<BDRoomExitZone>();
            exitZone.Configure(encounter, anchor);
        }

        private static void CreateExitWarningMarker(BDRoomEncounter encounter)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.name = "BD_ExitWarningMarker";
            marker.transform.position = Vector3.zero;
            marker.transform.localScale = new Vector3(2.1f, 0.04f, 2.1f);

            Collider col = marker.GetComponent<Collider>();
            if (col != null)
                UnityEngine.Object.DestroyImmediate(col);

            marker.GetComponent<Renderer>().sharedMaterial = CreateTransparentMaterial("MAT_ExitWarning", new Color(1f, 0f, 0f, 0.28f));
            marker.SetActive(false);

            SerializedObject so = new SerializedObject(encounter);
            so.FindProperty("exitWarningMarker").objectReferenceValue = marker;
            so.ApplyModifiedPropertiesWithoutUndo();
        }




        private static void CreateCombatRoomForPrototype(BDHealth[] enemies)
        {
            GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
            room.name = "BD_CombatRoom_ExitInterferenceZone";
            room.transform.position = new Vector3(0f, 1.1f, 0f);
            room.transform.localScale = new Vector3(39f, 2.2f, 29f);

            Collider zoneCollider = room.GetComponent<Collider>();
            zoneCollider.isTrigger = true;

            Renderer renderer = room.GetComponent<Renderer>();
            if (renderer != null)
                UnityEngine.Object.DestroyImmediate(renderer);

            BDCombatRoom combatRoom = room.AddComponent<BDCombatRoom>();

            foreach (BDHealth enemy in enemies)
                combatRoom.RegisterEnemy(enemy);

            Transform northAnchor = CreateAnchor("BD_CombatExitAnchor_North", new Vector3(0f, 0f, 14f));
            Transform southAnchor = CreateAnchor("BD_CombatExitAnchor_South", new Vector3(0f, 0f, -14f));

            CreateExitPressureZone("BD_ExitPressureZone_North", new Vector3(0f, 1f, 13.2f), new Vector3(8f, 5.5f, 2.2f), combatRoom, northAnchor);
            CreateExitPressureZone("BD_ExitPressureZone_South", new Vector3(0f, 1f, -13.2f), new Vector3(8f, 5.5f, 2.2f), combatRoom, southAnchor);
        }
private static void CreateExitPressureZone(string name, Vector3 position, Vector3 scale, BDCombatRoom room, Transform anchor)
        {
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
            pressureZone.Configure(room, anchor);
            room.RegisterExitAnchor(anchor);

            BDExitIntentSensor intentSensor = zone.AddComponent<BDExitIntentSensor>();
            intentSensor.Configure(room, anchor);

            BDExitPredictionSensor predictionSensor = zone.AddComponent<BDExitPredictionSensor>();
            predictionSensor.Configure(room, anchor);
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

        private static void CreateMarker(string name, Vector3 position, Vector3 scale, Color color)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = name;
            marker.transform.position = position;
            marker.transform.localScale = scale;
            marker.GetComponent<Renderer>().sharedMaterial = CreateMaterial(color);
        }

        private static void CreateCamera(Transform target)
        {
            GameObject camObject = new GameObject("Main Camera");
            camObject.tag = "MainCamera";

            Camera cam = camObject.AddComponent<Camera>();
            cam.fieldOfView = 45f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 300f;

            BDCameraFollow follow = camObject.AddComponent<BDCameraFollow>();
            follow.SetTarget(target);

            BDCameraOccluderFader occluderFader = camObject.AddComponent<BDCameraOccluderFader>();
            occluderFader.SetTarget(target);
        }


        private static void CreateGameHud()
        {
            GameObject hud = new GameObject("BD_GameHud");
            hud.AddComponent<BDGameHud>();
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
