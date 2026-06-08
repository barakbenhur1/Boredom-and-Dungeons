#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19BHookGuardianQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCombatantProfile.cs",
                "V23R19B_ELITE_POLICY_MISSING",
                "ConfigureEliteGuardian",
                "Configure(BDCombatantRank.MiniBoss, false)",
                "ReceivesForcedMovement",
                "CanReceiveForcedMovement"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDKnockbackReceiver.cs",
                "V23R19B_FORCED_MOVEMENT_GUARD_MISSING",
                "BD NON-SMALL COMBATANT FORCED-MOVEMENT IMMUNITY V23R19B",
                "!combatantProfile.ReceivesForcedMovement"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerGrapplingHook.cs",
                "V23R19B_HOOK_COMMITMENT_MISSING",
                "BD HOOK HIT-COMMITS SMALL-ENEMY PULL V23R19B",
                "BDEnemyHazardNavigation.IsSmallRegularEnemy",
                "BDCombatantProfile.CanReceiveForcedMovement",
                "Re-evaluate at the actual impact frame",
                "targetRoot.GetComponent<BDHitStaggerReceiver>()"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Collectibles/BDCollectibleGuardianSpawner.cs",
                "V23R19B_GUARDIAN_ACTIVATION_MISSING",
                "BD ATOMIC COLLECTIBLE-GUARDIAN ACTIVATION V23R19B",
                "guardian.SetActive(false)",
                "guardian.SetActive(true)",
                "profile.ConfigureEliteGuardian()",
                "guardian.AddComponent<BDEnemyTacticalCommand>()",
                "guardian.AddComponent<BDSwordEnemy>()",
                "guardian.AddComponent<BDChargerEnemy>()"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Collectibles/BDCollectibleGuardianSpawner.cs",
                "V23R19B_PARTIAL_GUARDIAN_TOGGLE_REMAINS",
                "SetGuardianActiveState(guardian, false)",
                "hiddenPosition = position + Vector3.down"
            );

            Require(result, root,
                "Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md",
                "V23R19B_GUARDIAN_DESIGN_MISSING",
                "V23R19B runtime activation and combat classification",
                "`Elite`, not small regular enemies",
                "fully damageable"
            );

            Require(result, root,
                "Assets/_Project/Design/Combat/GRAPPLING_HOOK_HEAVY_HOLD_V1.md",
                "V23R19B_HOOK_DESIGN_MISSING",
                "V23R19B hit-committed pull reliability",
                "pull is committed",
                "Elite collectible guardians"
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

                Add(result, code, "Missing V23R19B contract: " + token);
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

                Add(result, code, "Obsolete V23R19B behavior remains: " + token);
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
