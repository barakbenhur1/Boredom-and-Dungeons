#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDMountedRunIntroQA
    {
        private const string IntroRelative =
            "Assets/_Project/Scripts/Runtime/RunIntro/" +
            "BDMountedRunIntro.cs";

        private const string PortalRelative =
            "Assets/_Project/Scripts/Runtime/RunIntro/" +
            "BDDoorwayPortalVisual.cs";

        public static void Scan(
            BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root =
                ResolveProjectRoot();

            ValidateTokens(
                result,
                root,
                IntroRelative,
                new[]
                {
                    "RuntimeInitializeLoadType.BeforeSceneLoad",
                    "IsGameplayInputLocked",
                    "BeginMountedRunIntro",
                    "CompleteMountedRunIntro",
                    "MoveByExternalControl",
                    "RideDurationSeconds = 2.25f",
                    "ZoomInDurationSeconds = 0.48f",
                    "ZoomOutDurationSeconds = 0.72f",
                    "while (IsAnyGameplayInputHeld())",
                    "CleanupTransientFloorArtifacts",
                    "BD_Enemy_Ranged_Telegraph",
                    "BD_Enemy_Melee_Telegraph",
                    "EnsureDoorwayPortals",
                    "BD_Entrance_Portal",
                    "BD_Exit_Portal",
                    "deathRestartPending",
                    "playerHealth.IsDead",
                    "RunDeathRestartAtSpawn",
                    "if (deathRestartPending)",
                    "rooms.Length == 0"
                }
            );

            ValidateTokens(
                result,
                root,
                PortalRelative,
                new[]
                {
                    "class BDDoorwayPortalVisual",
                    "EnsurePortal",
                    "Portal_Surface_Front",
                    "Portal_Surface_Back",
                    "Portal_Light_Band_",
                    "EmissionColorId",
                    "HideFlags.DontSave"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDHorseController.cs",
                new[]
                {
                    "public bool BeginMountedRunIntro(",
                    "public void CompleteMountedRunIntro()",
                    "mounted run intro",
                    "PlaceRiderOnMountPoint();"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDPlayerCombat.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked",
                    "ResetTransientCombatInputState",
                    "ClearPendingLightPress();",
                    "ClearPendingRangedPress();",
                    "CancelChargedShot();"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/" +
                    "BDPlayerMeleeEnhancer.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked",
                    "ClearBuffer();"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Combat/" +
                    "BDPlayerAirStateTracker.cs",
                new[]
                {
                    "!BDMountedRunIntro.IsGameplayInputLocked",
                    "IsDescendingFromJump"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDEnemyProximityTelegraph.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDEnemyAttackTelegraph.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDEnemyAttackTelegraphVisual.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked"
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/" +
                    "BDHorseExhaustedFollowAndPetInteraction.cs",
                new[]
                {
                    "BDMountedRunIntro.IsGameplayInputLocked",
                    "CancelPetInteractionForRunIntro",
                    "emergencyCancelRequested = true",
                    "ClearPetHold();",
                }
            );

            ValidateTokens(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/" +
                    "BDHealth.cs",
                new[]
                {
                    "ShouldIgnorePlayerDamageDuringRunIntro",
                    "BDMountedRunIntro.IsGameplayInputLocked"
                }
            );
        }

        private static void ValidateTokens(
            BDOneClickQAResult result,
            string root,
            string relative,
            IEnumerable<string> required)
        {
            string absolute =
                Path.Combine(
                    root,
                    relative
                );

            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    "MOUNTED_RUN_INTRO_FILE_MISSING",
                    relative,
                    "Required mounted-run-intro file is missing."
                );
                return;
            }

            string source =
                File.ReadAllText(absolute);

            if (source.IndexOf(
                    "using UnityEditor",
                    StringComparison.Ordinal) >= 0 &&
                relative.Contains(
                    "/Scripts/Runtime/"))
            {
                Add(
                    result,
                    "MOUNTED_RUN_INTRO_EDITOR_DEPENDENCY",
                    relative,
                    "Runtime mounted-intro code must not depend on UnityEditor."
                );
            }

            foreach (string token in required)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    "MOUNTED_RUN_INTRO_CONTRACT_MISSING",
                    relative,
                    "Missing mounted-run-intro contract: " +
                    token +
                    "."
                );
            }
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets =
                new DirectoryInfo(
                    Application.dataPath
                );

            return assets.Parent != null
                ? assets.Parent.FullName
                : Application.dataPath;
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string assetPath,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    assetPath,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
