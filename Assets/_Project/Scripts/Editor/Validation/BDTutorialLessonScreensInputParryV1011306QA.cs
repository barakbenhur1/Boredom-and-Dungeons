#if UNITY_EDITOR
using System.IO;
using System.Text;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialLessonScreensInputParryV1011306QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011306_LESSON_SCREEN_CONTRACT_MISSING",
                "ShouldBlockFirstLaunchTutorialActionForTravel",
                "targetActor.Health,",
                "FirstLaunchTutorialLessonCompleteTravelMessage",
                "LESSON COMPLETE — MOVE RIGHT TO THE NEXT SCREEN");
            Require(result, root, LessonPath,
                "TUTORIAL_V1011306_LESSON_SCREEN_STATE_MISSING",
                "QueueFirstLaunchTutorialStepForNextScreen",
                "SetFirstLaunchTutorialLessonInstructionVisible(false)",
                "FirstLaunchTutorialLessonCompleteTravelMessage",
                "ResolveFirstLaunchTutorialScreenCameraTarget",
                "TryResolveFirstLaunchTutorialUnifiedParry");
            Require(result, root, RepairPath,
                "TUTORIAL_V1011306_PARRY_COLLISION_MISSING",
                "The parry target must never be an invisible wall");

            StringBuilder all = new StringBuilder();
            string ui = Path.Combine(root,
                "Assets/_Project/Scripts/Runtime/UI");
            foreach (string file in Directory.GetFiles(
                         ui,
                         "BDModernHandheld3DPresenter*.cs"))
            {
                all.Append(File.ReadAllText(file));
            }
            string source = all.ToString();
            RequireText(result, source,
                "TUTORIAL_V1011306_MOUSE_MAPPING_MISSING",
                "Assets/_Project/Scripts/Runtime/UI",
                "Mouse.current.leftButton.wasPressedThisFrame",
                "Mouse.current.rightButton.wasPressedThisFrame",
                "Keyboard.current.qKey.wasPressedThisFrame",
                "Keyboard.current.eKey.wasPressedThisFrame");
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
            RequireText(result, File.ReadAllText(full), code, relative, tokens);
        }

        private static void RequireText(
            BDOneClickQAResult result,
            string source,
            string code,
            string path,
            params string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (!source.Contains(token))
                {
                    Add(
                        result,
                        code,
                        path,
                        "Missing required contract token: " + token
                    );
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
