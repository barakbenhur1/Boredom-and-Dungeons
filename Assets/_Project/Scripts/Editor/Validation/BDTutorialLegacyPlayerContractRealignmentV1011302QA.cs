#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialLegacyPlayerContractRealignmentV1011302QA
    {
        private const string PolishPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V1011Polish.cs";
        private const string ValidationRoot =
            "Assets/_Project/Scripts/Editor/Validation";
        private const string ThisFileName =
            "BDTutorialLegacyPlayerContractRealignmentV1011302QA.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, PolishPath,
                "TUTORIAL_V1011302_CURRENT_PLAYER_CONTRACT_MISSING",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "Positive X is authored facing right",
                "const int width = 18",
                "const int height = 26");
            Forbid(result, root, PolishPath,
                "TUTORIAL_V1011302_RETIRED_PLAYER_VISUAL_RETURNED",
                "UpdateFirstLaunchTutorialArticulatedPlayer",
                "BD UNMISTAKABLE PLAYER COLOR IDENTITY V10.11.17",
                "B&D Tutorial Player Blond Red Blue Sprite V10.11.17");
            ForbidObsoleteValidatorContracts(result, root);
        }

        private static void ForbidObsoleteValidatorContracts(
            BDOneClickQAResult result,
            string root)
        {
            string validationRoot = Path.Combine(root, ValidationRoot);
            if (!Directory.Exists(validationRoot))
            {
                Add(result,
                    "TUTORIAL_V1011302_VALIDATION_ROOT_MISSING",
                    ValidationRoot,
                    "Validation directory is missing.");
                return;
            }

            string[] obsoleteTokens =
            {
                "new Vector2(82f, 118f)",
                "Color hair = new Color(1f, 0.76f, 0.01f, 1f)",
                "Color shirt = new Color(0.94f, 0.025f, 0.045f, 1f)",
                "Color pants = new Color(0.025f, 0.20f, 0.90f, 1f)",
                "BD UNMISTAKABLE PLAYER COLOR IDENTITY V10.11.17",
                "B&D Tutorial Player Blond Red Blue Sprite V10.11.17"
            };

            string[] files = Directory.GetFiles(
                validationRoot,
                "*.cs",
                SearchOption.AllDirectories
            );
            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                string file = files[fileIndex];
                if (Path.GetFileName(file) == ThisFileName)
                    continue;
                string text = File.ReadAllText(file);
                for (int tokenIndex = 0;
                     tokenIndex < obsoleteTokens.Length;
                     tokenIndex++)
                {
                    if (!text.Contains(obsoleteTokens[tokenIndex]))
                        continue;
                    Add(result,
                        "TUTORIAL_V1011302_STALE_PLAYER_QA_CONTRACT",
                        file.Substring(root.Length).TrimStart(
                            Path.DirectorySeparatorChar,
                            Path.AltDirectorySeparatorChar),
                        "Retired player-visual QA token remains: " +
                            obsoleteTokens[tokenIndex]);
                }
            }
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
