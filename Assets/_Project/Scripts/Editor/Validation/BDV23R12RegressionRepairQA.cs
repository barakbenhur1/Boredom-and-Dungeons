#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R12RegressionRepairQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs",
                "V23R12_QA_INTEGRATION_MISSING",
                "BDV23R12RegressionRepairQA.Scan(result)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerGrapplingHook.cs",
                "V23R12_HOOK_ROOT_PULL_MISSING",
                "BD RELIABLE MOVEMENT-ROOT HOOK PULL V23R12",
                "ResolveTargetRoot",
                "ResolveTargetBodyCollider",
                "IsContactAttackSuppressed",
                "AcceptCurrentPositionAsBaseline"
            );

            foreach (string enemyPath in new[]
            {
                "Assets/_Project/Scripts/Runtime/BDChargerEnemy.cs",
                "Assets/_Project/Scripts/Runtime/BDSwordEnemy.cs",
                "Assets/_Project/Scripts/Runtime/BDPatrolGuardEnemy.cs",
                "Assets/_Project/Scripts/Runtime/BDJumperEnemy.cs",
                "Assets/_Project/Scripts/Runtime/BDExitBlockerEnemy.cs"
            })
            {
                Require(result, root, enemyPath,
                    "V23R12_HOOK_CONTACT_SUPPRESSION_MISSING",
                    "BDGrapplingHookPullState.IsContactAttackSuppressed"
                );
            }

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R12_MOUNTED_TARGET_ENVELOPE_MISSING",
                "BD RANGE-AWARE COMBAT TARGET ENVELOPE V23R8",
                "TargetHighlightOrigin",
                "IsMountedOnHorse()",
                "cachedMountedHorseCheck.transform.position",
                "Vector3.up * 1.22f"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDCombatTargetHighlighter.cs",
                "V23R12_MOUNTED_HIGHLIGHT_MISSING",
                "combat.TargetHighlightOrigin"
            );
            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDCombatTargetHighlighter.cs",
                "V23R12_DISABLED_COMBAT_HIGHLIGHT_BLOCK_REMAINS",
                "!combat.enabled"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R12_SLASH_CLEANUP_MISSING",
                "BD PARRY CANCELS ACTIVE PLAYER SLASH V23R12",
                "ClearAllActive"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDParrySystem.cs",
                "V23R12_PARRY_SLASH_CLEAR_MISSING",
                "BDMeleeSlashArcVisual.ClearAllActive()"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs",
                "V23R12_HORSE_CONTEXT_STRIP_MISSING",
                "BD HORSE BOTTOM CONTEXT STRIP V2",
                "bottomMargin",
                "No icon, label, health bar, or interaction card is drawn",
                "EaseOutCubic"
            );
            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs",
                "V23R12_WORLD_HORSE_PROMPT_REMAINS",
                "WorldToScreenPoint",
                "worldHeight",
                "screenOffset"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/EnemyPlacementSafety/BDEnemyPlacementSafety.cs",
                "V23R12_ROOT_GROUNDING_MISSING",
                "BD CONTROLLER-CENTER-AWARE GROUND ROOT V23R12",
                "ResolveRootPositionForGround",
                "BD SPAWN-ONLY HORIZONTAL RELOCATION V23R12",
                "ApplyGroundPoint"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDEnemyMotionStabilizer.cs",
                "V23R12_STABILIZER_BASELINE_MISSING",
                "AcceptCurrentPositionAsBaseline",
                "ResolveRootPositionForGround"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDEnemyBootstrap.cs",
                "V23R12_PLACEMENT_GUARD_BOOTSTRAP_MISSING",
                "BDEnemyPlacementGuard"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDGameBoyMenuShell.cs",
                "V23R12_INTEGRATED_MENU_SHELL_MISSING",
                "BD INTEGRATED GAME BOY MENU SHELL V23R12",
                "DrawIntegrated"
            );
            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDGameBoyMenuShell.cs",
                "V23R12_COMPETING_SHELL_ONGUI_REMAINS",
                "private void OnGUI("
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "V23R12_MENU_INTEGRATION_MISSING",
                "BDGameBoyMenuShell.DrawIntegrated",
                "BDMeleeSlashArcVisual.ClearAllActive()"
            );

            Require(result, root,
                "ProjectGuide/Features/Runtime/V23R12_RUNTIME_REGRESSION_REPAIR.md",
                "V23R12_DESIGN_CONTRACT_MISSING",
                "reliable pull and safe release",
                "Mounted gameplay uses a horse-height targeting origin",
                "Enemy grounding and spawn safety",
                "same `BDMainMenuFlow.OnGUI` pass"
            );
            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R12_STATUS_MISSING",
                "C01/C03/C04/C05/C11.RUNTIME.V23R12",
                "focused Play Mode regression repair",
                "Saved feature resume point"
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
                Add(result, code, "Missing V23R12 contract: " + token);
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
                Add(result, code, "Obsolete V23R12 token remains: " + token);
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
