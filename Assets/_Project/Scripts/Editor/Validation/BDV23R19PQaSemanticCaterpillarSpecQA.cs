#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19PQaSemanticCaterpillarSpecQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19TraversalQuicksandAirborneQA.cs",
                "V23R19P_TRAVERSAL_QA_STALE",
                "wallJumpHorizontalDuration = 0.62f",
                "wallJumpSteeringDegreesPerSecond",
                "wallJumpMinimumRetainedSpeed"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19TraversalQuicksandAirborneQA.cs",
                "V23R19P_TRAVERSAL_QA_OLD_DURATION",
                "\"wallJumpHorizontalDuration = 0.48f\""
            );

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19GFocusedRegressionQA.cs",
                "V23R19P_BUG_LEDGER_QA_STALE",
                "BUG-V23R19G-005",
                "BUG-V23R19O-001"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19GFocusedRegressionQA.cs",
                "V23R19P_RESOLVED_BUG_REQUIREMENT_REMAINS",
                "\"BUG-V23R19G-001\""
            );

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19OCriticalIntroOutlineWallJumpQA.cs",
                "V23R19P_INTRO_QA_CALL_STALE",
                "BDRunPresentationCoordinator.EnsureMountedIntroRiderVisible"
            );

            Require(result, root,
                "Assets/_Project/Design/Economy/CATERPILLAR_GAMBLING_NPC_V1.md",
                "V23R19P_CATERPILLAR_SPEC_MISSING",
                "REQUIRED / FUTURE / NOT IMPLEMENTED",
                "does **not** appear in every room",
                "selected for this NPC",
                "appearance animation",
                "disappearance animation",
                "gambling-session safety state",
                "The refill threshold is **not** an absolute wallet maximum",
                "one game"
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19P_STATUS_MISSING",
                "C01.QA.V23R19P",
                "Caterpillar gambling NPC"
            );
        }

        private static string Read(
            string root,
            string relative)
        {
            string path = Path.Combine(root, relative);
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                Add(result, code, "Missing V23R19P contract: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);

            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;

                Add(result, code, "Obsolete V23R19P token remains: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    string.Empty,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
