#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialPlayerVisibilityRuntimeV1011303QA
    {
        private const string PolishPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V1011Polish.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, PolishPath,
                "TUTORIAL_V1011303_PLAYER_VISIBILITY_OWNER_MISSING",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "firstLaunchTutorialPlayerPixelVisual.gameObject.SetActive(true)",
                "firstLaunchTutorialPlayerPixelImage.enabled = true",
                "firstLaunchTutorialPlayer.enabled = false",
                "new Vector2(64f, 92f)",
                "entry.StepA = firstLaunchTutorialSimplePlayerWalkASprite",
                "entry.ActionB = firstLaunchTutorialSimplePlayerActionBSprite",
                "IsFirstLaunchTutorialPlayerVisualActing()",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "Positive X is authored facing right");
            Forbid(result, root, PolishPath,
                "TUTORIAL_V1011303_PLAYER_VISIBILITY_REGRESSION",
                "DisableFirstLaunchTutorialCompositePlayerChildren()",
                "firstLaunchTutorialPlayer.sprite =\n                firstLaunchTutorialPolishedPlayerSprite");
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
                if (text.Contains(tokens[index]))
                    continue;
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
                if (!text.Contains(tokens[index]))
                    continue;
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
