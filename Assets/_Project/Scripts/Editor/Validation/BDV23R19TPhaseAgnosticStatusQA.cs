#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19TPhaseAgnosticStatusQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19RMenuContractAndWorkLedgerQA.cs",
                "V23R19T_R_SCANNER_STILL_PHASE_BOUND",
                "\"Classification: CURRENT /\"",
                "\"MASTER_ACTIVE_WORK_SEQUENCE_V1.md\"",
                "\"Enemy Attack Animations\"",
                "\"Professional Opening Cinematic\"",
                "\"This document must be updated every time\""
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19RMenuContractAndWorkLedgerQA.cs",
                "V23R19T_R_SCANNER_HISTORICAL_TOKEN_REMAINS",
                "\"C01.QA.V23R19R\"",
                "\"BUG-V23R19R-001\""
            );

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19SContinuitySemanticQA.cs",
                "V23R19T_S_SCANNER_STILL_PHASE_BOUND",
                "\"Classification: CURRENT /\"",
                "\"MASTER_ACTIVE_WORK_SEQUENCE_V1.md\"",
                "\"Enemy Attack Animations\"",
                "\"Professional Opening Cinematic\"",
                "\"This document must be updated every time\""
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19SContinuitySemanticQA.cs",
                "V23R19T_S_SCANNER_HISTORICAL_TOKEN_REMAINS",
                "\"C01.QA.V23R19S\"",
                "\"BUG-V23R19S-001\""
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19T_CURRENT_STATUS_MISSING",
                "Classification: CURRENT /",
                "MASTER_ACTIVE_WORK_SEQUENCE_V1.md",
                "Enemy Attack Animations",
                "Professional Opening Cinematic"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/Tasks/MASTER_ACTIVE_WORK_SEQUENCE_V1.md",
                "V23R19T_MASTER_SEQUENCE_MISSING",
                "Priority 1 — Enemy attack animations",
                "Priority 2 — Professional opening cinematic",
                "Exact current resume point"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md",
                "V23R19T_BUG_TRACKING_DISCIPLINE_MISSING",
                "This document must be updated every time"
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

                Add(result, code, "Missing V23R19T contract: " + token);
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

                Add(result, code, "Obsolete V23R19T token remains: " + token);
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
