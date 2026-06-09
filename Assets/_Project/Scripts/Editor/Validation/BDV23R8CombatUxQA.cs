#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R8CombatUxQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs",
                "V23R8_QA_INTEGRATION_MISSING",
                "BDV23R8CombatUxQA.Scan(result)"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V23R8_CAMERA_COMPOSITION_CONTRACT_MISSING",
                "BD EXPLICIT 40-BEHIND 60-AHEAD COMPOSITION V23R8",
                "BD LEGACY SERIALIZED COMPOSITION MIGRATION V23R8",
                "targetViewportHeight01 = 0.40f",
                "minPitch = Mathf.Min(minPitch, 35f)",
                "ResolveScreenCompositionPitch"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R8_HORSE_CONTROL_CONTRACT_MISSING",
                "BD COHERENT MOUNTED MOUSE STEERING V23R8",
                "BD HEALING ON FOOT ONLY V23R9",
                "IsMountedStationary",
                "BDHorseContextActionPrompts",
                "lastMountedAimDirection"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs",
                "V23R8_HORSE_PROMPT_CONTRACT_MISSING",
                "BD UNIFIED HORSE CONTEXT PROMPTS V23R8",
                "MOUNT",
                "DISMOUNT",
                "HOLD HEAL",
                "PET",
                "IsMountedStationary",
                "BD HORSE PROMPT STATE MATRIX V23R9"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseExhaustedFollowAndPetInteraction.cs",
                "V23R8_OLD_PET_CUE_CREATION_REMAINS",
                "BD UNIFIED HORSE CONTEXT PROMPTS V23R8",
                "BDHorseContextActionPrompts",
                "BD MOUNTED STATIONARY PET WITHOUT PROMPT V23R9"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "C03_23A_GRAPPLING_INPUT_CONTRACT_MISSING",
                "BD HEAVY-HOLD GRAPPLING HOOK C03.23A",
                "grapplingHookDamage = 2f",
                "grapplingHookCooldown = 2.75f",
                "CommitRegularHeavyAttack",
                "BDPlayerGrapplingHook.Launch",
                "BD RANGE-AWARE COMBAT TARGET ENVELOPE V23R8",
                "TryResolveTargetHighlightEnvelope",
                "BDCombatTargetHighlighter"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerGrapplingHook.cs",
                "C03_23A_GRAPPLING_RUNTIME_MISSING",
                "CanPullSmallEnemy",
                "BD HOOK HIT-COMMITS SMALL-ENEMY PULL V23R19B",
                "BDEnemyHazardNavigation.IsSmallRegularEnemy",
                "BDCombatantProfile.CanReceiveForcedMovement",
                "targetHealth.ApplyDamage(damage)",
                "pullStopDistance"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/BDCombatTargetHighlighter.cs",
                "V23R8_TARGET_HIGHLIGHT_CONTRACT_MISSING",
                "BD CONSTANT-SIZE SILHOUETTE TARGET OUTLINE V23R13",
                "TryResolveTargetHighlightEnvelope",
                "SphereCastNonAlloc",
                "BDTargetOutlineVisual",
                "outlineWidthPixels"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerAirStateTracker.cs",
                "V23R8_AIRBORNE_STATE_CONTRACT_MISSING",
                "BD AIRBORNE ATTACK VISUAL STATE V23R8",
                "IsAirborneFromJump",
                "IsDescendingFromJump"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R8_VERTICAL_AIRBORNE_ATTACK_MISSING",
                "BD VERTICAL AIRBORNE MELEE VISUAL V23R8",
                "IsAirborneFromJump",
                "BD_Heavy_Vertical_Airborne_Slash",
                "BD_Light_Vertical_Airborne_Slash",
                "PrepareCommittedAttack"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R8_OBSOLETE_LANDING_CUBE_VISUAL_REMAINS",
                "BD_Landing_Downward_Strike"
            );

            Require(
                result,
                root,
                "ProjectGuide/Features/Combat/GRAPPLING_HOOK_HEAVY_HOLD_V1.md",
                "V23R8_GRAPPLING_DESIGN_MISSING",
                "2 damage",
                "small regular enemies",
                "large enemies",
                "cooldown"
            );

            Require(
                result,
                root,
                "ProjectGuide/Features/Combat/AIRBORNE_VERTICAL_ATTACK_PRESENTATION_V1.md",
                "V23R8_AIRBORNE_DESIGN_MISSING",
                "light",
                "heavy",
                "vertical",
                "airborne"
            );

            Require(
                result,
                root,
                "ProjectGuide/Features/Combat/RANGE_AWARE_TARGET_HIGHLIGHT_V1.md",
                "V23R8_HIGHLIGHT_DESIGN_MISSING",
                "red",
                "frame",
                "range",
                "one target"
            );

            Require(
                result,
                root,
                "ProjectGuide/Features/Horse/HORSE_CONTEXT_ACTION_PROMPTS_V1.md",
                "V23R8_HORSE_PROMPT_DESIGN_MISSING",
                "Mount",
                "Dismount",
                "Heal",
                "Pet",
                "stationary"
            );

            Require(
                result,
                root,
                "ProjectGuide/INDEX.md",
                "V23R8_DOCUMENTATION_MAP_MISSING",
                "Current V23R8 feature contracts",
                "ProjectGuide/Product/ART_DIRECTION.md",
                "Art-direction conventions are active"
            );

            Require(
                result,
                root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R8_STATUS_CONTRACT_MISSING",
                "C01/C03/C04/C11.RUNTIME.V23R8",
                "Art-direction conventions document",
                "Saved feature resume point"
            );
        }

        private static string Read(
            string root,
            string relative)
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
                Add(
                    result,
                    code,
                    "Missing required file: " + relative
                );
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    code,
                    "Missing V23R8 contract: " + token
                );
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
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) < 0)
                {
                    continue;
                }

                Add(
                    result,
                    code,
                    "Obsolete V23R8 token remains: " + token
                );
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
