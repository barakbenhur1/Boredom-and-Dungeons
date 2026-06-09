#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19JQaSemanticRealignmentQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V23R19J_MOUNTED_RELEASE_OWNER_MISSING",
                "fullStopHoldDuration = 0.24f",
                "RestoreMountedIntroControls(",
                "inputLocked = false"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R19J_COMBAT_COMMIT_OWNER_MISSING",
                "BD COMMITTED AIRBORNE ATTACK PRESENTATION V23R11",
                "PrepareCommittedAttackDamage"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R19J_AIRBORNE_PRESENTATION_OWNER_MISSING",
                "BD EXPLICIT COMMITTED AIRBORNE VISUAL OWNER V23R19K",
                "out airbornePresentation",
                "SpawnCommittedMeleeSlashArc",
                "BDMeleeSlashArcVisual.SpawnVertical"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCombatantProfile.cs",
                "V23R19J_ELITE_POLICY_MISSING",
                "ConfigureEliteGuardian",
                "Configure(BDCombatantRank.MiniBoss, false)",
                "CanReceiveForcedMovement"
            );

            Require(result, root,
                "ProjectGuide/Features/Combat/GRAPPLING_HOOK_HEAVY_HOLD_V1.md",
                "V23R19J_HOOK_DESIGN_MISSING",
                "V23R19B hit-committed pull reliability",
                "pull is committed",
                "Elite collectible guardians",
                "boy cannot use the grappling hook while mounted"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCharacterDeathAnimation.cs",
                "V23R19J_DEATH_OWNER_MISSING",
                "BD RENDERER-BRANCH PLAYER AND ENEMY DEATH V23R19G",
                "PlayPlayerDeath",
                "PlayEnemyDeath",
                "ResolveAndCaptureVisualBranches"
            );

            Require(result, root,
                "ProjectGuide/Features/Characters/GIRL_FATHER_BOSS_META_PROGRESSION_HE_V1.md",
                "V23R19J_CHARACTER_SPEC_CORRECTION_MISSING",
                "REQUIRED / FUTURE / NOT IMPLEMENTED",
                "זימון האמא בשלבים 1–3",
                "בדיוק 30 שניות",
                "הילד נשאר דמות ברירת המחדל",
                "Meta Progression"
            );

        }

        private static string Read(string root, string relative)
        {
            string absolute = Path.Combine(root, relative);
            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
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
                Add(result, code, "Missing V23R19J contract: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
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
