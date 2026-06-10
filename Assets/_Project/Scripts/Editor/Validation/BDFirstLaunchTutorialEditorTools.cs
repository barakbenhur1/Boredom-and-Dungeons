#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDFirstLaunchTutorialEditorTools
    {
        private const string ResetMenuPath =
            "Boredom And Dungeons/Development/Reset First Launch Tutorial";

        [MenuItem(ResetMenuPath)]
        private static void ResetFirstLaunchTutorial()
        {
            BoredomAndDungeons.BDFirstLaunchTutorialStateStore.ResetForDevelopment();
            Debug.Log(
                "B&D FIRST LAUNCH TUTORIAL: state reset to NotStarted. " +
                "Stop and restart Play Mode to verify the clean-install flow."
            );
        }
    }
}
#endif
