#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialOpeningScreenSequenceV1011322QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string ProductionCoursePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011322_OPENING_SCREEN_SEQUENCE_MISSING",
                "current == FirstLaunchTutorialStep.Move &&",
                "next == FirstLaunchTutorialStep.Jump",
                "current == FirstLaunchTutorialStep.MountHorse &&",
                "next == FirstLaunchTutorialStep.RideHorse",
                "BD HORSE-FREE OPENING VISIBILITY V10.11.30.27",
                "case FirstLaunchTutorialStep.Jump:",
                "case FirstLaunchTutorialStep.AttackEnemy:",
                "roomIndex = 1;");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011322_REAL_FORWARD_WALK_MISSING",
                "firstLaunchTutorialMoveLessonStartX",
                "firstLaunchTutorialPlayerWorldPosition.x -\n                        firstLaunchTutorialMoveLessonStartX",
                "firstLaunchTutorialTravelDistance >= 64f",
                "FirstLaunchTutorialStep.RideHorse &&",
                "firstLaunchTutorialLessonScreenCenterX + 118f",
                "FirstLaunchTutorialStep.EnemyArrival");
            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011322_ATTEMPTED_DISTANCE_REGRESSION",
                "firstLaunchTutorialTravelDistance += delta.magnitude");

            Require(result, root, ProductionCoursePath,
                "TUTORIAL_V1011322_JUMP_ATTACK_ROOM_HANDOFF_MISSING",
                "BD HORSE-FREE OPENING HANDOFF V10.11.30.27",
                "TutorialJumpObstacleX + 24f",
                "SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.AttackEnemy)",
                "JUMP CLEARED");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011322_MOUNT_RIDE_HANDOFF_MISSING",
                "CompleteFirstLaunchTutorialMountAnimation",
                "FirstLaunchTutorialStep.MountHorse",
                "FirstLaunchTutorialStep.RideHorse");
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
                {
                    Add(result, code, relative,
                        "Missing required contract token: " + token);
                }
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
                {
                    Add(result, code, relative,
                        "Forbidden regression token remains: " + token);
                }
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
