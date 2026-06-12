#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialFinalInputCombatPlayerV1011301QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string EntryPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string ProductionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";
        private const string PolishPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V1011Polish.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011301_INPUT_DAMAGE_WORLD_PROOF_MISSING",
                "currentKeyboard.leftArrowKey.isPressed",
                "currentKeyboard.rightArrowKey.isPressed",
                "Input.GetKey(KeyCode.LeftArrow)",
                "Input.GetKey(KeyCode.DownArrow)",
                "BeginFirstLaunchTutorialMeleeTransaction(",
                "targetActor.Health,",
                "IsFirstLaunchTutorialPrimaryEnemyDead()",
                "HasFirstLaunchTutorialAtomicSpinPairInRange()",
                "ResolveFirstLaunchTutorialAtomicSpinPairAtImpact()",
                "DidFirstLaunchTutorialDodgeCrossObjective()");
            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011301_INPUT_DAMAGE_WORLD_PROOF_MISSING",
                "HandleFirstLaunchTutorialMeleeLessonDeathAtImpact");
            Require(result, root, EntryPath,
                "TUTORIAL_V1011301_ENTRY_ARROW_INPUT_MISSING",
                "keyboard.upArrowKey.wasPressedThisFrame",
                "keyboard.downArrowKey.wasPressedThisFrame",
                "Input.GetKeyDown(KeyCode.UpArrow)",
                "Input.GetKeyDown(KeyCode.DownArrow)");
            Require(result, root, ProductionPath,
                "TUTORIAL_V1011301_ACTOR_TRANSACTION_MISSING",
                "PrepareFirstLaunchTutorialPrimaryMeleeTarget(",
                "TryGetFirstLaunchTutorialAtomicSpinPair(",
                "ApplyFirstLaunchTutorialActorDamage(left, left.Health)",
                "ApplyFirstLaunchTutorialActorDamage(right, right.Health)");
            Require(result, root, PolishPath,
                "TUTORIAL_V1011301_SIMPLE_PLAYER_MISSING",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "Positive X is authored facing right");

            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011301_MALFORMED_OR_LEGACY_INPUT",
                "RightArrowownArrow",
                "firstLaunchTutorialWorldMousePrimaryCapturedV101122",
                "firstLaunchTutorialWorldMouseSecondaryCapturedV101122",
                "firstLaunchTutorialWorldMouseRangedCapturedV101122");
            Forbid(result, root, EntryPath,
                "TUTORIAL_V1011301_MALFORMED_ENTRY_INPUT",
                "RightArrowownArrow");
            Forbid(result, root, PolishPath,
                "TUTORIAL_V1011301_ARTICULATED_PLAYER_HOOK_REGRESSION",
                "UpdateFirstLaunchTutorialArticulatedPlayer");
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
