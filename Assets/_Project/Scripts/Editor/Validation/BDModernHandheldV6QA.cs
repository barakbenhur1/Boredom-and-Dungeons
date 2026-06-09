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

            string[] files =
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
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldTactileCompatibility.cs",
                "ProjectGuide/Tasks/ACTIVE/MODERN_HANDHELD_V6_LAYOUT_AND_PAUSE_POLISH.md",
                "ProjectGuide/QA/MODERN_HANDHELD_V6_QA.md"
            };

            foreach (string file in files)
            {
                if (!File.Exists(Path.Combine(root, file)))
                    return "Missing required V6 file: " + file;
            }

            string source = string.Empty;
            for (int index = 0; index < files.Length; index++)
            {
                if (files[index].EndsWith(
                        ".cs",
                        StringComparison.Ordinal))
                {
                    source += File.ReadAllText(
                        Path.Combine(root, files[index])
                    );
                }
            }

            string[] required =
            {
                "DeviceYOffset = -0.34f",
                "V6 Product Shot Lowering Offset",
                "PERSISTENT MEMORY",
                "SYSTEM CONFIGURATION",
                "BEHIND THE ADVENTURE",
                "LEAVE THE HANDHELD",
                "Next Card",
                "EnsureSettingsGear",
                "Pause Internal Menu Panel",
                "RUN PAUSED",
                "PlaceMenuItem",
                "AlignScreenArea",
                "BDModernHandheldPressScaleFeedback",
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

            string[] forbidden =
            {
                "System.Reflection",
                "GetField(",
                "GetProperty(",
                "HandleModernPrimaryAction",
                "HandleModernOpenSettings",
                "HandleModernOpenProgression",
                "HandleModernRequestMainMenu"
            };

            foreach (string token in forbidden)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    return "V6 polish may not own menu semantics: " + token;
                }
            }

            return string.Empty;
        }
    }
}
#endif
