#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialFlowCoherenceV1011328QA
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
        private const string ContractsPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonContractsV101128.cs";
        private const string ActionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ActionPresentation.cs";
        private const string FinalPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.FinalProductionPass.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, ContractsPath,
                "TUTORIAL_V1011328_OPENING_FACING_RELEASE_MISSING",
                "BD OPENING FACING RELEASE V10.11.30.28",
                "if (obstacleDelta > 1f)",
                "return 1f;");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011328_PERMANENT_JUMP_MISSING",
                "BD PERMANENT LEARNED JUMP V10.11.30.28",
                "IsFirstLaunchTutorialMechanicUnlocked(2)",
                "!firstLaunchTutorialContinuousHandoffActive");
            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011328_POST_LESSON_JUMP_BLOCKED",
                "!firstLaunchTutorialMounted &&\n                !firstLaunchTutorialLessonCompleteAwaitingTravel &&\n                !firstLaunchTutorialContinuousHandoffActive");
            Forbid(result, root, CoursePath,
                "TUTORIAL_V1011328_REQUEST_JUMP_TRAVEL_BLOCKED",
                "!IsFirstLaunchTutorialMechanicUnlocked(2) ||\n                firstLaunchTutorialLessonCompleteAwaitingTravel ||\n                firstLaunchTutorialContinuousHandoffActive");

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011328_IMMEDIATE_REMOUNT_MISSING",
                "BD IMMEDIATE POST-HEAL REMOUNT V10.11.30.28",
                "current == FirstLaunchTutorialStep.HealHorse &&\n                next == FirstLaunchTutorialStep.RemountHorse",
                "case FirstLaunchTutorialStep.HorseReturn:\n                case FirstLaunchTutorialStep.HealHorse:\n                case FirstLaunchTutorialStep.RemountHorse:\n                    roomIndex = 8;");

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011329_POST_SCROLL_HORSE_RETURN_MISSING",
                "BD POST-SCROLL HORSE RETURN V10.11.30.29",
                "TutorialHorseReturnPostHandoffDelaySeconds = 0.14f",
                "firstLaunchTutorialHorse.gameObject.SetActive(false)",
                "firstLaunchTutorialPlayerWorldPosition +\n                        new Vector2(-320f, -8f)");
            Require(result, root, ActionPath,
                "TUTORIAL_V1011328_HORSE_HIT_FIRST_FRAME_MISSING",
                "BD HORSE HIT FIRST-FRAME POSE V10.11.30.28",
                "UpdateFirstLaunchTutorialHorseHitPresentation(0f);");

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011328_ROOM_PRELOAD_MISSING",
                "BD PRELOADED ROOM VISUALS V10.11.30.28",
                "PrepareFirstLaunchTutorialRoomVisualsForHandoff(",
                "firstLaunchTutorialHandoffHorseStagedInTargetRoom",
                "Canvas.ForceUpdateCanvases();",
                "RenderFirstLaunchTutorialFreePlayCourse(force: true);");

            Require(result, root, FinalPath,
                "TUTORIAL_V1011328_CONTEXTUAL_GEOMETRY_MISSING",
                "BD CONTEXTUAL COURSE GEOMETRY V10.11.30.28",
                "bool wallJumpRoom =",
                "bool finishGateRoom =",
                "gateOpeningAnimationVisible",
                "finishGateRoom &&");
            Forbid(result, root, FinalPath,
                "TUTORIAL_V1011328_GLOBAL_GATE_OR_WALL_REGRESSION",
                "firstLaunchTutorialWallJumpWall.gameObject.SetActive(true);",
                "firstLaunchTutorialFinishGate.gameObject.SetActive(true);");
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
