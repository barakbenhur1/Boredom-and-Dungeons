using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static partial class BDCreateCleanMazePrototypeScene
    {
        private const string CleanMazeBuilderStage = "V115 Scene Builder Decomposition";
        private const string CleanMazeBuilderSafetyRule = "This partial file must not change generated gameplay behavior.";

        private static void LogSceneBuilderStageInfo()
        {
            Debug.Log($"B&D Scene Builder Stage: {CleanMazeBuilderStage} — {CleanMazeBuilderSafetyRule}");
        }
    }
}
