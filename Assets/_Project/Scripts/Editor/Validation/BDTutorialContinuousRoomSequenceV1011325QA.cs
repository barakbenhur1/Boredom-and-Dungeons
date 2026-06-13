#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialContinuousRoomSequenceV1011325QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string ProductionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011325_WORLD_SEQUENCE_CAPACITY_MISSING",
                "TutorialWorldMaxX = 17200f",
                "TutorialOpeningScreenCenterX +",
                "index * TutorialLessonScreenSpacing",
                "TutorialLessonScreenExitOffset;",
                "if (firstLaunchTutorialLessonCompleteAwaitingTravel)",
                "return firstLaunchTutorialLessonScreenExitX;");

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011325_CONTINUOUS_HANDOFF_MISSING",
                "TutorialLessonScreenSpacing = 780f",
                "TutorialLessonScreenHandoffSeconds = 0.72f",
                "BeginFirstLaunchTutorialContinuousRoomHandoff",
                "UpdateFirstLaunchTutorialContinuousRoomHandoff",
                "Mathf.Lerp(\n                firstLaunchTutorialHandoffStartCameraX",
                "Vector2.Lerp(\n                firstLaunchTutorialHandoffStartPlayerPosition",
                "DisableFirstLaunchTutorialRoomTransitionOverlay",
                "firstLaunchTutorialLessonScreenExitX = Mathf.Clamp",
                "SetFirstLaunchTutorialLessonInstructionVisible(false)",
                "SetFirstLaunchTutorialLessonInstructionVisible(true)",
                "return firstLaunchTutorialLessonScreenCenterX;",
                "firstLaunchTutorialStepStartedAt = Time.unscaledTime",
                "firstLaunchTutorialReloadCompletesAt =",
                "actor.NextActionAt = Time.unscaledTime + 0.80f",
                "ApplyFirstLaunchTutorialHorseVisibilityForScreen(step)",
                "PrepareFirstLaunchTutorialRoomVisualsForHandoff(");

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011325_ORDERED_ROOM_MAP_MISSING",
                "roomIndex = 1;",
                "roomIndex = 5;",
                "roomIndex = 9;",
                "roomIndex = 14;",
                "roomIndex = 19;",
                "roomIndex = 20;",
                "roomIndex = 21;",
                "PositionFirstLaunchTutorialWallJumpGeometry(center)",
                "ResolveFirstLaunchTutorialWallJumpWallX()",
                "firstLaunchTutorialGapWorldPosition =",
                "firstLaunchTutorialGap.gameObject.SetActive(true)");

            Require(result, root, ProductionPath,
                "TUTORIAL_V1011325_DYNAMIC_ROOM_OBJECTIVES_MISSING",
                "firstLaunchTutorialLessonScreenCenterX + 76f",
                "firstLaunchTutorialLessonScreenCenterX + 150f",
                "firstLaunchTutorialLessonScreenCenterX + 260f",
                "ResolveFirstLaunchTutorialWallJumpPlatformMinX()",
                "ResolveFirstLaunchTutorialWallJumpUpperGroundMaxX()");

            Forbid(result, root, LessonScreensPath,
                "TUTORIAL_V1011325_FADE_ROOM_TRANSITION_REMAINS",
                "UpdateFirstLaunchTutorialLessonScreenTransition",
                "TutorialLessonScreenFadeInEnd",
                "TutorialLessonScreenFadeOutStart",
                "firstLaunchTutorialLessonScreenTransitionActive",
                "firstLaunchTutorialSecondToThirdContinuousHandoffActive",
                "firstLaunchTutorialWhiteOverlay.gameObject.SetActive(true)");
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
