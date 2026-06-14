#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDMetalSettingsInputV1011392QA
    {
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.cs";

        private const string SettingsPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.SettingsProfessionalLayoutV1011381.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetCurrentDirectory();
            string presenterPath = Path.Combine(root, PresenterPath);
            string settingsPath = Path.Combine(root, SettingsPath);

            Require(
                result,
                presenterPath,
                "METAL_COMBINED_DEPTH_V1011392_MISSING",
                "BD COMBINED NON-MEMORYLESS SCREEN TARGET V10.11.30.92",
                "screenDescriptor.depthStencilFormat =",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenCamera.targetTexture = screenRenderTexture;",
                "screenDepthRenderTexture = null;",
                "TryHandleSettingsPointerActivationV1011392(target)"
            );

            Require(
                result,
                settingsPath,
                "SETTINGS_POINTER_SCROLL_V1011392_MISSING",
                "BD PROFESSIONAL SETTINGS INPUT + SCROLL REPAIR V10.11.30.92",
                "fullyVisibleV1011392",
                "TryHandleSettingsPointerActivationV1011392",
                "ResolveSettingsPointerDirectionV1011392",
                "settingsLastObservedSelectionV1011392"
            );

            if (File.Exists(presenterPath))
            {
                string executable = StripComments(
                    File.ReadAllText(presenterPath)
                );
                if (executable.IndexOf(
                        "screenCamera.SetTargetBuffers(",
                        StringComparison.Ordinal) >= 0)
                {
                    Add(
                        result,
                        "METAL_MANUAL_DEPTH_BINDING_V1011392_REMAINS",
                        PresenterPath,
                        "Executable SetTargetBuffers binding remains."
                    );
                }
            }

            if (File.Exists(settingsPath))
            {
                string executable = StripComments(
                    File.ReadAllText(settingsPath)
                );
                if (executable.IndexOf(
                        "collider.enabled = alwaysAvailable || alpha >= 0.96f",
                        StringComparison.Ordinal) >= 0)
                {
                    Add(
                        result,
                        "SETTINGS_LAST_VISIBLE_ROW_V1011392_STILL_BLOCKED",
                        SettingsPath,
                        "The rejected alpha threshold still disables the last visible row."
                    );
                }
            }
        }

        private static string StripComments(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            StringBuilder output = new StringBuilder(source.Length);
            bool lineComment = false;
            bool blockComment = false;
            bool inString = false;
            bool escaped = false;

            for (int index = 0; index < source.Length; index++)
            {
                char current = source[index];
                char next = index + 1 < source.Length
                    ? source[index + 1]
                    : '\0';

                if (lineComment)
                {
                    if (current == '\n')
                    {
                        lineComment = false;
                        output.Append(current);
                    }
                    continue;
                }

                if (blockComment)
                {
                    if (current == '*' && next == '/')
                    {
                        blockComment = false;
                        index++;
                    }
                    continue;
                }

                if (inString)
                {
                    output.Append(current);
                    if (escaped)
                    {
                        escaped = false;
                    }
                    else if (current == '\\')
                    {
                        escaped = true;
                    }
                    else if (current == '"')
                    {
                        inString = false;
                    }
                    continue;
                }

                if (current == '/' && next == '/')
                {
                    lineComment = true;
                    index++;
                    continue;
                }

                if (current == '/' && next == '*')
                {
                    blockComment = true;
                    index++;
                    continue;
                }

                output.Append(current);
                if (current == '"')
                    inString = true;
            }

            return output.ToString();
        }

        private static void Require(
            BDOneClickQAResult result,
            string path,
            string code,
            params string[] tokens)
        {
            if (!File.Exists(path))
            {
                Add(result, code, path, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(path);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    Add(
                        result,
                        code,
                        path,
                        "Missing required contract: " + token
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
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    path,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
