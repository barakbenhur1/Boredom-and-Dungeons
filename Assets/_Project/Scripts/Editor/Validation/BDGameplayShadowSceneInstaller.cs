using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDGameplayShadowSceneInstaller
    {
        public const string PolicyRootName =
            "B&D Gameplay Shadow Policy";

        public static bool TryInstallActiveScene(
            out string error)
        {
            error = string.Empty;

            Scene scene = SceneManager.GetActiveScene();

            if (!scene.IsValid() || !scene.isLoaded)
            {
                error =
                    "Gameplay shadow policy requires a loaded scene.";
                return false;
            }

            GameObject root =
                FindSceneRoot(scene, PolicyRootName);

            if (root == null)
            {
                root = new GameObject(PolicyRootName);
                SceneManager.MoveGameObjectToScene(root, scene);
            }

            BDGameplayShadowPolicy policy =
                root.GetComponent<BDGameplayShadowPolicy>();

            if (policy == null)
                policy = root.AddComponent<BDGameplayShadowPolicy>();

            policy.Configure(
                distance: 22f,
                optionalRendererBudget: 28,
                refreshSeconds: 0.35f,
                discoverySeconds: 2.5f
            );

            policy.ApplyNow();

            EditorUtility.SetDirty(policy);
            EditorSceneManager.MarkSceneDirty(scene);

            return ValidateActiveScene(out error);
        }

        public static bool ValidateActiveScene(
            out string error)
        {
            error = string.Empty;

            Scene scene = SceneManager.GetActiveScene();

            if (!scene.IsValid() || !scene.isLoaded)
            {
                error =
                    "Gameplay shadow validation has no loaded scene.";
                return false;
            }

            GameObject root =
                FindSceneRoot(scene, PolicyRootName);

            if (root == null ||
                root.GetComponent<BDGameplayShadowPolicy>() == null)
            {
                error =
                    "Gameplay shadow policy root is missing.";
                return false;
            }

            Renderer[] renderers =
                Object.FindObjectsByType<Renderer>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            int requiredRenderers = 0;

            for (int index = 0;
                 index < renderers.Length;
                 index++)
            {
                Renderer renderer = renderers[index];

                if (BDGameplayShadowPolicy.ClassifyRenderer(
                        renderer) !=
                    BDGameplayShadowPolicy.ShadowClass.Required)
                {
                    continue;
                }

                requiredRenderers++;

                if (renderer.shadowCastingMode ==
                        ShadowCastingMode.Off ||
                    !renderer.receiveShadows)
                {
                    error =
                        "Required gameplay model does not cast and receive shadows: " +
                        renderer.name;
                    return false;
                }
            }

            if (requiredRenderers <= 0)
            {
                error =
                    "No required gameplay renderers were validated.";
                return false;
            }

            return true;
        }

        private static GameObject FindSceneRoot(
            Scene scene,
            string objectName)
        {
            GameObject[] roots = scene.GetRootGameObjects();

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
    }
}
