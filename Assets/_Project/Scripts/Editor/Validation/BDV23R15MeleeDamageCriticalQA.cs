#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R15MeleeDamageCriticalQA
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
                "V23R15_MELEE_DAMAGE_CONTRACT_MISSING",
                "BD PLAYER SWORD DAMAGE SPECTRUM AND CRITICAL V23R15",
                "swordDamageVariance = 0.10f",
                "SwordCriticalChance = 0.06f",
                "SwordCriticalMultiplier = 1.50f",
                "ResolveSwordAttackDamage",
                "ResolveSwordAttackBaseDamage",
                "ApplySwordCriticalRoll",
                "BD SPIN SHARED SPECTRUM + PER-TARGET CRITICAL V23R15B",
                "criticalHitCount",
                "ApplyPlayerSwordDamage"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "V23R15_CRITICAL_ROUTING_MISSING",
                "BD PLAYER SWORD CRITICAL DAMAGE ROUTING V23R15",
                "ApplyPlayerSwordDamage",
                "critical: critical"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDDamageNumberFeedback.cs",
                "V23R15_CRITICAL_NUMBER_COLOR_MISSING",
                "CriticalDamageColor",
                "bool critical = false"
            );

            Require(result, root,
                "Assets/_Project/Design/Combat/MELEE_DAMAGE_SPECTRUM_AND_CRITICALS_V1.md",
                "V23R15_DESIGN_MISSING",
                "exactly a 6% critical chance",
                "exactly 1.5",
                "independent critical roll for every valid enemy hit",
                "Projectiles and the grappling hook remain fixed-damage systems"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerGrapplingHook.cs",
                "V23R15_HOOK_CRITICAL_REGRESSION",
                "ApplyPlayerSwordDamage"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerRangedProjectile.cs",
                "V23R15_RANGED_CRITICAL_REGRESSION",
                "ApplyPlayerSwordDamage"
            );

            ValidateSpinPerTargetCriticalOrdering(result, root);
        }

        private static void ValidateSpinPerTargetCriticalOrdering(
            BDOneClickQAResult result,
            string root)
        {
            string source = Read(
                root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs"
            );

            int sharedSpectrum = source.IndexOf(
                "float spinningBaseDamage = ResolveSwordAttackBaseDamage(",
                StringComparison.Ordinal
            );
            int targetLoop = source.IndexOf(
                "for (int index = 0; index < overlapCount; index++)",
                sharedSpectrum >= 0 ? sharedSpectrum : 0,
                StringComparison.Ordinal
            );
            int perTargetCritical = source.IndexOf(
                "float effectiveDamage = ApplySwordCriticalRoll(",
                targetLoop >= 0 ? targetLoop : 0,
                StringComparison.Ordinal
            );
            int applyDamage = source.IndexOf(
                "health.ApplyPlayerSwordDamage(",
                perTargetCritical >= 0 ? perTargetCritical : 0,
                StringComparison.Ordinal
            );

            if (sharedSpectrum < 0 ||
                targetLoop < sharedSpectrum ||
                perTargetCritical < targetLoop ||
                applyDamage < perTargetCritical)
            {
                Add(
                    result,
                    "V23R15B_SPIN_CRITICAL_ORDER_INVALID",
                    "Spin must roll one shared spectrum before the target loop and an independent critical inside the loop before applying damage."
                );
            }
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
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    Add(result, code, "Missing V23R15 contract: " + token);
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
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    Add(result, code, "Forbidden V23R15 token remains: " + token);
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
