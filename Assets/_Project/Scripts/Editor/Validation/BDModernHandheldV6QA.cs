#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal sealed class BDModernHandheldV6QA : IPreprocessBuildWithReport
    {
        public int callbackOrder => 90;

        [MenuItem(
            "Boredom And Dungeons/Validate Modern Handheld V6 Polish")]
        private static void ValidateFromMenu()
        {
            string error = ValidateProject();
            if (string.IsNullOrEmpty(error))
            {
                Debug.Log(
                    "B&D HANDHELD V6 QA: STATIC PASS / UNITY PLAY MODE STILL REQUIRED"
                );
                return;
            }

            Debug.LogError("B&D HANDHELD V6 QA: BLOCKED\n" + error);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            string error = ValidateProject();
            if (!string.IsNullOrEmpty(error))
                throw new BuildFailedException(error);
        }

        private static string ValidateProject()
        {
            string root = Directory.GetParent(
                Application.dataPath
            ).FullName;

            string[] runtimeFiles =
            {
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Physical.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.MainPage.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.PausePage.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.PauseStyle.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Layout.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Helpers.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.ItemLayout.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.AreaAlignment.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Tactile.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldPressScaleFeedback.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldTactileCompatibility.cs"
            };

            string[] maintainedDocuments =
            {
                "ProjectGuide/Tasks/ACTIVE/MODERN_HANDHELD_V6_LAYOUT_AND_PAUSE_POLISH.md",
                "ProjectGuide/QA/MODERN_HANDHELD_V6_QA.md",
                "ProjectGuide/Status/MODERN_HANDHELD_V6_REVIEW.md"
            };

            string missing = FindMissingFile(root, runtimeFiles);
            if (!string.IsNullOrEmpty(missing))
                return missing;

            missing = FindMissingFile(root, maintainedDocuments);
            if (!string.IsNullOrEmpty(missing))
                return missing;

            string source = string.Empty;
            for (int index = 0; index < runtimeFiles.Length; index++)
            {
                source += File.ReadAllText(
                    Path.Combine(root, runtimeFiles[index])
                );
            }

            string[] required =
            {
                "DeviceYOffset = -0.34f",
                "V6 Product Shot Lowering Offset",
                "EnforcePlanarPosition",
                "PERSISTENT MEMORY",
                "SYSTEM CONFIGURATION",
                "BEHIND THE ADVENTURE",
                "LEAVE THE HANDHELD",
                "New Game Memory Card",
                "Next Card",
                "EnsureSettingsGear",
                "supportsMenuMark",
                "PAUSE MENU",
                "RUN PAUSED",
                "PlaceMenuItem",
                "AlignScreenArea",
                "ReadPressedState",
                "SetTactileProfile"
            };

            foreach (string token in required)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) < 0)
                {
                    return "Missing V6 contract token: " + token;
                }
            }

            return string.Empty;
        }

        private static string FindMissingFile(
            string root,
            string[] files)
        {
            for (int index = 0; index < files.Length; index++)
            {
                if (!File.Exists(Path.Combine(root, files[index])))
                    return "Missing required V6 file: " + files[index];
            }

            return string.Empty;
        }
    }
}
#endif
