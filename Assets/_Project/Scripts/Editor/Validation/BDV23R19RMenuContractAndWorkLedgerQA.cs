#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19RMenuContractAndWorkLedgerQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDGameBoyMenuShell.cs",
                "V23R19R_ACTIVE_MENU_IDENTITY_MISSING",
                "BD PROFESSIONAL MEMORY-HANDHELD SHELL V23R19Q",
                "B&D // POCKET MEMORY"
            );

            ForbidDirectory(result, root,
                "Assets/_Project/Scripts/Editor/Validation",
                "V23R19R_STALE_MENU_QA_REMAINS",
                "\"B&D POCKET ADVENTURE\""
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/Tasks/MASTER_ACTIVE_WORK_SEQUENCE_V1.md",
                "V23R19R_MASTER_LEDGER_MISSING",
                "Priority 0 — Close current automated blocker",
                "Priority 1 — Enemy attack animations",
                "Priority 2 — Professional opening cinematic",
                "Retained implemented but not yet user-verified work",
                "Open-bug and verification discipline",
                "Exact current resume point"
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19R_PROJECT_STATUS_MISSING",
                "Classification: CURRENT /",
                "MASTER_ACTIVE_WORK_SEQUENCE_V1.md",
                "Enemy Attack Animations",
                "Professional Opening Cinematic"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md",
                "V23R19R_BUG_LEDGER_MISSING",
                "This document must be updated every time"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/TASK_CONTINUITY_AND_HANDOFF_CONTRACT.md",
                "V23R19R_CONTINUITY_CONTRACT_MISSING",
                "implemented but not yet user-confirmed",
                "installer/static validation",
                "compilation",
                "TEST EVERYTHING",
                "focused Play Mode",
                "target-device/performance verification",
                "user acceptance"
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

                Add(result, code, "Missing V23R19R contract: " + token);
            }
        }

        private static void ForbidDirectory(
            BDOneClickQAResult result,
            string root,
            string relativeDirectory,
            string code,
            params string[] tokens)
        {
            string directory = Path.Combine(root, relativeDirectory);
            if (!Directory.Exists(directory))
            {
                Add(result, code, "Missing directory: " + relativeDirectory);
                return;
            }

            string[] files = Directory.GetFiles(
                directory,
                "*.cs",
                SearchOption.AllDirectories
            );

            foreach (string file in files)
            {
                string source = File.ReadAllText(file);

                foreach (string token in tokens)
                {
                    if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                        continue;

                    Add(
                        result,
                        code,
                        "Obsolete token remains in " +
                        Path.GetRelativePath(root, file) +
                        ": " +
                        token
                    );
                }
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
