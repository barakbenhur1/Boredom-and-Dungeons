#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDEarlyRunRegressionRepairQA
    {
        private static readonly Dictionary<string, string[]> Contracts =
            new Dictionary<string, string[]>
            {
                {
                    "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                    new[]
                    {
                        "SetMaxHealth",
                        "BDNewRunFeedbackReset.IsFeedbackSuppressed"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/BDGameFeelEvents.cs",
                    new[]
                    {
                        "ResetTransientFeedback",
                        "BDNewRunFeedbackReset.IsFeedbackSuppressed"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/BDGameFeelAudio.cs",
                    new[]
                    {
                        "StopTransientAudio",
                        "BDNewRunFeedbackReset.IsFeedbackSuppressed"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/BDDamageFlashFeedback.cs",
                    new[]
                    {
                        "ResetTransientFeedback",
                        "BDNewRunFeedbackReset.IsFeedbackSuppressed"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/BDNewRunFeedbackReset.cs",
                    new[]
                    {
                        "SuppressionDuration = 1.50f",
                        "IsFeedbackSuppressed",
                        "CombatInputQuarantineMinimumDuration = 0.20f",
                        "CombatInputQuarantineMaximumDuration = 1.50f",
                        "IsCombatInputSuppressed",
                        "IsAttackInputHeld"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                    new[]
                    {
                        "BDNewRunFeedbackReset.IsCombatInputSuppressed",
                        "ResetTransientCombatInputState",
                        "new run input quarantined"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                    new[]
                    {
                        "BDNewRunFeedbackReset.IsCombatInputSuppressed",
                        "ClearBuffer()"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/Combat/BDPlayerAirStateTracker.cs",
                    new[]
                    {
                        "!BDNewRunFeedbackReset.IsCombatInputSuppressed",
                        "IsDescendingFromJump"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/Horse/BDHorseCleanRunStartGuard.cs",
                    new[]
                    {
                        "StartupCalmSeconds = 3.50f",
                        "StartupMaintenanceSeconds = 1.50f",
                        "horseHealth.ResetForCleanGameStart",
                        "ResetHorse(horseHealth, instance.RemainingCalmSeconds)"
                    }
                },
                {
                    "Assets/_Project/Scripts/Runtime/Horse/BDHorseLocalThreatUtility.cs",
                    new[]
                    {
                        "ResolveActiveRoom",
                        "ContainsWorldPosition",
                        "HasLivingThreatNear",
                        "UnityEngine.Object.FindObjectsByType<BDHealth>",
                        "UnityEngine.Object.FindObjectsByType<BDMinimapRoom>"
                    }
                },
                {
                    "Assets/_Project/Scripts/Editor/Validation/BDPrototypeHazardSceneInstaller.cs",
                    new[]
                    {
                        "MinimumPlayerDistance = 14.0f",
                        "16.0f, 19.0f, 22.0f, 25.0f, 28.0f",
                        "avoidPosition.HasValue ? 24f : 18f"
                    }
                }
            };

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();
            foreach (KeyValuePair<string, string[]> item in Contracts)
            {
                string absolute = Path.Combine(root, item.Key);
                if (!File.Exists(absolute))
                {
                    Add(result, "EARLY_RUN_REPAIR_FILE_MISSING", item.Key,
                        "Required early-run repair file is missing.");
                    continue;
                }

                string source = File.ReadAllText(absolute);
                foreach (string token in item.Value)
                {
                    if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                        continue;

                    Add(result, "EARLY_RUN_REPAIR_CONTRACT_MISSING", item.Key,
                        "Missing early-run repair contract: " + token + ".");
                }
            }

            ValidateNoConfigurationHitFeedback(result, root);
            ValidateNoRemoteEncounterFlee(result, root);
            ValidateCompileRepairContracts(result, root);
            ValidateNewGameClickLeakContracts(result, root);
        }

        private static void ValidateNoConfigurationHitFeedback(
            BDOneClickQAResult result,
            string root)
        {
            string relative = "Assets/_Project/Scripts/Runtime/BDHealth.cs";
            string source = File.ReadAllText(Path.Combine(root, relative));
            int method = source.IndexOf("public void SetMaxHealth", StringComparison.Ordinal);
            int next = source.IndexOf("public void ApplyDamage", method, StringComparison.Ordinal);
            if (method >= 0 && next > method)
            {
                string setMaxHealth = source.Substring(method, next - method);
                if (setMaxHealth.IndexOf("RequestDamageCameraShake", StringComparison.Ordinal) >= 0)
                {
                    Add(result, "CONFIGURATION_TRIGGERS_HIT_FEEDBACK", relative,
                        "SetMaxHealth still triggers damage camera/audio feedback.");
                }
            }
        }

        private static void ValidateNoRemoteEncounterFlee(
            BDOneClickQAResult result,
            string root)
        {
            string[] relatives =
            {
                "Assets/_Project/Scripts/Runtime/BDHorseCombatFleeController.cs",
                "Assets/_Project/Scripts/Runtime/BDHorseReliableFleeMotor.cs"
            };

            foreach (string relative in relatives)
            {
                string source = File.ReadAllText(Path.Combine(root, relative));
                if (source.IndexOf("FindObjectsByType<BDRoomEncounter>", StringComparison.Ordinal) >= 0)
                {
                    Add(result, "REMOTE_ENCOUNTER_STILL_TRIGGERS_HORSE_FLEE", relative,
                        "Horse flee still scans every unfinished room encounter.");
                }

                if (source.IndexOf("BDHorseLocalThreatUtility.HasLivingThreatNear", StringComparison.Ordinal) < 0)
                {
                    Add(result, "LOCAL_HORSE_THREAT_CONTRACT_MISSING", relative,
                        "Horse flee does not use the same-room local-threat utility.");
                }
            }
        }

        private static void ValidateCompileRepairContracts(
            BDOneClickQAResult result,
            string root)
        {
            string guardRelative =
                "Assets/_Project/Scripts/Runtime/Horse/" +
                "BDHorseCleanRunStartGuard.cs";
            string guardSource =
                File.ReadAllText(
                    Path.Combine(root, guardRelative)
                );

            if (guardSource.IndexOf(
                    "instance.ResetHorse(",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    "HORSE_CLEAN_START_STATIC_CALL_INVALID",
                    guardRelative,
                    "Static ResetHorse must be called directly."
                );
            }

            string utilityRelative =
                "Assets/_Project/Scripts/Runtime/Horse/" +
                "BDHorseLocalThreatUtility.cs";
            string utilitySource =
                File.ReadAllText(
                    Path.Combine(root, utilityRelative)
                );

            string[] required =
            {
                "UnityEngine.Object.FindObjectsByType<BDHealth>",
                "UnityEngine.Object.FindObjectsByType<BDMinimapRoom>"
            };

            foreach (string token in required)
            {
                if (utilitySource.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    "HORSE_LOCAL_THREAT_OBJECT_API_UNQUALIFIED",
                    utilityRelative,
                    "Missing qualified UnityEngine.Object query: " +
                    token +
                    "."
                );
            }
        }

        private static void ValidateNewGameClickLeakContracts(
            BDOneClickQAResult result,
            string root)
        {
            string combatRelative =
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs";
            string combatSource =
                File.ReadAllText(
                    Path.Combine(
                        root,
                        combatRelative
                    )
                );

            if (combatSource.IndexOf(
                    "BDNewRunFeedbackReset.IsCombatInputSuppressed",
                    StringComparison.Ordinal) < 0)
            {
                Add(
                    result,
                    "NEW_GAME_COMBAT_INPUT_GATE_MISSING",
                    combatRelative,
                    "Combat can still consume the UI click " +
                    "that loaded the new run."
                );
            }

            string enhancerRelative =
                "Assets/_Project/Scripts/Runtime/Combat/" +
                "BDPlayerMeleeEnhancer.cs";
            string enhancerSource =
                File.ReadAllText(
                    Path.Combine(
                        root,
                        enhancerRelative
                    )
                );

            if (enhancerSource.IndexOf(
                    "BDNewRunFeedbackReset.IsCombatInputSuppressed",
                    StringComparison.Ordinal) < 0)
            {
                Add(
                    result,
                    "NEW_GAME_LANDING_ATTACK_GATE_MISSING",
                    enhancerRelative,
                    "Landing/melee enhancer can still consume " +
                    "the New Game button click."
                );
            }
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets = new DirectoryInfo(Application.dataPath);
            return assets.Parent != null ? assets.Parent.FullName : Application.dataPath;
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string assetPath,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                assetPath,
                string.Empty,
                message
            ));
        }
    }
}
#endif
