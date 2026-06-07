#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDHorseExhaustedFollowPetQA
    {
        private const string PrototypeScenePath =
            "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();

            const string interactionRelative =
                "Assets/_Project/Scripts/Runtime/Horse/" +
                "BDHorseExhaustedFollowAndPetInteraction.cs";
            const string indicatorRelative =
                "Assets/_Project/Scripts/Runtime/Horse/" +
                "BDHorsePetAvailabilityIndicator.cs";
            const string healIndicatorRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseHealAvailabilityIndicator.cs";
            const string playerControllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDPlayerController.cs";
            const string recoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";
            const string controllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseController.cs";
            const string hazardRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHorseHazardSafety.cs";
            const string repairRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseRuntimeRepair.cs";
            const string installerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";
            const string designRelative =
                "Assets/_Project/Design/Horse/" +
                "HORSE_EXHAUSTED_FOLLOW_AND_PET_INTERACTION.md";

            ValidateRequiredFile(result, root, interactionRelative);
            ValidateRequiredFile(result, root, indicatorRelative);
            ValidateRequiredFile(result, root, healIndicatorRelative);
            ValidateRequiredFile(result, root, playerControllerRelative);
            ValidateRequiredFile(result, root, recoveryRelative);
            ValidateRequiredFile(result, root, controllerRelative);
            ValidateRequiredFile(result, root, hazardRelative);
            ValidateRequiredFile(result, root, repairRelative);
            ValidateRequiredFile(result, root, installerRelative);
            ValidateRequiredFile(result, root, designRelative);

            ValidateRequiredTokens(
                result,
                root,
                interactionRelative,
                new[]
                {
                    "exhaustedFollowBeginDistance = 14f",
                    "exhaustedFollowStopDistance = 8f",
                    "exhaustedFollowStartDelay = 1.25f",
                    "exhaustedFollowSpeedFraction = 0.20f",
                    "petInteractionRange = 2.25f",
                    "petLongPressThreshold = 0.65f",
                    "IsExhaustedFollowing",
                    "PetHoldProgress01",
                    "BeginExhaustedFollow",
                    "TryUseSafeFallbackNearPlayer",
                    "StartPetInteraction",
                    "horseNuzzlesPlayer",
                    "AcquireHorseExternalControl",
                    "ownsHorseExternalControl",
                    "playerControlStateCaptured",
                    "DesktopPetKey",
                    "EnsurePetAvailabilityIndicator",
                    "BDHorsePetAvailabilityIndicator",
                    "desktopPetKey = KeyCode.Tab",
                    "desktopPetKey == KeyCode.P",
                    "BuildPetButtonLabel",
                    "Pet [{keyLabel}]"
                },
                "HORSE_EXHAUSTED_PET_CONTRACT_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                indicatorRelative,
                new[]
                {
                    "BD_Horse_Pet_Available_Indicator",
                    "BD_Horse_Pet_Heart",
                    "BuildIdleLabel",
                    "DesktopPetKey",
                    "key + \"  PET\"",
                    "PetHoldProgress01",
                    "IsPetAvailable",
                    "IsPetInteractionActive",
                    "PETTING",
                    "HOLD  ",
                    "heightOffset = 4.65f",
                    "horizontalOffset = 1.15f",
                    "depthOffsetTowardCamera = 0.18f",
                    "sortingOrder = 90",
                    "camera.transform.right"
                },
                "HORSE_PET_VISUAL_CUE_CONTRACT_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                healIndicatorRelative,
                new[]
                {
                    "heightOffset = 3.95f",
                    "horizontalOffset = -1.15f",
                    "depthOffsetTowardCamera = 0.12f",
                    "sortingOrder = 80",
                    "camera.transform.right"
                },
                "HORSE_HEAL_VISUAL_STACK_CONTRACT_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                playerControllerRelative,
                new[]
                {
                    "BD POST-RECOVERY WALK REENTRY SUPPRESSION V23R3",
                    "PostRecoveryGapEntrySuppressionSeconds = 0.85f",
                    "IsPostRecoveryGapEntrySuppressed",
                    "postRecoveryGapEntrySuppressedUntil =",
                    "Time.time + PostRecoveryGapEntrySuppressionSeconds",
                    "lastDodgeStartedAt = -999f",
                    "lastJumpStartedAt = -999f",
                    "forcedGapEntryUntil = -999f"
                },
                "PLAYER_POST_RECOVERY_WALK_GUARD_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                recoveryRelative,
                new[]
                {
                    "holeFallDuration = 2.05f",
                    "holeFallSpeed = 4.60f",
                    "holeFallAccelerationMultiplier = 1.35f",
                    "lavaBounceDistanceMultiplier = 0.80f",
                    "ApplyHazardFeelProfile",
                    "ResolveReducedLavaBounceTarget",
                    "normalizedFall",
                    "TryResolveGroundedCandidate",
                    "IsCandidateSafe"
                },
                "PLAYER_HAZARD_FEEL_CONTRACT_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                controllerRelative,
                new[]
                {
                    "BD C04 EXTERNAL HORSE CONTROL V1",
                    "IsExternallyControlled",
                    "SetExternalControlLock",
                    "MoveByExternalControl",
                    "external control - "
                },
                "HORSE_EXTERNAL_CONTROL_CONTRACT_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                hazardRelative,
                new[]
                {
                    "BD C04 EXHAUSTED FOLLOW SAFE FALLBACK V1",
                    "TryResolveSafePositionNear",
                    "HasHorseClearanceAt",
                    "IsHorsePositionSafe"
                },
                "HORSE_EXHAUSTED_SAFE_FALLBACK_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                repairRelative,
                new[]
                {
                    "BDHorseExhaustedFollowAndPetInteraction"
                },
                "HORSE_EXHAUSTED_RUNTIME_FALLBACK_MISSING"
            );

            ValidateRequiredTokens(
                result,
                root,
                installerRelative,
                new[]
                {
                    "Undo.AddComponent<",
                    "BDHorseExhaustedFollowAndPetInteraction"
                },
                "HORSE_EXHAUSTED_SCENE_WIRING_MISSING"
            );

            ValidateNoStatEffects(
                result,
                root,
                interactionRelative
            );

            ValidateSceneComponent(result);
        }

        private static void ValidateRequiredFile(
            BDOneClickQAResult result,
            string root,
            string relative)
        {
            if (File.Exists(Path.Combine(root, relative)))
                return;

            Add(
                result,
                BDOneClickQASeverity.Blocker,
                "HORSE_EXHAUSTED_PET_FILE_MISSING",
                relative,
                string.Empty,
                "Required exhausted-follow or Pet integration file is missing."
            );
        }

        private static void ValidateRequiredTokens(
            BDOneClickQAResult result,
            string root,
            string relative,
            IEnumerable<string> requiredTokens,
            string findingCode)
        {
            string absolute = Path.Combine(root, relative);

            if (!File.Exists(absolute))
                return;

            string source = File.ReadAllText(absolute);

            foreach (string token in requiredTokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    findingCode,
                    relative,
                    string.Empty,
                    "Required regression anchor is missing: " +
                    token +
                    "."
                );
            }
        }

        private static void ValidateNoStatEffects(
            BDOneClickQAResult result,
            string root,
            string interactionRelative)
        {
            string absolute =
                Path.Combine(root, interactionRelative);

            if (!File.Exists(absolute))
                return;

            string source = File.ReadAllText(absolute);
            string[] forbidden =
            {
                ".Heal(",
                "ApplyDamage(",
                "SetMaxHealth("
            };

            foreach (string token in forbidden)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) < 0)
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "PET_INTERACTION_MODIFIES_STATS",
                    interactionRelative,
                    string.Empty,
                    "Exhausted follow and Pet must not heal, damage, " +
                    "or change maximum health. Forbidden token: " +
                    token
                );
            }
        }

        private static void ValidateSceneComponent(
            BDOneClickQAResult result)
        {
            BDHorseController horse =
                UnityEngine.Object.FindFirstObjectByType<
                    BDHorseController>();

            if (horse == null)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HORSE_EXHAUSTED_PET_HORSE_MISSING",
                    PrototypeScenePath,
                    string.Empty,
                    "The prototype scene does not contain a horse."
                );
                return;
            }

            if (horse.GetComponent<
                    BDHorseExhaustedFollowAndPetInteraction>() != null)
            {
                return;
            }

            Add(
                result,
                BDOneClickQASeverity.Blocker,
                "HORSE_EXHAUSTED_PET_NOT_SERIALIZED",
                PrototypeScenePath,
                horse.name,
                "The prototype horse must serialize the exhausted-follow " +
                "and Pet interaction component."
            );
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets =
                new DirectoryInfo(Application.dataPath);

            if (assets.Parent == null)
                return Application.dataPath;

            return assets.Parent.FullName;
        }

        private static void Add(
            BDOneClickQAResult result,
            BDOneClickQASeverity severity,
            string code,
            string assetPath,
            string objectPath,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    severity,
                    code,
                    assetPath,
                    objectPath,
                    message
                )
            );
        }
    }
}
#endif
