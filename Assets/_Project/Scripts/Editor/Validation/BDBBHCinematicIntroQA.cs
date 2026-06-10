#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDBBHCinematicIntroQA
    {
        private const string RuntimeRelative =
            "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs";
        private const string FeatureRelative =
            "ProjectGuide/Features/UI/BBH_BOOT_INTRO_V1.md";
        private const string ChecklistRelative =
            "ProjectGuide/QA/QA_CHECKLIST.md";
        private const string DecisionsRelative =
            "ProjectGuide/Engineering/TECHNICAL_DECISIONS.md";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetParent(Application.dataPath)?.FullName;
            if (string.IsNullOrWhiteSpace(root))
            {
                Add(result, "BBH_CINEMATIC_PROJECT_ROOT_MISSING", string.Empty,
                    "Could not resolve the project root for BBH cinematic validation.");
                return;
            }

            ValidateFile(result, root, RuntimeRelative, new[]
            {
                "BD CINEMATIC CHARACTER MOTION + LARGER RESPONSIVE CIRCLE SIDE TASK V1",
                "CircleDiameterScale = 1.16f",
                "CircleMaximumScreenWidth = 0.78f",
                "CircleMaximumScreenHeight = 0.72f",
                "ResolveLetterMotion",
                "ResolveInteractionOffset",
                "ResolveInteractionScale",
                "ResolveInteractionRotation",
                "ResolvePulse",
                "EaseOutBackTuned",
                "Mathf.LerpUnclamped",
                "CircleActivationGatherFraction"
            });

            string runtimePath = Path.Combine(root, RuntimeRelative);
            if (File.Exists(runtimePath))
            {
                string runtime = File.ReadAllText(runtimePath);
                if (runtime.Contains("Time.realtimeSinceStartup * 2.6f"))
                {
                    Add(result, "BBH_PERPETUAL_IDLE_PULSE_RETURNED", RuntimeRelative,
                        "The old perpetual real-time letter pulse returned; the completed logo must use one deterministic authored breath.");
                }
            }

            ValidateFile(result, root, FeatureRelative, new[]
            {
                "Cinematic side-task refinement",
                "16%",
                "first B",
                "second B",
                "H landing",
                "does not change the active handheld task"
            });

            ValidateFile(result, root, ChecklistRelative, new[]
            {
                "BBH cinematic boot-intro side-task gate",
                "circle is clearly larger",
                "active handheld priority remains unchanged"
            });

            ValidateFile(result, root, DecisionsRelative, new[]
            {
                "TD-037",
                "existing BDBBHBootIntro owner",
                "16% larger"
            });
        }

        private static void ValidateFile(
            BDOneClickQAResult result,
            string root,
            string relative,
            string[] requiredTokens)
        {
            string path = Path.Combine(root, relative);
            if (!File.Exists(path))
            {
                Add(result, "BBH_CINEMATIC_FILE_MISSING", relative,
                    "A required BBH cinematic source or maintained document is missing.");
                return;
            }

            string text = File.ReadAllText(path);
            foreach (string token in requiredTokens)
            {
                if (text.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                Add(result, "BBH_CINEMATIC_CONTRACT_MISSING", relative,
                    "Missing required BBH cinematic contract token: " + token);
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
