using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDC07PlayableBossEncounterInstaller
    {
        public const string EncounterRootName =
            "C07_FrameworkTestEncounter";

        public const string BossBodyName =
            "C07_FrameworkTestBoss";

        public const string TriggerName =
            "C07_FrameworkTestEntryTrigger";

        public static bool TryInstallActiveScene(
            out string error)
        {
            Scene scene = SceneManager.GetActiveScene();

            if (!scene.IsValid() || !scene.isLoaded)
            {
                error =
                    "C07.16 requires a valid loaded scene.";
                return false;
            }

            GameObject existing = FindSceneRoot(
                scene,
                EncounterRootName
            );

            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    existing
                );
            }

            BDPlayerMarker playerMarker =
                UnityEngine.Object.FindFirstObjectByType<
                    BDPlayerMarker>();

            if (playerMarker == null)
            {
                error =
                    "C07.16 could not find BDPlayerMarker.";
                return false;
            }

            if (!TrySelectArenaRoom(
                    playerMarker.transform,
                    out BDMinimapRoom arenaRoom,
                    out error))
            {
                return false;
            }

            GameObject root = new GameObject(
                EncounterRootName
            );

            SceneManager.MoveGameObjectToScene(
                root,
                scene
            );

            BDBossEncounterController encounter =
                root.AddComponent<
                    BDBossEncounterController>();

            BDBossHealthGroup healthGroup =
                root.AddComponent<
                    BDBossHealthGroup>();

            BDBossEncounterRuntimeBindings bindings =
                root.AddComponent<
                    BDBossEncounterRuntimeBindings>();

            root.AddComponent<BDBossHealthHud>();

            Vector3 arenaCenter =
                arenaRoom.WorldCenter;

            arenaCenter.y = ResolveGroundY(
                arenaCenter,
                playerMarker.transform.position.y
            );

            float roomRadius = Mathf.Clamp(
                arenaRoom.RoomSize * 0.32f,
                4.25f,
                7.5f
            );

            GameObject boss = CreateBossBody(
                root.transform,
                encounter,
                arenaCenter,
                roomRadius
            );

            BDBossArenaTrigger trigger =
                CreateEntryTrigger(
                    root.transform,
                    encounter,
                    arenaCenter,
                    arenaRoom.RoomSize
                );

            CreateArenaLabel(
                root.transform,
                arenaCenter,
                arenaRoom.RoomSize
            );

            healthGroup.RefreshChannels();
            bindings.RefreshReferences();
            trigger.Configure(encounter);

            EditorUtility.SetDirty(encounter);
            EditorUtility.SetDirty(healthGroup);
            EditorUtility.SetDirty(bindings);
            EditorUtility.SetDirty(trigger);
            EditorUtility.SetDirty(boss);
            EditorSceneManager.MarkSceneDirty(scene);

            if (!ValidateExistingEncounter(
                    root,
                    out error))
            {
                return false;
            }

            // C07 SCENE SAVE OWNED BY TEST EVERYTHING.
            // This nested installer only marks the scene dirty.

            AssetDatabase.SaveAssets();
            error = string.Empty;
            return true;
        }

        public static bool ValidateActiveScene(
            out string error)
        {
            Scene scene = SceneManager.GetActiveScene();

            if (!scene.IsValid() || !scene.isLoaded)
            {
                error =
                    "C07.16 validation has no loaded scene.";
                return false;
            }

            GameObject root = FindSceneRoot(
                scene,
                EncounterRootName
            );

            if (root == null)
            {
                error =
                    "C07.16 encounter root is missing from the active scene.";
                return false;
            }

            return ValidateExistingEncounter(
                root,
                out error
            );
        }

        private static bool ValidateExistingEncounter(
            GameObject root,
            out string error)
        {
            if (root.GetComponent<
                    BDBossEncounterController>() == null)
            {
                error =
                    "C07.16 encounter controller is missing.";
                return false;
            }

            BDBossHealthGroup healthGroup =
                root.GetComponent<
                    BDBossHealthGroup>();

            if (healthGroup == null)
            {
                error =
                    "C07.16 health group is missing.";
                return false;
            }

            if (root.GetComponent<
                    BDBossEncounterRuntimeBindings>() == null)
            {
                error =
                    "C07.16 runtime bindings are missing.";
                return false;
            }

            if (root.GetComponent<
                    BDBossHealthHud>() == null)
            {
                error =
                    "C07.16 boss HUD is missing.";
                return false;
            }

            Transform bossTransform =
                FindChildRecursive(
                    root.transform,
                    BossBodyName
                );

            if (bossTransform == null)
            {
                error =
                    "C07.16 playable boss body is missing.";
                return false;
            }

            GameObject boss =
                bossTransform.gameObject;

            Type[] requiredBossComponents =
            {
                typeof(CharacterController),
                typeof(BDHealth),
                typeof(BDBossHealthChannel),
                typeof(BDBossHealthDamageBridge),
                typeof(BDC07FrameworkTestBoss)
            };

            for (int index = 0;
                 index < requiredBossComponents.Length;
                 index++)
            {
                Type type =
                    requiredBossComponents[index];

                if (boss.GetComponent(type) != null)
                    continue;

                error =
                    "C07.16 boss is missing " +
                    type.Name +
                    ".";

                return false;
            }

            Transform triggerTransform =
                FindChildRecursive(
                    root.transform,
                    TriggerName
                );

            if (triggerTransform == null ||
                triggerTransform.GetComponent<
                    BDBossArenaTrigger>() == null)
            {
                error =
                    "C07.16 arena entry trigger is missing.";
                return false;
            }

            healthGroup.RefreshChannels();

            if (healthGroup.Channels == null ||
                healthGroup.Channels.Count != 1)
            {
                error =
                    "C07.16 must expose exactly one boss health channel.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private static bool TrySelectArenaRoom(
            Transform player,
            out BDMinimapRoom selected,
            out string error)
        {
            selected = null;
            error = string.Empty;

            BDMinimapRoom[] rooms =
                UnityEngine.Object.FindObjectsByType<
                    BDMinimapRoom>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            if (rooms == null || rooms.Length < 2)
            {
                error =
                    "C07.16 requires at least two minimap rooms.";
                return false;
            }

            BDHealth[] healthObjects =
                UnityEngine.Object.FindObjectsByType<
                    BDHealth>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            float bestScore =
                float.NegativeInfinity;

            for (int roomIndex = 0;
                 roomIndex < rooms.Length;
                 roomIndex++)
            {
                BDMinimapRoom room =
                    rooms[roomIndex];

                if (room == null ||
                    room.RoomSize < 10f ||
                    room.ContainsWorldPosition(
                        player.position,
                        0.25f))
                {
                    continue;
                }

                float distanceScore =
                    room.SqrDistanceToCenter(
                        player.position
                    );

                int residentCombatants = 0;

                for (int healthIndex = 0;
                     healthIndex < healthObjects.Length;
                     healthIndex++)
                {
                    BDHealth health =
                        healthObjects[healthIndex];

                    if (health == null ||
                        health.GetComponentInParent<
                            BDPlayerMarker>() != null ||
                        health.GetComponentInParent<
                            BDHorseHealth>() != null)
                    {
                        continue;
                    }

                    if (room.ContainsWorldPosition(
                            health.transform.position,
                            0f))
                    {
                        residentCombatants++;
                    }
                }

                float score =
                    distanceScore -
                    residentCombatants * 10000f;

                if (score <= bestScore)
                    continue;

                bestScore = score;
                selected = room;
            }

            if (selected == null)
            {
                error =
                    "C07.16 could not find a non-start room large enough for the test encounter.";
                return false;
            }

            return true;
        }

        private static void RemoveResidentCombatants(
            BDMinimapRoom room)
        {
            BDHealth[] healthObjects =
                UnityEngine.Object.FindObjectsByType<
                    BDHealth>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < healthObjects.Length;
                 index++)
            {
                BDHealth health =
                    healthObjects[index];

                if (health == null ||
                    health.GetComponentInParent<
                        BDPlayerMarker>() != null ||
                    health.GetComponentInParent<
                        BDHorseHealth>() != null ||
                    !room.ContainsWorldPosition(
                        health.transform.position,
                        0f))
                {
                    continue;
                }

                UnityEngine.Object.DestroyImmediate(
                    health.gameObject
                );
            }
        }

        private static GameObject CreateBossBody(
            Transform parent,
            BDBossEncounterController encounter,
            Vector3 arenaCenter,
            float arenaRadius)
        {
            GameObject boss = new GameObject(
                BossBodyName
            );

            boss.transform.SetParent(parent, true);
            boss.transform.position = arenaCenter;

            CharacterController controller =
                boss.AddComponent<
                    CharacterController>();

            controller.center =
                new Vector3(0f, 1.35f, 0f);

            controller.height = 2.7f;
            controller.radius = 1.08f;
            controller.stepOffset = 0.25f;

            BDHealth health =
                boss.AddComponent<BDHealth>();

            SerializedObject healthObject =
                new SerializedObject(health);

            SerializedProperty maxHealth =
                healthObject.FindProperty(
                    "maxHealth"
                );

            if (maxHealth != null)
                maxHealth.floatValue = 180f;

            SerializedProperty destroyOnDeath =
                healthObject.FindProperty(
                    "destroyOnDeath"
                );

            if (destroyOnDeath != null)
                destroyOnDeath.boolValue = false;

            healthObject.ApplyModifiedPropertiesWithoutUndo();

            BDBossHealthChannel channel =
                boss.AddComponent<
                    BDBossHealthChannel>();

            channel.Configure(
                "framework-test-boss",
                "Framework Test Boss",
                180f,
                BDBossZeroHealthPolicy.DieImmediately,
                refill: true
            );

            BDBossHealthDamageBridge bridge =
                boss.AddComponent<
                    BDBossHealthDamageBridge>();

            bridge.Configure(
                "framework-test-boss",
                "Framework Test Boss",
                BDBossZeroHealthPolicy.DieImmediately
            );

            GameObject visual =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            visual.name = "Visual";
            visual.transform.SetParent(
                boss.transform,
                false
            );

            visual.transform.localPosition =
                new Vector3(0f, 1.35f, 0f);

            visual.transform.localScale =
                new Vector3(2.5f, 2.7f, 2.5f);

            Collider visualCollider =
                visual.GetComponent<Collider>();

            if (visualCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    visualCollider
                );
            }

            GameObject telegraph =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder
                );

            telegraph.name = "AttackTelegraph";
            telegraph.transform.SetParent(
                boss.transform,
                false
            );

            telegraph.transform.localPosition =
                new Vector3(0f, 0.04f, 0f);

            telegraph.transform.localScale =
                new Vector3(0.35f, 0.035f, 0.35f);

            Collider telegraphCollider =
                telegraph.GetComponent<Collider>();

            if (telegraphCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    telegraphCollider
                );
            }

            BDC07FrameworkTestBoss combatant =
                boss.AddComponent<
                    BDC07FrameworkTestBoss>();

            combatant.Configure(
                encounter,
                arenaCenter,
                arenaRadius
            );

            int enemyLayer =
                LayerMask.NameToLayer("Enemy");

            if (enemyLayer >= 0)
                SetLayerRecursively(boss, enemyLayer);

            return boss;
        }

        private static BDBossArenaTrigger CreateEntryTrigger(
            Transform parent,
            BDBossEncounterController encounter,
            Vector3 arenaCenter,
            float roomSize)
        {
            GameObject triggerObject =
                new GameObject(TriggerName);

            triggerObject.transform.SetParent(
                parent,
                true
            );

            triggerObject.transform.position =
                arenaCenter + Vector3.up * 1.5f;

            BoxCollider collider =
                triggerObject.AddComponent<
                    BoxCollider>();

            collider.isTrigger = true;

            float triggerSize = Mathf.Clamp(
                roomSize * 0.72f,
                7f,
                14f
            );

            collider.size =
                new Vector3(
                    triggerSize,
                    3.5f,
                    triggerSize
                );

            BDBossArenaTrigger trigger =
                triggerObject.AddComponent<
                    BDBossArenaTrigger>();

            trigger.Configure(encounter);
            return trigger;
        }

        private static void CreateArenaLabel(
            Transform parent,
            Vector3 arenaCenter,
            float roomSize)
        {
            GameObject labelObject =
                new GameObject(
                    "C07_FrameworkTestLabel"
                );

            labelObject.transform.SetParent(
                parent,
                true
            );

            labelObject.transform.position =
                arenaCenter +
                new Vector3(
                    0f,
                    0.10f,
                    -Mathf.Clamp(
                        roomSize * 0.30f,
                        3f,
                        6f
                    )
                );

            labelObject.transform.rotation =
                Quaternion.Euler(
                    90f,
                    0f,
                    0f
                );

            TextMesh label =
                labelObject.AddComponent<TextMesh>();

            label.text =
                "C07 FRAMEWORK TEST - ENTER AND DEFEAT THE BOSS";

            label.anchor =
                TextAnchor.MiddleCenter;

            label.alignment =
                TextAlignment.Center;

            label.characterSize = 0.18f;
            label.fontSize = 44;
        }

        private static float ResolveGroundY(
            Vector3 center,
            float fallbackY)
        {
            RaycastHit[] hits =
                Physics.RaycastAll(
                    center + Vector3.up * 10f,
                    Vector3.down,
                    30f,
                    ~0,
                    QueryTriggerInteraction.Ignore
                );

            float bestY =
                float.NegativeInfinity;

            for (int index = 0;
                 index < hits.Length;
                 index++)
            {
                RaycastHit hit = hits[index];

                if (hit.collider == null ||
                    Vector3.Angle(
                        hit.normal,
                        Vector3.up
                    ) > 55f)
                {
                    continue;
                }

                if (hit.point.y > bestY)
                    bestY = hit.point.y;
            }

            return float.IsNegativeInfinity(bestY)
                ? fallbackY
                : bestY;
        }

        private static GameObject FindSceneRoot(
            Scene scene,
            string objectName)
        {
            GameObject[] roots =
                scene.GetRootGameObjects();

            for (int index = 0;
                 index < roots.Length;
                 index++)
            {
                GameObject root = roots[index];

                if (root != null &&
                    root.name == objectName)
                {
                    return root;
                }
            }

            return null;
        }

        private static Transform FindChildRecursive(
            Transform root,
            string objectName)
        {
            if (root == null)
                return null;

            if (root.name == objectName)
                return root;

            for (int index = 0;
                 index < root.childCount;
                 index++)
            {
                Transform found =
                    FindChildRecursive(
                        root.GetChild(index),
                        objectName
                    );

                if (found != null)
                    return found;
            }

            return null;
        }

        private static void SetLayerRecursively(
            GameObject root,
            int layer)
        {
            root.layer = layer;

            for (int index = 0;
                 index < root.transform.childCount;
                 index++)
            {
                SetLayerRecursively(
                    root.transform
                        .GetChild(index)
                        .gameObject,
                    layer
                );
            }
        }
    }
}
