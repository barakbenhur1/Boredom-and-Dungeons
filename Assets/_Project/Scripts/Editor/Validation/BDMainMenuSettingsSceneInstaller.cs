using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDMainMenuSettingsSceneInstaller
    {
        public const string RootName =
            "B&D Main Menu And Settings";

        public static bool TryInstallActiveScene(
            out string error)
        {
            error = string.Empty;

            Scene scene =
                SceneManager.GetActiveScene();

            if (!scene.IsValid() ||
                !scene.isLoaded)
            {
                error =
                    "Main-menu installation requires a loaded scene.";
                return false;
            }

            GameObject root =
                FindSceneRoot(
                    scene,
                    RootName
                );

            if (root == null)
            {
                root = new GameObject(
                    RootName
                );

                SceneManager.MoveGameObjectToScene(
                    root,
                    scene
                );
            }

            int removedMissingScripts =
                RemoveMissingScriptsRecursively(root);

            BDMainMenuFlow flow =
                root.GetComponent<BDMainMenuFlow>();

            if (flow == null)
            {
                flow =
                    root.AddComponent<BDMainMenuFlow>();
            }

            BDSettingsAudioRouter audioRouter =
                root.GetComponent<BDSettingsAudioRouter>();

            if (audioRouter == null)
            {
                audioRouter =
                    root.AddComponent<BDSettingsAudioRouter>();
            }

            BDBBHBootIntro intro =
                root.GetComponent<BDBBHBootIntro>();

            if (intro == null)
            {
                intro =
                    root.AddComponent<BDBBHBootIntro>();
            }

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(flow);
            EditorUtility.SetDirty(audioRouter);
            EditorUtility.SetDirty(intro);
            EditorSceneManager.MarkSceneDirty(scene);

            if (removedMissingScripts > 0)
            {
                Debug.Log(
                    "B&D repaired " +
                    removedMissingScripts +
                    " missing main-menu script reference(s)."
                );
            }

            return ValidateActiveScene(
                out error
            );
        }

        public static bool ValidateActiveScene(
            out string error)
        {
            error = string.Empty;

            Scene scene =
                SceneManager.GetActiveScene();

            if (!scene.IsValid() ||
                !scene.isLoaded)
            {
                error =
                    "Main-menu validation has no loaded scene.";
                return false;
            }

            GameObject root =
                FindSceneRoot(
                    scene,
                    RootName
                );

            if (root == null)
            {
                error =
                    "Main-menu root is missing.";
                return false;
            }

            int missingScripts =
                CountMissingScriptsRecursively(root);

            if (missingScripts > 0)
            {
                error =
                    "Main-menu root still contains " +
                    missingScripts +
                    " missing script reference(s).";
                return false;
            }

            if (root.GetComponent<BDMainMenuFlow>() == null)
            {
                error =
                    "BDMainMenuFlow is missing.";
                return false;
            }

            if (root.GetComponent<
                    BDSettingsAudioRouter>() == null)
            {
                error =
                    "BDSettingsAudioRouter is missing.";
                return false;
            }

            if (root.GetComponent<BDBBHBootIntro>() == null)
            {
                error =
                    "BDBBHBootIntro is missing.";
                return false;
            }

            return true;
        }

        private static int RemoveMissingScriptsRecursively(
            GameObject root)
        {
            if (root == null)
                return 0;

            int removed = 0;

            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(
                    includeInactive: true
                );

            for (int index = 0;
                 index < transforms.Length;
                 index++)
            {
                Transform current = transforms[index];

                if (current == null)
                    continue;

                GameObject currentObject =
                    current.gameObject;

                int missing =
                    GameObjectUtility
                        .GetMonoBehavioursWithMissingScriptCount(
                            currentObject
                        );

                if (missing <= 0)
                    continue;

                GameObjectUtility
                    .RemoveMonoBehavioursWithMissingScript(
                        currentObject
                    );

                removed += missing;
                EditorUtility.SetDirty(currentObject);
            }

            return removed;
        }

        private static int CountMissingScriptsRecursively(
            GameObject root)
        {
            if (root == null)
                return 0;

            int count = 0;

            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(
                    includeInactive: true
                );

            for (int index = 0;
                 index < transforms.Length;
                 index++)
            {
                Transform current = transforms[index];

                if (current == null)
                    continue;

                count += GameObjectUtility
                    .GetMonoBehavioursWithMissingScriptCount(
                        current.gameObject
                    );
            }

            return count;
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
                GameObject root =
                    roots[index];

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
