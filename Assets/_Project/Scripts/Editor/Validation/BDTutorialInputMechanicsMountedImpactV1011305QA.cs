#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialInputMechanicsMountedImpactV1011305QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string EntryPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string ProductionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011305_FULL_INPUT_AND_MECHANICS_MISSING",
                "currentKeyboard.aKey.isPressed",
                "currentKeyboard.dKey.isPressed",
                "currentKeyboard.wKey.isPressed",
                "currentKeyboard.sKey.isPressed",
                "Input.GetKey(KeyCode.A)",
                "Input.GetKey(KeyCode.D)",
                "Input.GetKey(KeyCode.W)",
                "Input.GetKey(KeyCode.S)",
                "ReadFirstLaunchTutorialInteractPressed()",
                "ReadFirstLaunchTutorialRangedPressed()",
                "ShouldRouteFirstLaunchTutorialProgressionToHealing()",
                "UpdateFirstLaunchTutorialSpinningHold();",
                "UpdateFirstLaunchTutorialGrappleHoldFreePlay();",
                "BeginFirstLaunchTutorialMeleeTransaction(",
                "targetActor.Health,",
                "ResolveFirstLaunchTutorialFreeSpinAtImpact()");
            Require(result, root, EntryPath,
                "TUTORIAL_V1011305_ENTRY_WASD_MISSING",
                "keyboard.wKey.wasPressedThisFrame",
                "keyboard.sKey.wasPressedThisFrame",
                "Input.GetKeyDown(KeyCode.W)",
                "Input.GetKeyDown(KeyCode.S)");
            Require(result, root, ProductionPath,
                "TUTORIAL_V1011305_GLOBAL_MECHANICS_AND_RAM_MISSING",
                "Held light/spin and heavy/grapple arbitration is global",
                "FindClosestLivingTutorialActor(180f, requireForward: false)",
                "EnsureFirstLaunchTutorialMountedImpactTarget()",
                "ResolveFirstLaunchTutorialFreeSpinAtImpact()");
            Require(result, root, RepairPath,
                "TUTORIAL_V1011305_MOUNTED_IMPACT_COLLISION_BYPASS_MISSING",
                "firstLaunchTutorialMounted &&",
                "FirstLaunchTutorialStep.MountedImpact",
                "return requestedX;");
            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011305_ARROW_ONLY_OR_STEP_LOCK_REGRESSION",
                "bool healingLesson =",
                "currentKeyboard.leftArrowKey.isPressed)\n                    keyboard.x -= 1f;");
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relativePath,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relativePath);
            if (!File.Exists(absolute))
            {
                Add(result, code, relativePath, "Required file is missing.");
                return;
            }
            string text = File.ReadAllText(absolute);
            for (int index = 0; index < tokens.Length; index++)
            {
                if (!text.Contains(tokens[index]))
                    Add(result, code, relativePath,
                        "Missing required contract token: " + tokens[index]);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relativePath,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relativePath);
            if (!File.Exists(absolute))
            {
                Add(result, code, relativePath, "Required file is missing.");
                return;
            }
            string text = File.ReadAllText(absolute);
            for (int index = 0; index < tokens.Length; index++)
            {
                if (text.Contains(tokens[index]))
                    Add(result, code, relativePath,
                        "Forbidden regression token remains: " + tokens[index]);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string path,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                path,
                string.Empty,
                message
            ));
        }
    }
}
#endif
