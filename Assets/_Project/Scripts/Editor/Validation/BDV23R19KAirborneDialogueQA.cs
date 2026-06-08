#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19KAirborneDialogueQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R19K_AIRBORNE_IDENTITY_OWNER_MISSING",
                "BD EXPLICIT AIRBORNE ATTACK PRESENTATION BRANCH V23R19",
                "out bool airbornePresentation",
                "ShouldSpawnAirborneSlashVisual"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R19K_COMMITTED_VISUAL_BRANCH_MISSING",
                "BD EXPLICIT COMMITTED AIRBORNE VISUAL OWNER V23R19K",
                "out airbornePresentation",
                "SpawnCommittedMeleeSlashArc",
                "BDMeleeSlashArcVisual.SpawnVertical",
                "SpawnMeleeSlashArc(aim, heavySwing)"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R19K_TIMING_SUPPRESSION_RACE_REMAINS",
                "combat.SuppressNextStandardMeleeVisual"
            );

            Require(result, root,
                "Assets/_Project/Design/Characters/GIRL_FATHER_BOSS_META_PROGRESSION_HE_V1.md",
                "V23R19K_CHARACTER_HOOK_RULE_MISSING",
                "הילד אינו יכול להשתמש בוו בזמן רכיבה",
                "הילדה תוכל להשתמש בוו"
            );

            Require(result, root,
                "Assets/_Project/Design/Cinematics/OPENING_DIALOGUE_WORDLESS_CHARACTER_VOICE_HE_V1.md",
                "V23R19K_OPENING_DIALOGUE_SPEC_MISSING",
                "REQUIRED / LATER / NOT IMPLEMENTED",
                "I’m bored.",
                "Emotion: Bored",
                "צליל דמוי דיבור ללא מילים"
            );

        }

        private static string Read(string root, string relative)
        {
            string absolute = Path.Combine(root, relative);
            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
                : string.Empty;
        }

        private static void Require(BDOneClickQAResult result,
            string root, string relative, string code,
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
                Add(result, code, "Missing V23R19K contract: " + token);
            }
        }

        private static void Forbid(BDOneClickQAResult result,
            string root, string relative, string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;
                Add(result, code, "Obsolete V23R19K token remains: " + token);
            }
        }

        private static void Add(BDOneClickQAResult result,
            string code, string message)
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
