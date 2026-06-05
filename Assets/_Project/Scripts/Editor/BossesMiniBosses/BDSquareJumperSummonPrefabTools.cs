#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDSquareJumperSummonPrefabTools
    {
        private const string MenuPath =
            "Boredom And Dungeons/Bosses/Mini-Bosses/" +
            "Create Square Jumper Summon Prefabs";

        private const string PrefabFolder =
            "Assets/_Project/Prefabs/Enemies/Regular/SquareJumperSummons";

        [MenuItem(MenuPath, priority = 182)]
        private static void CreateSummonPrefabs()
        {
            Directory.CreateDirectory(PrefabFolder);

            CreatePrefabIfMissing(
                "BD_Summon_SwordEnemy.prefab",
                "BDSwordEnemy",
                PrimitiveType.Capsule,
                new Color(0.82f, 0.82f, 0.92f, 1f)
            );

            CreatePrefabIfMissing(
                "BD_Summon_ShooterEnemy.prefab",
                "BDEnemyShooter",
                PrimitiveType.Cylinder,
                new Color(0.25f, 0.70f, 1f, 1f),
                fallbackTypeName: "BDRangedEnemy"
            );

            CreatePrefabIfMissing(
                "BD_Summon_PatrolEnemy.prefab",
                "BDPatrolEnemy",
                PrimitiveType.Cube,
                new Color(0.40f, 0.95f, 0.36f, 1f)
            );

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "Square Jumper summon prefabs are ready. " +
                "Assign them to BDSquareJumperMiniBoss.Summon Prefabs " +
                "if auto discovery does not find them."
            );
        }

        private static void CreatePrefabIfMissing(
            string fileName,
            string primaryTypeName,
            PrimitiveType visualPrimitive,
            Color color,
            string fallbackTypeName = null)
        {
            string path = $"{PrefabFolder}/{fileName}";

            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = new GameObject(
                Path.GetFileNameWithoutExtension(fileName)
            );

            CharacterController controller =
                root.AddComponent<CharacterController>();

            controller.radius = 0.45f;
            controller.height = 1.65f;
            controller.center = new Vector3(0f, 0.82f, 0f);

            BDHealth health = root.AddComponent<BDHealth>();
            health.SetMaxHealth(35f, refill: true);

            BDCombatantProfile profile =
                root.AddComponent<BDCombatantProfile>();

            profile.ConfigureRegularEnemy();

            Type aiType =
                ResolveRuntimeType(primaryTypeName);

            if (aiType == null &&
                !string.IsNullOrWhiteSpace(fallbackTypeName))
            {
                aiType = ResolveRuntimeType(fallbackTypeName);
            }

            if (aiType != null &&
                typeof(MonoBehaviour).IsAssignableFrom(aiType))
            {
                root.AddComponent(aiType);
            }
            else
            {
                Debug.LogWarning(
                    $"Could not find runtime AI type {primaryTypeName}. " +
                    "Prefab was created with Health/Profile only; assign " +
                    "the correct enemy behaviour manually."
                );
            }

            GameObject visual =
                GameObject.CreatePrimitive(visualPrimitive);

            visual.name = "Visual";
            visual.transform.SetParent(root.transform, false);
            visual.transform.localPosition = new Vector3(0f, 0.82f, 0f);
            visual.transform.localScale = new Vector3(0.9f, 1.45f, 0.9f);

            Collider visualCollider = visual.GetComponent<Collider>();

            if (visualCollider != null)
                UnityEngine.Object.DestroyImmediate(visualCollider);

            Renderer renderer = visual.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = CreateMaterialAsset(path, color);

            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static Type ResolveRuntimeType(string shortTypeName)
        {
            string fullName =
                $"BoredomAndDungeons.{shortTypeName}";

            Type type = Type.GetType(fullName);

            if (type != null)
                return type;

            foreach (System.Reflection.Assembly assembly
                     in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(fullName);

                if (type != null)
                    return type;
            }

            return null;
        }

        private static Material CreateMaterialAsset(
            string prefabPath,
            Color color)
        {
            string materialPath =
                prefabPath.Replace(".prefab", ".mat");

            Material existing =
                AssetDatabase.LoadAssetAtPath<Material>(
                    materialPath
                );

            if (existing != null)
                return existing;

            Shader shader =
                Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 0.65f);
            }

            AssetDatabase.CreateAsset(material, materialPath);
            return material;
        }
    }
}
#endif
