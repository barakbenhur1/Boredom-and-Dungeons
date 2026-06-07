#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDHorseCleanRunStartQA
    {
        private const string GuardRelative =
            "Assets/_Project/Scripts/Runtime/Horse/BDHorseCleanRunStartGuard.cs";
        private const string UtilityRelative =
            "Assets/_Project/Scripts/Runtime/Horse/BDHorseLocalThreatUtility.cs";
        private const string ControllerRelative =
            "Assets/_Project/Scripts/Runtime/BDHorseController.cs";
        private const string HealthRelative =
            "Assets/_Project/Scripts/Runtime/BDHorseHealth.cs";
        private const string CombatFleeRelative =
            "Assets/_Project/Scripts/Runtime/BDHorseCombatFleeController.cs";
        private const string ReliableFleeRelative =
            "Assets/_Project/Scripts/Runtime/BDHorseReliableFleeMotor.cs";
        private const string RepairRelative =
            "Assets/_Project/Scripts/Runtime/BDHorseRuntimeRepair.cs";
        private const string HazardRelative =
            "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();

            ValidateTokens(result, root, GuardRelative, new[]
            {
                "RuntimeInitializeLoadType.BeforeSceneLoad",
                "StartupCalmSeconds = 3.50f",
                "StartupMaintenanceSeconds = 1.50f",
                "SceneManager.sceneLoaded",
                "RegisterHorse",
                "RemainingCalmSeconds",
                "ResetHorse(horseHealth, instance.RemainingCalmSeconds)"
            });

            string guardSource = Read(root, GuardRelative);
            RejectToken(
                result,
                GuardRelative,
                guardSource,
                "instance.ResetHorse(",
                "HORSE_CLEAN_START_STATIC_CALL_INVALID",
                "Static ResetHorse must be called directly."
            );

            ValidateTokens(result, root, UtilityRelative, new[]
            {
                "ResolveActiveRoom",
                "ContainsWorldPosition",
                "HasLivingThreatNear",
                "UnityEngine.Object.FindObjectsByType<BDHealth>",
                "UnityEngine.Object.FindObjectsByType<BDMinimapRoom>",
                "FindObjectsInactive.Exclude",
                "FindObjectsSortMode.None"
            });

            ValidateTokens(result, root, ControllerRelative, new[]
            {
                "public bool IsStartupCalm",
                "public void ResetForCleanGameStart(",
                "startup calm - ignored safe spot request",
                "BDHorseLocalThreatUtility.HasLivingThreatNear",
                "HasLivingEnemyNearHorseOrPlayer",
                "BD DEATH RESTART HORSE ROOT GROUNDING V15",
                "ResolveHorseRootPositionFromGroundAnchor",
                "controller.center.y",
                "controller.height * 0.5f",
                "controller.skinWidth * 0.625f",
                "grounded = hit.point;"
            });

            ValidateTokens(result, root, HealthRelative, new[]
            {
                "public void ResetForCleanGameStart(",
                "currentHealth = maxHealth;",
                "recentHits = 0;",
                "BDHorseCleanRunStartGuard.IsActive"
            });

            string combatSource = ValidateTokens(
                result,
                root,
                CombatFleeRelative,
                new[]
                {
                    "suppressedUntil",
                    "horse.IsStartupCalm",
                    "localThreatRadius",
                    "BDHorseLocalThreatUtility.HasLivingThreatNear",
                    "ResetForCleanGameStart("
                }
            );

            string reliableSource = ValidateTokens(
                result,
                root,
                ReliableFleeRelative,
                new[]
                {
                    "suppressedUntil",
                    "horseController.IsStartupCalm",
                    "nearbyEnemyDangerRadius",
                    "BDHorseLocalThreatUtility.HasLivingThreatNear",
                    "ResetForCleanGameStart("
                }
            );

            RejectGlobalEncounterTrigger(
                result,
                CombatFleeRelative,
                combatSource
            );
            RejectGlobalEncounterTrigger(
                result,
                ReliableFleeRelative,
                reliableSource
            );

            ValidateTokens(result, root, RepairRelative, new[]
            {
                "BDHorseCleanRunStartGuard.RegisterHorse(",
                "horseHealth"
            });

            ValidateTokens(result, root, HazardRelative, new[]
            {
                "public void ResetForCleanGameStart(",
                "hazardRetreatRemainingDistance = 0f",
                "nextHazardPollAt =",
                "safePointUpdatesBlockedUntil ="
            });
        }

        private static string ValidateTokens(
            BDOneClickQAResult result,
            string root,
            string relative,
            IEnumerable<string> required)
        {
            string source = Read(root, relative);

            if (string.IsNullOrEmpty(source))
            {
                Add(
                    result,
                    "HORSE_CLEAN_RUN_START_FILE_MISSING",
                    relative,
                    "Required horse clean-start file is missing."
                );
                return string.Empty;
            }

            if (source.IndexOf(
                    "using UnityEditor",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    "HORSE_CLEAN_RUN_START_EDITOR_DEPENDENCY",
                    relative,
                    "Runtime horse code must not depend on UnityEditor."
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
                    "HORSE_CLEAN_RUN_START_CONTRACT_MISSING",
                    relative,
                    "Missing current horse clean-start contract: " +
                    token +
                    "."
                );
            }

            return source;
        }

        private static void RejectGlobalEncounterTrigger(
            BDOneClickQAResult result,
            string relative,
            string source)
        {
            RejectToken(
                result,
                relative,
                source,
                "FindObjectsByType<BDRoomEncounter>",
                "HORSE_REMOTE_ENCOUNTER_FLEE_TRIGGER",
                "Horse flee still scans remote encounters."
            );

            RejectToken(
                result,
                relative,
                source,
                "!encounter.IsComplete && encounter.LiveEnemies > 0",
                "HORSE_REMOTE_ENCOUNTER_FLEE_TRIGGER",
                "Horse flee still reacts to remote encounters."
            );
        }

        private static void RejectToken(
            BDOneClickQAResult result,
            string relative,
            string source,
            string forbidden,
            string code,
            string message)
        {
            if (source.IndexOf(
                    forbidden,
                    StringComparison.Ordinal) < 0)
            {
                return;
            }

            Add(
                result,
                code,
                relative,
                message
            );
        }

        private static string Read(
            string root,
            string relative)
        {
            string absolute =
                Path.Combine(root, relative);

            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
                : string.Empty;
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets =
                new DirectoryInfo(Application.dataPath);

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
