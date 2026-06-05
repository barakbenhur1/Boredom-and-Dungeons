#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDSquareJumperSetupTools
    {
        private const string ExistingName =
            "BD_SquareJumper_Encounter";

        [MenuItem(
            "Boredom And Dungeons/Bosses/Mini-Bosses/" +
            "Create Square Jumper Prototype",
            priority = 180)]
        private static void CreatePrototype()
        {
            GameObject parent = Selection.activeGameObject;

            if (parent != null)
            {
                Transform existing =
                    parent.transform.Find(ExistingName);

                if (existing != null)
                {
                    Selection.activeGameObject =
                        existing.gameObject;

                    Debug.LogWarning(
                        "Square Jumper prototype already exists " +
                        "under the selected object. No duplicate " +
                        "was created.",
                        existing.gameObject
                    );

                    return;
                }
            }

            GameObject root =
                new GameObject(ExistingName);

            Undo.RegisterCreatedObjectUndo(
                root,
                "Create Square Jumper Prototype"
            );

            if (parent != null)
            {
                root.transform.SetParent(
                    parent.transform,
                    false
                );
            }

            BDBossEncounterController encounter =
                Undo.AddComponent<
                    BDBossEncounterController
                >(root);

            BDBossHealthGroup healthGroup =
                Undo.AddComponent<
                    BDBossHealthGroup
                >(root);

            BDBossSharedSummonBudget summonBudget =
                Undo.AddComponent<
                    BDBossSharedSummonBudget
                >(root);

            BDBossEncounterRuntimeBindings bindings =
                Undo.AddComponent<
                    BDBossEncounterRuntimeBindings
                >(root);

            summonBudget.Configure(
                encounter,
                maximumAlive: 8
            );

            GameObject boss =
                CreateBossBody(root.transform);

            BDSquareJumperMiniBoss squareJumper =
                boss.GetComponent<
                    BDSquareJumperMiniBoss
                >();

            squareJumper.ConfigureEncounter(
                encounter,
                summonBudget
            );

            Transform[] spawnPoints =
                CreateSummonPoints(root.transform);

            GameObject[] discoveredPrefabs =
                DiscoverRegularEnemyPrefabs();

            squareJumper.ConfigureSummons(
                discoveredPrefabs,
                spawnPoints
            );

            BDBossHealthChannel channel =
                boss.GetComponent<
                    BDBossHealthChannel
                >();

            BDBossHealthDamageBridge bridge =
                boss.GetComponent<
                    BDBossHealthDamageBridge
                >();

            channel.Configure(
                "square-jumper",
                "Square Jumper",
                650f,
                BDBossZeroHealthPolicy.DieImmediately,
                refill: true
            );

            bridge.Configure(
                "square-jumper",
                "Square Jumper",
                BDBossZeroHealthPolicy.DieImmediately
            );

            healthGroup.RefreshChannels();

            BDBossArenaTrigger arenaTrigger =
                CreateArenaTrigger(
                    root.transform,
                    encounter
                );

            BDBossRewardChest rewardChest =
                CreateRewardChest(root.transform);

            ConfigureBindings(
                bindings,
                encounter,
                healthGroup,
                rewardChest,
                arenaTrigger,
                squareJumper
            );

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(squareJumper);
            EditorUtility.SetDirty(healthGroup);
            EditorUtility.SetDirty(bindings);

            EditorSceneManager.MarkSceneDirty(root.scene);

            Selection.activeGameObject = root;
            SceneView.lastActiveSceneView?.FrameSelected();

            Debug.Log(
                "Created working Square Jumper prototype. " +
                $"Auto-discovered regular enemy prefabs: " +
                $"{discoveredPrefabs.Length}. " +
                "Assign missing Sword/Shooter/Patrol prefabs " +
                "in BDSquareJumperMiniBoss if needed.",
                root
            );
        }

        [MenuItem(
            "Boredom And Dungeons/Bosses/Mini-Bosses/" +
            "Validate Selected Square Jumper",
            priority = 181)]
        private static void ValidateSelected()
        {
            GameObject selected =
                Selection.activeGameObject;

            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "Square Jumper Validation",
                    "Select the Square Jumper encounter root.",
                    "OK"
                );
                return;
            }

            BDSquareJumperMiniBoss boss =
                selected.GetComponentInChildren<
                    BDSquareJumperMiniBoss
                >(includeInactive: true);

            List<string> errors = new List<string>();

            if (boss == null)
            {
                errors.Add(
                    "BDSquareJumperMiniBoss is missing."
                );
            }

            if (selected.GetComponentInChildren<
                    BDBossEncounterController
                >(includeInactive: true) == null)
            {
                errors.Add(
                    "BDBossEncounterController is missing."
                );
            }

            if (selected.GetComponentInChildren<
                    BDBossHealthGroup
                >(includeInactive: true) == null)
            {
                errors.Add(
                    "BDBossHealthGroup is missing."
                );
            }

            if (selected.GetComponentInChildren<
                    BDBossRewardChest
                >(includeInactive: true) == null)
            {
                errors.Add(
                    "BDBossRewardChest is missing."
                );
            }

            if (boss != null)
            {
                BDHealth health =
                    boss.GetComponent<BDHealth>();

                BDCombatantProfile profile =
                    boss.GetComponent<
                        BDCombatantProfile
                    >();

                if (health == null)
                    errors.Add("BDHealth is missing.");

                if (profile == null)
                {
                    errors.Add(
                        "BDCombatantProfile is missing."
                    );
                }
                else if (
                    profile.Rank !=
                    BDCombatantRank.MiniBoss)
                {
                    errors.Add(
                        "Combatant rank is not MiniBoss."
                    );
                }
                else if (
                    profile
                        .ReceivesPlayerProjectileKnockback)
                {
                    errors.Add(
                        "Large mini-boss must be immune " +
                        "to player projectile knockback."
                    );
                }
            }

            if (errors.Count == 0)
            {
                Debug.Log(
                    "Square Jumper validation passed.",
                    selected
                );

                EditorUtility.DisplayDialog(
                    "Square Jumper Validation",
                    "PASS",
                    "OK"
                );
            }
            else
            {
                string message =
                    string.Join("\n", errors);

                Debug.LogError(
                    "Square Jumper validation failed:\n" +
                    message,
                    selected
                );

                EditorUtility.DisplayDialog(
                    "Square Jumper Validation",
                    "BLOCKED\n\n" + message,
                    "OK"
                );
            }
        }

        private static GameObject CreateBossBody(
            Transform parent)
        {
            GameObject boss =
                new GameObject("SquareJumper_Body");

            Undo.RegisterCreatedObjectUndo(
                boss,
                "Create Square Jumper Body"
            );

            boss.transform.SetParent(parent, false);
            boss.transform.localPosition =
                new Vector3(0f, 0f, 2.5f);

            CharacterController controller =
                Undo.AddComponent<
                    CharacterController
                >(boss);

            controller.radius = 1.25f;
            controller.height = 2.4f;
            controller.center =
                new Vector3(0f, 1.2f, 0f);
            controller.stepOffset = 0.25f;

            BDHealth health =
                Undo.AddComponent<BDHealth>(boss);

            health.SetMaxHealth(
                650f,
                refill: true
            );

            BDCombatantProfile profile =
                Undo.AddComponent<
                    BDCombatantProfile
                >(boss);

            profile.ConfigureLargeMiniBoss();

            Undo.AddComponent<
                BDBossHealthChannel
            >(boss);

            Undo.AddComponent<
                BDBossHealthDamageBridge
            >(boss);

            BDSquareJumperMiniBoss ai =
                Undo.AddComponent<
                    BDSquareJumperMiniBoss
                >(boss);

            GameObject visualRoot =
                new GameObject("Visual");

            Undo.RegisterCreatedObjectUndo(
                visualRoot,
                "Create Square Jumper Visual"
            );

            visualRoot.transform.SetParent(
                boss.transform,
                false
            );

            visualRoot.transform.localPosition =
                new Vector3(0f, 1.2f, 0f);

            GameObject body =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            Undo.RegisterCreatedObjectUndo(
                body,
                "Create Square Jumper Body Visual"
            );

            body.name = "LargeSquareBody";
            body.transform.SetParent(
                visualRoot.transform,
                false
            );

            body.transform.localScale =
                new Vector3(2.6f, 2.25f, 2.4f);

            Collider visualCollider =
                body.GetComponent<Collider>();

            if (visualCollider != null)
                Undo.DestroyObjectImmediate(
                    visualCollider
                );

            ApplyMaterial(
                body,
                new Color(
                    0.28f,
                    0.12f,
                    0.08f,
                    1f
                )
            );

            CreateFaceDetail(
                visualRoot.transform,
                new Vector3(-0.52f, 0.28f, 1.22f)
            );

            CreateFaceDetail(
                visualRoot.transform,
                new Vector3(0.52f, 0.28f, 1.22f)
            );

            ai.ConfigureVisualRoot(
                visualRoot.transform
            );

            return boss;
        }

        private static void CreateFaceDetail(
            Transform parent,
            Vector3 localPosition)
        {
            GameObject eye =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            Undo.RegisterCreatedObjectUndo(
                eye,
                "Create Square Jumper Eye"
            );

            eye.name = "Eye";
            eye.transform.SetParent(parent, false);
            eye.transform.localPosition = localPosition;
            eye.transform.localScale =
                new Vector3(0.28f, 0.28f, 0.14f);

            Collider collider =
                eye.GetComponent<Collider>();

            if (collider != null)
                Undo.DestroyObjectImmediate(collider);

            ApplyMaterial(
                eye,
                new Color(1f, 0.62f, 0.08f, 1f)
            );
        }

        private static Transform[] CreateSummonPoints(
            Transform parent)
        {
            GameObject root =
                new GameObject("SummonPoints");

            Undo.RegisterCreatedObjectUndo(
                root,
                "Create Summon Points"
            );

            root.transform.SetParent(parent, false);

            Transform[] result = new Transform[8];

            for (int i = 0; i < result.Length; i++)
            {
                float angle =
                    i * (360f / result.Length);

                Vector3 direction =
                    Quaternion.Euler(
                        0f,
                        angle,
                        0f
                    ) *
                    Vector3.forward;

                GameObject point =
                    new GameObject(
                        $"SummonPoint_{i + 1:00}"
                    );

                Undo.RegisterCreatedObjectUndo(
                    point,
                    "Create Summon Point"
                );

                point.transform.SetParent(
                    root.transform,
                    false
                );

                point.transform.localPosition =
                    direction * 4.2f;

                point.transform.localRotation =
                    Quaternion.LookRotation(
                        -direction,
                        Vector3.up
                    );

                result[i] = point.transform;
            }

            return result;
        }

        private static BDBossArenaTrigger
            CreateArenaTrigger(
                Transform parent,
                BDBossEncounterController encounter)
        {
            GameObject triggerObject =
                new GameObject("ArenaTrigger");

            Undo.RegisterCreatedObjectUndo(
                triggerObject,
                "Create Boss Arena Trigger"
            );

            triggerObject.transform.SetParent(
                parent,
                false
            );

            BoxCollider collider =
                Undo.AddComponent<BoxCollider>(
                    triggerObject
                );

            collider.isTrigger = true;
            collider.size =
                new Vector3(14f, 3f, 14f);
            collider.center =
                new Vector3(0f, 1.5f, 0f);

            BDBossArenaTrigger trigger =
                Undo.AddComponent<
                    BDBossArenaTrigger
                >(triggerObject);

            trigger.Configure(encounter);
            return trigger;
        }

        private static BDBossRewardChest
            CreateRewardChest(Transform parent)
        {
            GameObject chest =
                new GameObject("RewardChest");

            Undo.RegisterCreatedObjectUndo(
                chest,
                "Create Reward Chest"
            );

            chest.transform.SetParent(parent, false);
            chest.transform.localPosition =
                new Vector3(0f, 0f, 5.8f);

            BDBossRewardChest chestLogic =
                Undo.AddComponent<
                    BDBossRewardChest
                >(chest);

            GameObject baseVisual =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            Undo.RegisterCreatedObjectUndo(
                baseVisual,
                "Create Chest Base"
            );

            baseVisual.name = "Base";
            baseVisual.transform.SetParent(
                chest.transform,
                false
            );

            baseVisual.transform.localPosition =
                new Vector3(0f, 0.35f, 0f);

            baseVisual.transform.localScale =
                new Vector3(1.7f, 0.7f, 1.15f);

            ApplyMaterial(
                baseVisual,
                new Color(0.26f, 0.12f, 0.04f, 1f)
            );

            GameObject lid =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            Undo.RegisterCreatedObjectUndo(
                lid,
                "Create Chest Lid"
            );

            lid.name = "Lid";
            lid.transform.SetParent(
                chest.transform,
                false
            );

            lid.transform.localPosition =
                new Vector3(0f, 0.82f, -0.48f);

            lid.transform.localScale =
                new Vector3(1.75f, 0.36f, 1.18f);

            ApplyMaterial(
                lid,
                new Color(0.42f, 0.20f, 0.06f, 1f)
            );

            GameObject reward =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            Undo.RegisterCreatedObjectUndo(
                reward,
                "Create Chest Reward"
            );

            reward.name = "RewardPlaceholder";
            reward.transform.SetParent(
                chest.transform,
                false
            );

            reward.transform.localPosition =
                new Vector3(0f, 0.75f, 0f);

            reward.transform.localScale =
                Vector3.one * 0.42f;

            ApplyMaterial(
                reward,
                new Color(0.20f, 0.95f, 1f, 1f)
            );

            Collider rewardCollider =
                reward.GetComponent<Collider>();

            if (rewardCollider != null)
                rewardCollider.isTrigger = true;

            SerializedObject serialized =
                new SerializedObject(chestLogic);

            serialized.FindProperty("lid")
                .objectReferenceValue = lid.transform;

            serialized.FindProperty("rewardObject")
                .objectReferenceValue = reward;

            serialized.FindProperty("rewardCollider")
                .objectReferenceValue =
                    rewardCollider;

            serialized.ApplyModifiedPropertiesWithoutUndo();

            return chestLogic;
        }

        private static void ConfigureBindings(
            BDBossEncounterRuntimeBindings bindings,
            BDBossEncounterController encounter,
            BDBossHealthGroup healthGroup,
            BDBossRewardChest chest,
            BDBossArenaTrigger trigger,
            BDSquareJumperMiniBoss boss)
        {
            SerializedObject serialized =
                new SerializedObject(bindings);

            serialized.FindProperty("encounter")
                .objectReferenceValue = encounter;

            serialized.FindProperty("healthGroup")
                .objectReferenceValue = healthGroup;

            serialized.FindProperty("rewardChest")
                .objectReferenceValue = chest;

            SerializedProperty triggers =
                serialized.FindProperty("arenaTriggers");

            triggers.arraySize = 1;

            triggers
                .GetArrayElementAtIndex(0)
                .objectReferenceValue = trigger;

            SerializedProperty behaviours =
                serialized.FindProperty(
                    "bossCombatBehaviours"
                );

            behaviours.arraySize = 1;

            behaviours
                .GetArrayElementAtIndex(0)
                .objectReferenceValue = boss;

            serialized.ApplyModifiedPropertiesWithoutUndo();
            bindings.RefreshReferences();
        }

        private static GameObject[]
            DiscoverRegularEnemyPrefabs()
        {
            string[] prefabGuids =
                AssetDatabase.FindAssets(
                    "t:Prefab",
                    new[] { "Assets/_Project" }
                );

            Dictionary<string, GameObject> roles =
                new Dictionary<string, GameObject>
                {
                    { "Sword", null },
                    { "Shooter", null },
                    { "Patrol", null }
                };

            for (int i = 0;
                 i < prefabGuids.Length;
                 i++)
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(
                        prefabGuids[i]
                    );

                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject
                    >(path);

                if (prefab == null)
                    continue;

                MonoBehaviour[] behaviours =
                    prefab.GetComponentsInChildren<
                        MonoBehaviour
                    >(includeInactive: true);

                for (int b = 0;
                     b < behaviours.Length;
                     b++)
                {
                    MonoBehaviour behaviour =
                        behaviours[b];

                    if (behaviour == null)
                        continue;

                    string typeName =
                        behaviour.GetType().Name;

                    if (roles["Sword"] == null &&
                        typeName.IndexOf(
                            "SwordEnemy",
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0)
                    {
                        roles["Sword"] = prefab;
                    }

                    if (roles["Shooter"] == null &&
                        (typeName.IndexOf(
                            "Shooter",
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0 ||
                         typeName.IndexOf(
                            "RangedEnemy",
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0))
                    {
                        roles["Shooter"] = prefab;
                    }

                    if (roles["Patrol"] == null &&
                        typeName.IndexOf(
                            "Patrol",
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0)
                    {
                        roles["Patrol"] = prefab;
                    }
                }
            }

            List<GameObject> discovered =
                new List<GameObject>();

            foreach (string role in new[]
                     {
                         "Sword",
                         "Shooter",
                         "Patrol"
                     })
            {
                if (roles[role] != null &&
                    !discovered.Contains(roles[role]))
                {
                    discovered.Add(roles[role]);
                }
            }

            return discovered.ToArray();
        }

        // BD SQUARE JUMPER MATERIAL-ASSET FIX
        private const string GeneratedMaterialRoot =
            "Assets/_Project/Materials/Generated/SquareJumper";

        private static void ApplyMaterial(
            GameObject visual,
            Color color)
        {
            Renderer renderer =
                visual.GetComponent<Renderer>();

            if (renderer == null)
                return;

            renderer.sharedMaterial =
                GetOrCreateGeneratedMaterial(color);
        }

        private static Material GetOrCreateGeneratedMaterial(
            Color color)
        {
            EnsureGeneratedMaterialFolder();

            string colorKey =
                ColorUtility.ToHtmlStringRGBA(color);

            string path =
                $"{GeneratedMaterialRoot}/" +
                $"SquareJumper_{colorKey}.mat";

            Material material =
                AssetDatabase.LoadAssetAtPath<Material>(
                    path
                );

            if (material != null)
                return material;

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            material = new Material(shader)
            {
                name = $"SquareJumper_{colorKey}",
                color = color
            };

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(
                    "_EmissionColor",
                    color * 0.65f
                );
            }

            AssetDatabase.CreateAsset(material, path);
            AssetDatabase.SaveAssets();

            return material;
        }

        private static void EnsureGeneratedMaterialFolder()
        {
            EnsureFolder(
                "Assets/_Project/Materials"
            );

            EnsureFolder(
                "Assets/_Project/Materials/Generated"
            );

            EnsureFolder(GeneratedMaterialRoot);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            int slash = path.LastIndexOf('/');

            if (slash <= 0)
                return;

            string parent = path.Substring(0, slash);
            string name = path.Substring(slash + 1);

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
#endif
