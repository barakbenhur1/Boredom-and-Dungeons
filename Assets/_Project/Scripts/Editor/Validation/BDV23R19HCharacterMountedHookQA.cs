#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19HCharacterMountedHookQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R19H_BOY_MOUNTED_HOOK_RULE_MISSING",
                "BD BOY MOUNTED HOOK DISABLED GIRL FUTURE V23R19H",
                "ClearPendingHeavyPress();",
                "boy mounted hook disabled"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R19H_BOY_MOUNTED_HOOK_STILL_ENABLED",
                "TickMountedGrapplingHookInput",
                "mounted grappling hook",
                "mounted heavy hold pending hook",
                "BD MOUNTED GRAPPLING HOOK ENVELOPE V23R19G"
            );

            Require(result, root,
                "Assets/_Project/Design/Combat/GRAPPLING_HOOK_HEAVY_HOLD_V1.md",
                "V23R19H_HOOK_DESIGN_RULE_MISSING",
                "Boy mounted rule",
                "The boy cannot use the grappling hook while mounted.",
                "Girl future rule",
                "the girl may use the grappling hook while mounted"
            );

            Require(result, root,
                "Assets/_Project/Design/Characters/GIRL_FATHER_BOSS_META_PROGRESSION_HE_V1.md",
                "V23R19H_GIRL_FUTURE_SPEC_MISSING",
                "הילד אינו יכול להשתמש בוו בזמן רכיבה",
                "הילדה תוכל להשתמש בוו"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md",
                "V23R19H_BUG_LEDGER_MISSING",
                "BUG-V23R19H-001",
                "Boy mounted hook incorrectly enabled",
                "IMPLEMENTED / UNITY VERIFICATION REQUIRED"
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19H_STATUS_MISSING",
                "C01/C03/C04.RUNTIME.V23R19H",
                "V23R19H acceptance gate",
                "boy cannot use the hook while mounted"
            );
        }

        private static string Read(string root, string relative)
        {
            string absolute = Path.Combine(root, relative);
            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
                : string.Empty;
        }

        private static void Require(BDOneClickQAResult result, string root,
            string relative, string code, params string[] tokens)
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
                Add(result, code, "Missing V23R19H contract: " + token);
            }
        }

        private static void Forbid(BDOneClickQAResult result, string root,
            string relative, string code, params string[] tokens)
        {
            string source = Read(root, relative);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;
                Add(result, code, "Obsolete mounted-hook token remains: " + token);
            }
        }

        private static void Add(BDOneClickQAResult result, string code,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                string.Empty,
                string.Empty,
                message
            ));
        }
    }
}
#endif
