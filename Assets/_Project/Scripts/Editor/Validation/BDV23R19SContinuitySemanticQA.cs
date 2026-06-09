#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19SContinuitySemanticQA
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
                "V23R19S_R_SCANNER_NOT_SEMANTIC",
                "\"installer/static validation\"",
                "\"compilation\"",
                "\"TEST EVERYTHING\"",
                "\"focused Play Mode\"",
                "\"target-device/performance verification\"",
                "\"user acceptance\""
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19RMenuContractAndWorkLedgerQA.cs",
                "V23R19S_BRITTLE_CONTINUITY_SENTENCE_REMAINS",
                "\"automated, Play Mode, device, performance, visual, audio, and user acceptance\""
            );

            Require(result, root,
                "ProjectGuide/Rules/TASK_CONTINUITY.md",
                "V23R19S_CONTINUITY_LADDER_MISSING",
                "installer/static validation",
                "compilation",
                "TEST EVERYTHING",
                "focused Play Mode",
                "target-device/performance verification",
                "user acceptance",
                "An earlier stage never implies a later stage"
            );

            Require(result, root,
                "ProjectGuide/Status/WORK_QUEUE.md",
                "V23R19S_MASTER_SEQUENCE_MISSING",
                "Priority 0 — Close current automated blocker",
                "Priority 1 — Enemy attack animations",
                "Exact current resume point"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R19S_STATUS_MISSING",
                "Classification: CURRENT /",
                "MASTER_ACTIVE_WORK_SEQUENCE_V1.md",
                "Enemy Attack Animations",
                "Professional Opening Cinematic"
            );

            Require(result, root,
                "ProjectGuide/Status/BUGS.md",
                "V23R19S_BUG_LEDGER_MISSING",
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

                Add(result, code, "Missing V23R19S contract: " + token);
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

                Add(result, code, "Obsolete V23R19S token remains: " + token);
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
