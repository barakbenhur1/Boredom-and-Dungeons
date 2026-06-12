#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialPlayerCanonicalAssetNameV1011304QA
    {
        private const string PolishPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V1011Polish.cs";
        private const string PreviousQaPath =
            "Assets/_Project/Scripts/Editor/Validation/" +
            "BDTutorialPlayerVisibilityRuntimeV1011303QA.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, PolishPath,
                "TUTORIAL_V1011304_CANONICAL_PLAYER_ASSET_MISSING",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "firstLaunchTutorialPlayerPixelVisual.gameObject.SetActive(true)",
                "firstLaunchTutorialPlayerPixelImage.enabled = true",
                "entry.StepA = firstLaunchTutorialSimplePlayerWalkASprite",
                "entry.StepB = firstLaunchTutorialSimplePlayerWalkBSprite",
                "entry.ActionA = firstLaunchTutorialSimplePlayerActionASprite",
                "entry.ActionB = firstLaunchTutorialSimplePlayerActionBSprite");
            Forbid(result, root, PolishPath,
                "TUTORIAL_V1011304_OBSOLETE_IDLE_ASSET_NAME_REMAINS",
                "B&D Tutorial Player Simple Right Facing Idle");
            Require(result, root, PreviousQaPath,
                "TUTORIAL_V1011304_PREVIOUS_QA_NOT_REALIGNED",
                "B&D Tutorial Player Simple Right Facing Sprite");
            Forbid(result, root, PreviousQaPath,
                "TUTORIAL_V1011304_PREVIOUS_QA_STILL_EXPECTS_IDLE",
                "B&D Tutorial Player Simple Right Facing Idle");
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
                    "Forbidden obsolete contract token remains: " + tokens[index]);
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
