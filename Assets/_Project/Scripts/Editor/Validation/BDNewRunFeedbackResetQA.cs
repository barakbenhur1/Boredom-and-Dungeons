#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDNewRunFeedbackResetQA
    {
        private const string RuntimeRelative =
            "Assets/_Project/Scripts/Runtime/" +
            "BDNewRunFeedbackReset.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string projectRoot =
                ResolveProjectRoot();
            string absolute =
                Path.Combine(
                    projectRoot,
                    RuntimeRelative
                );

            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "NEW_RUN_FEEDBACK_RESET_FILE_MISSING",
                    RuntimeRelative,
                    string.Empty,
                    "The new-run feedback reset runtime is missing."
                );
                return;
            }

            string source =
                File.ReadAllText(absolute);

            string[] required =
            {
                "RuntimeInitializeLoadType.SubsystemRegistration",
                "RuntimeInitializeLoadType.BeforeSceneLoad",
                "SceneManager.sceneLoaded",
                "SuppressionDuration = 0.45f",
                "IsFeedbackSuppressed",
                "RunStartFeedbackResetRequested",
                "Time.timeScale = 1f",
                "AudioListener.pause = false",
                "BDParrySystem",
                "BDGameFeelEvents",
                "BDGameFeelAudio",
                "StopEmittingAndClear",
                "StopTransientAudio",
                "ClearFeedbackCanvasGroups",
                "ResetFeedbackAnimators",
                "ResetCameraShakeState",
                "FindObjectsInactive.Include"
            };

            ValidateTokens(
                result,
                source,
                required
            );

            if (source.IndexOf(
                    "using UnityEditor",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "NEW_RUN_FEEDBACK_RESET_EDITOR_DEPENDENCY",
                    RuntimeRelative,
                    string.Empty,
                    "Runtime feedback reset must not depend on UnityEditor."
                );
            }
        }

        private static void ValidateTokens(
            BDOneClickQAResult result,
            string source,
            IEnumerable<string> required)
        {
            foreach (string token in required)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "NEW_RUN_FEEDBACK_RESET_CONTRACT_MISSING",
                    RuntimeRelative,
                    string.Empty,
                    "Required new-run reset contract is missing: " +
                    token +
                    "."
                );
            }
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets =
                new DirectoryInfo(
                    Application.dataPath
                );

            if (assets.Parent == null)
                return Application.dataPath;

            return assets.Parent.FullName;
        }

        private static void Add(
            BDOneClickQAResult result,
            BDOneClickQASeverity severity,
            string code,
            string assetPath,
            string objectPath,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    severity,
                    code,
                    assetPath,
                    objectPath,
                    message
                )
            );
        }
    }
}
#endif
