#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialHorseFreeOpeningPetSuppressionV1011327QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string ScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string CoursePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";
        private const string VisibilityPath =
            "Assets/_Project/Scripts/Runtime/UI/BDGameplayUiVisibility.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, CoursePath,
                "TUTORIAL_V1011327_OPENING_NOT_HORSE_FREE",
                "BD HORSE-FREE OPENING HANDOFF V10.11.30.27",
                "SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.AttackEnemy)");
            Forbid(result, root, CoursePath,
                "TUTORIAL_V1011327_JUMP_TO_HORSE_REGRESSION",
                "SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.MountHorse)",
                "JUMP CLEARED — MOUNT THE HORSE");

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011327_HORSE_VISIBILITY_OR_ORDER_MISSING",
                "BD HORSE-FREE OPENING VISIBILITY V10.11.30.27",
                "BD DEFERRED HORSE ROOM LAYOUT V10.11.30.27",
                "case FirstLaunchTutorialStep.AttackEnemy:\n                    roomIndex = 1;",
                "case FirstLaunchTutorialStep.Parry:\n                    roomIndex = 4;",
                "case FirstLaunchTutorialStep.MountHorse:\n                case FirstLaunchTutorialStep.RideHorse:\n                    roomIndex = 5;",
                "case FirstLaunchTutorialStep.EnemyArrival:\n                case FirstLaunchTutorialStep.HorseShot:\n                    roomIndex = 6;");
            Forbid(result, root, ScreensPath,
                "TUTORIAL_V1011327_OPENING_HORSE_VISIBILITY_REGRESSION",
                "step == FirstLaunchTutorialStep.WhiteBoot ||\n                step == FirstLaunchTutorialStep.Move ||\n                step == FirstLaunchTutorialStep.Jump ||\n                step == FirstLaunchTutorialStep.MountHorse");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011327_DEFERRED_HORSE_FLOW_MISSING",
                "BD DEFERRED HORSE LESSON V10.11.30.27",
                "FirstLaunchTutorialStep.MountHorse",
                "FirstLaunchTutorialStep.EnemyArrival",
                "UpdateFirstLaunchTutorialHorseShotEvent",
                "FirstLaunchTutorialStep.JumpAttack");

            Require(result, root, PresenterPath,
                "TUTORIAL_V1011327_FIRST_LAUNCH_HUD_GATE_MISSING",
                "SuppressFirstLaunchGameplayHud",
                "firstLaunchTutorialActive",
                "ShouldReserveFirstLaunchTutorialPresentation()");
            Require(result, root, VisibilityPath,
                "TUTORIAL_V1011327_PET_PROMPT_SUPPRESSION_MISSING",
                "BD FIRST-LAUNCH PET/HUD HARD GATE V10.11.30.27",
                "SuppressFirstLaunchGameplayHud",
                "PET card in the upper-right corner");
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string full = Path.Combine(root, relative);
            if (!File.Exists(full))
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }
            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (!source.Contains(token))
                    Add(result, code, relative,
                        "Missing required contract token: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string full = Path.Combine(root, relative);
            if (!File.Exists(full))
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }
            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (source.Contains(token))
                    Add(result, code, relative,
                        "Forbidden regression token remains: " + token);
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
